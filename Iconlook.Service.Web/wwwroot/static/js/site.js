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
    source.addEventListener('error', function(e) { console.log("ERROR", e); }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function() {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function(selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                if (message.cmd === 'PeersUpdatedSignal') {
                    console.log("Received " + message.cmd, json);
                }
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
                    if ($('#block_grid_content_table tr').length > 0) {
                        var row = $('#block_grid_content_table tr').first().clone().hide();
                        $(row).find('.BlockResponse_TotalAmount').decimal(json.block.totalAmount, 4);
                        $(row).find('.BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                        $(row).find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                        $(row).find('.BlockResponse_TransactionCount').integer(json.block.transactionCount, '0000');
                        $(row).prependTo($('#block_grid_content_table tbody')).slideDown(250, function() {
                            if ($('#block_grid_content_table tr').length >= 14) {
                                $('#block_grid_content_table tr').last().remove();
                            }
                        });
                    }
                }
            }
        }
    });
});