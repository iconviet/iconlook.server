$(document).ready(function() {
    console.log("Ready!!!!!");
    const source = new EventSource('/sse/stream?channel=iconlook');
    source.addEventListener('error', function (e) {
        console.log("ERROR", e);
    }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function (subscription) {
                console.log("Connected ***", subscription);
            }
        },
        success: function (selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                const block_grid = document.getElementById('block_grid');
                if (block_grid != null && block_grid.ej2_instances != null) {
                    if (message.cmd === 'BlockProducedSignal') {
                        const instance = block_grid.ej2_instances[0];
                        instance.addRecord(json.block);
                        // console.log("Received " + message.cmd, json);
                    }
                }

            }
        }
    });
    const click = window.rxjs.fromEvent(document, 'click');
    const example = click.pipe(window.rxjs.operators.map(event => 'Event time: ${event.timeStamp}'));
    example.subscribe(val => console.log(val));
})