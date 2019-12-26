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
    const source = new EventSource('/sse/stream?channel=iconlook');
    var leaderBlockCount = parseInt($('.leader-block-count').text());
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
                if (message.cmd === 'PeersUpdatedSignal') {
                    if (json.busy != null) {
                        json.busy.forEach(function(item, index) {
                            var id = item.peerId.toString();
                            if ($('.peer-state-' + id + ' span').text() === 'IDLE') {
                                $('.peer-state-' + id + ' span').text('BUSY');
                                $('.peer-name-' + id).addClass('font-weight-bold');
                                $('.peer-state-' + id).addClass('peer-state-busy');
                                $('.peer-state-' + id).closest('tr').hide().fadeIn(500);
                            }
                            if (index === 0 && $('.leader-name').text() !== item.name) {
                                $('.leader').hide().fadeIn(500);
                                $('.leader-name').text(item.name);
                                $('.leader-logo').attr('src', item.logoUrl);
                                $('.leader-ranking').text('#' + item.ranking);
                                $('.leader-block-count').text(leaderBlockCount);
                                if (item.madeBlockCount < 10) {
                                    leaderBlockCount = item.madeBlockCount;
                                } else {
                                    leaderBlockCount = 1;
                                    $('.leader-block-made').text('❚');
                                    $('.leader-block-count').text('1');
                                    $('.leader-block-remaining').text('❚❚❚❚❚❚❚❚❚');
                                }
                            }
                            $('.peer-state span').not('.peer-state-' + id + ' span').text('IDLE');
                            $('.peer-name').not('.peer-name-' + id).removeClass('font-weight-bold');
                            $('.peer-state').not('.peer-state-' + id).removeClass('peer-state-busy');
                        });
                    }
                }
                if (message.cmd === 'BlockProducedSignal') {
                    var leaderBlockMade = '';
                    var leaderBlockRemaining = '';
                    for (m = 0; m < leaderBlockCount; m++) {
                        leaderBlockMade += '❚';
                    }
                    for (r = leaderBlockCount; r < 10; r++) {
                        leaderBlockRemaining += '❚';
                    }
                    $('.leader-block-made').text(leaderBlockMade);
                    $('.leader-block-count').text(leaderBlockCount);
                    if (leaderBlockCount < 10) leaderBlockCount += 1;
                    $('.leader-block-remaining').text(leaderBlockRemaining);
                    if ($('#block_grid .e-content tr').length > 0) {
                        if ($('#block_grid .e-content tr').length >= 22) {
                            $('#block_grid .e-content tr:last').remove();
                        }
                        var row = $('#block_grid .e-content tr:first').clone().hide().css('opacity', 0);
                        $(row).find('.BlockResponse_TotalAmount').decimal(json.block.totalAmount, 4);
                        $(row).find('.BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                        $(row).find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                        $(row).find('.BlockResponse_TransactionCount').integer(json.block.transactionCount, '0000');
                        row.prependTo($('#block_grid .e-content tbody:first')).slideDown(250).animate({ opacity: 1 }, { queue: false, duration: 750 });
                    }
                }
            }
        }
    });
});