$(document).ready(function() {
    const source = new EventSource('/sse/stream?channel=iconlook');
    source.addEventListener('error', function (e) {
        console.log("ERROR", e);
    }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function() {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function(selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                // console.log("Received " + message.cmd, json);
                if (message.cmd === 'BlockProducedSignal') {
                    var row = $('#block_grid_content_table tr').first().clone();
                    $(row).find('.BlockResponse_TotalAmount').animateNumber({
                        number: json.block.totalAmount * Math.pow(10, 4),
                        numberStep: function(now, tween) {
                            $(tween.elem).text((Math.floor(now) / Math.pow(10, 4)).toFixed(4));
                        }
                    });
                    $(row).find('.BlockResponse_TransactionCount').animateNumber({
                        number: json.block.transactionCount,
                        numberStep: $.animateNumber.numberStepFactories.separator(',')
                    });
                    $(row).find('.BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                    $(row).find('.BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                    $(row).hide().prependTo($('#block_grid_content_table tbody'));
                    $(row).fadeIn(750, function() {
                        if ($('#block_grid_content_table tr').length === 14) {
                            $('#block_grid_content_table tr').last().remove();
                        }
                    });
                }
                if (message.cmd === 'BlockchainUpdatedSignal') {
                    $('.BlockchainResponse_IcxSupply').animateNumber({
                        number: json.blockchain.icxSupply,
                        numberStep: $.animateNumber.numberStepFactories.separator(',')
                    });
                    $('.BlockchainResponse_IcxCirculation').animateNumber({
                        number: json.blockchain.icxCirculation,
                        numberStep: $.animateNumber.numberStepFactories.separator(',')
                    });
                    $('.BlockchainResponse_PublicTreasury').animateNumber({
                        number: json.blockchain.publicTreasury,
                        numberStep: $.animateNumber.numberStepFactories.separator(',')
                    });
                    $('.BlockchainResponse_TransactionCount').animateNumber({
                        number: json.blockchain.transactionCount,
                        numberStep: $.animateNumber.numberStepFactories.separator(',')
                    });
                    $('.BlockchainResponse_MarketCap').text(json.blockchain.marketCap.toLocaleString());
                    $('.BlockchainResponse_BlockHeight').text(json.blockchain.blockHeight.toLocaleString());
                    $('.BlockchainResponse_TotalStaked').text(json.blockchain.totalStaked.toLocaleString());
                    $('.BlockchainResponse_TotalDelegated').text(json.blockchain.totalDelegated.toLocaleString());
                }
            }
        }
    });
    const click = window.rxjs.fromEvent(document, 'click');
    const example = click.pipe(window.rxjs.operators.map(event => '[ICONLOOK] Event time: ' + event.timeStamp));
    example.subscribe(val => console.log(val));
})