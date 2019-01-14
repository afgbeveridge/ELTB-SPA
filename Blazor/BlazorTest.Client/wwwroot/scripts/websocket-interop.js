

window.websocketInterop = {

        socket: null,

        connect: function (helper) {
                console.log("Connecting");
                socket = new WebSocket("ws://localhost:55444/api/EsotericLanguage/execute");
                socket.onmessage = function (event) {
                        console.debug("WebSocket message received:", event);
                        helper.invokeMethod("OnMessage", event.data);
                };
                socket.onclose = function (evt) {
                        console.log("Socket closed. Re-open");
                        helper.invokeMethod("OnReconnect");
                        window.websocketInterop.connect(helper);
                }
                console.log("Connected and ready....");
        },

        send: function (msg) {
                console.log("Sending:" + msg);
                socket.send(msg);
        },

        close: function () {
                console.log("Closing socket on demand");
                socket && socket.close();
        }

};