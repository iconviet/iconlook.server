$(document).ready(function() {
    const source = new EventSource('/sse/stream?channel=iconlook');
    source.addEventListener('error', function (e) {
        console.log("ERROR", e);
    }, false);
    $(source).handleServerEvents({
        handlers: {
            onConnect: function (subscription) {
                console.log("[ICONLOOK] Channel connected.");
            }
        },
        success: function (selector, json, message) {
            if (!selector.startsWith('cmd.on')) {
                if (message.cmd === 'BlockProducedSignal') {
                    // console.log("Received " + message.cmd, json);
                }

            }
        }
    });
    const click = window.rxjs.fromEvent(document, 'click');
    const example = click.pipe(window.rxjs.operators.map(event => '[ICONLOOK] Event time: ' + event.timeStamp));
    // example.subscribe(val => console.log(val));
})