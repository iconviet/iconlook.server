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
    source.addEventListener('error',
        function(e) {
            console.log("ERROR", e);
        },
        false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function() {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function(selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                if (message.cmd === 'BlockchainUpdatedSignal') {
                    $('.BlockchainResponse_MarketCap').integer(json.blockchain.marketCap);
                    $('.BlockchainResponse_IcxSupply').integer(json.blockchain.icxSupply);
                    $('.BlockchainResponse_BlockHeight').integer(json.blockchain.blockHeight);
                    $('.BlockchainResponse_TotalStaked').integer(json.blockchain.totalStaked);
                    $('.BlockchainResponse_IcxCirculation').integer(json.blockchain.icxCirculation);
                    $('.BlockchainResponse_PublicTreasury').integer(json.blockchain.publicTreasury);
                    $('.BlockchainResponse_TotalDelegated').integer(json.blockchain.totalDelegated);
                    $('.BlockchainResponse_TransactionCount').integer(json.blockchain.transactionCount);
                }
                if (message.cmd === 'BlockProducedSignal') {
                    var row = $('#block_grid_content_table tr').first().clone().hide();
                    $(row).find('.BlockResponse_BlockHeight').integer(json.block.height);
                    $(row).find('.BlockResponse_TotalAmount').decimal(json.block.totalAmount, 4);
                    $(row).find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                    $(row).find('.BlockResponse_TransactionCount').integer(json.block.transactionCount, '0000');
                    $(row).prependTo($('#block_grid_content_table tbody')).slideDown(250, function() {
                        if ($('#block_grid_content_table tr').length === 14) {
                            $('#block_grid_content_table tr').last().remove();
                        }
                    });
                }
            }
        }
    });
    const click = window.rxjs.fromEvent(document, 'click');
    const example = click.pipe(window.rxjs.operators.map(event => '[ICONLOOK] Event time: ' + event.timeStamp));
    example.subscribe(val => console.log(val));
});