

window.websocketInterop = {

        socket: null,

        connect: function (url, helper, msg) {
                console.log("Connecting");
                socket = new WebSocket(url);

                socket.onopen = function (evt) {
                        msg && socket.send(msg);
                }
                socket.onmessage = function (event) {
                        console.debug("WebSocket message received:", event);
                        helper.invokeMethod("OnMessage", event.data);
                };
                socket.onclose = function (evt) {
                        console.log("Socket closed. Notify this..");
                        helper.invokeMethod("OnChannelClose");
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