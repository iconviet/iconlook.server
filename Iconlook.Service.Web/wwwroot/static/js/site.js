$(document).ready(function() {
    const source = new EventSource('/sse/stream?channel=iconlook');
    source.addEventListener('error', function (e) {
        console.log("ERROR", e);
    }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function(subscription) {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function(selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                console.log("Received " + message.cmd, json);
                if (message.cmd === 'BlockchainUpdatedSignal') {
                    $('.BlockchainResponse_MarketCap').text(json.blockchain.marketCap.toLocaleString());
                    $('.BlockchainResponse_IcxSupply').text(json.blockchain.icxSupply.toLocaleString());
                    $('.BlockchainResponse_BlockHeight').text(json.blockchain.blockHeight.toLocaleString());
                    $('.BlockchainResponse_TotalStaked').text(json.blockchain.totalStaked.toLocaleString());
                    $('.BlockchainResponse_TotalDelegated').text(json.blockchain.totalDelegated.toLocaleString());
                    $('.BlockchainResponse_IcxCirculation').text(json.blockchain.icxCirculation.toLocaleString());
                    $('.BlockchainResponse_PublicTreasury').text(json.blockchain.publicTreasury.toLocaleString());
                    $('.BlockchainResponse_TransactionCount').text(json.blockchain.transactionCount.toLocaleString());
                }
                if (message.cmd === 'BlockProducedSignal') {
                    if ($('#block_grid_content_table > tbody > tr').length === 13) {
                        $('#block_grid_content_table .BlockResponse_BlockHeight').text(json.block.height.toLocaleString());
                        $('#block_grid_content_table .BlockResponse_BlockHash').text(json.block.hash.substring(0, 16) + '..');
                        $('#block_grid_content_table .BlockResponse_TransactionCount').text(json.block.transactionCount.toLocaleString());
                        $('#block_grid_content_table .BlockResponse_TotalAmount').text(json.block.totalAmount.toLocaleString());
                    }
                }
                
            }
        }
    });
    const click = window.rxjs.fromEvent(document, 'click');
    const example = click.pipe(window.rxjs.operators.map(event => '[ICONLOOK] Event time: ' + event.timeStamp));
    example.subscribe(val => console.log(val));
})