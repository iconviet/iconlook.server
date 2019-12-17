jQuery.fn.extend({
    integer: function(number, padding) {
        return this.each(function() {
            if (padding == null) {
                $(this).animateNumber({
                    number: number,
                    numberStep: $.animateNumber.numberStepFactories.separator(',')
                });
            } else {
                $(this).animateNumber({
                    number: number,
                    numberStep: function(now, tween) {
                        var text = padding + Math.round(now).toString();
                        text = text.substring(Math.round(now).toString().length);
                        $(tween.elem).prop('number', Math.round(now)).text(text);
                    }
                });
            }
        });
    },
    decimal: function(number, fraction) {
        return this.each(function() {
            var factor = Math.pow(10, fraction);
            $(this).animateNumber({
                number: number * factor,
                numberStep: function(now, tween) {
                    $(tween.elem).text((Math.floor(now) / factor).toFixed(fraction));
                }
            });
        });
    }
});
$(document).ready(function() {
    var currentPeerBlockCount = 1;
    const source = new EventSource('/sse/stream?channel=iconlook');
    source.addEventListener('error', function(e) { console.log("ERROR", e); }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function() {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function(selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                if (message.cmd === 'ChainUpdatedSignal') {
                    $('.ChainResponse_MarketCap').integer(json.chain.marketCap);
                    $('.ChainResponse_IcxSupply').integer(json.chain.icxSupply);
                    $('.ChainResponse_BlockHeight').integer(json.chain.blockHeight);
                    $('.ChainResponse_TotalStaked').integer(json.chain.totalStaked);
                    $('.ChainResponse_IcxCirculation').integer(json.chain.icxCirculation);
                    $('.ChainResponse_PublicTreasury').integer(json.chain.publicTreasury);
                    $('.ChainResponse_TotalDelegated').integer(json.chain.totalDelegated);
                    $('.ChainResponse_TransactionCount').integer(json.chain.transactionCount);
                }
                if (message.cmd === 'BlockProducedSignal') {
                    var currentPeerBlockMade = '';
                    var currentPeerBlockRemaining = '';
                    for (m = 0; m < currentPeerBlockCount; m++) {
                        currentPeerBlockMade += '❚';
                    }
                    for (r = currentPeerBlockCount; r < 10; r++) {
                        currentPeerBlockRemaining += '❚';
                    }
                    $('.current-peer-block-made').text(currentPeerBlockMade);
                    $('.current-peer-block-count').text(currentPeerBlockCount);
                    $('.current-peer-block-remaining').text(currentPeerBlockRemaining);
                    if (currentPeerBlockCount < 10) currentPeerBlockCount = currentPeerBlockCount + 1;
                    if ($('#block_grid_content_table tr').length > 0) {
                        var row = $('#block_grid_content_table tr').first().clone().hide();
                        $(row).prependTo($('#block_grid_content_table tbody')).slideDown(250,
                            function() {
                            if ($('#block_grid_content_table tr').length >= 23) {
                                $('#block_grid_content_table tr').last().remove();
                            }
                        });
                        $(row).find('.BlockResponse_TotalAmount').decimal(json.block.totalAmount, 4);
                        $(row).find('.BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                        $(row).find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                        $(row).find('.BlockResponse_TransactionCount').integer(json.block.transactionCount, '0000');
                    }
                }
                if (message.cmd === 'PeersUpdatedSignal') {
                    if (json.busy != null) {
                        var id = json.busy.peerId.toString();
                        if ($('.peer-state-' + id + ' span').text() === 'IDLE') {
                            $('.peer-state-' + id + ' span').text('BUSY');
                            $('.peer-state-' + id).addClass('peer-state-busy');
                            $('.peer-state-' + id).closest('tr').hide().fadeIn(500);
                        }
                        if ($('.current-peer-name').text() !== json.busy.name) {
                            $('.current-peer').hide().fadeIn(500);
                            $('.current-peer-name').text(json.busy.name);
                            $('.current-peer-block-count').text(currentPeerBlockCount);
                            if (json.busy.madeBlockCount < 10) {
                                currentPeerBlockCount = json.busy.madeBlockCount;
                            } else {
                                currentPeerBlockCount = 1;
                                $('.current-peer-block-made').text('❚');
                                $('.current-peer-block-count').text('1');
                                $('.current-peer-block-remaining').text('❚❚❚❚❚❚❚❚❚');
                            }
                        }
                        $('.peer-state span').not('.peer-state-' + id + ' span').text('IDLE');
                        $('.peer-state').not('.peer-state-' + id).removeClass('peer-state-busy');
                    }
                }
            }
        }
    });
});