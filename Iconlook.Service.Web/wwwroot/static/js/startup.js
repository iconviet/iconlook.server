$(document).ready(function() {
    console.log("Ready!!!!!");
    var source = new EventSource('/sse/stream?channel=iconlook');
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
                console.log("Received " + message.cmd, json);
            }
        }
    });
    const click = rxjs.fromEvent(document, 'click');
    const example = click.pipe(rxjs.operators.map(event => `Event time: ${event.timeStamp}`));
    example.subscribe(val => console.log(val));
})