using System;
using System.Text;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using SharedObjects.Esoterica;
using System.Composition;

namespace ELTB.Services {

    public class LocalIOWrapper : IOWrapper {

        private WebSocket Channel { get; set; }
        private CancellationTokenSource LocalToken { get; set; }

        internal LocalIOWrapper(WebSocket socket, CancellationTokenSource local) {
            Channel = socket;
            LocalToken = local;
        }

        public async Task<char> ReadCharacter() {
            return (await Receive()).First();
        }
        public async Task<string> ReadString(string defaultIfEmpty = "") {
            return await Receive() ?? defaultIfEmpty;
        }
        public async Task Write(string src) {
            await Send(src);
        }
        public async Task Write(char c) {
            await Send(new string(c, 1));
        }

        private async Task Send(string content) {
            var outputBuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(content));
            await Channel.SendAsync(outputBuffer, WebSocketMessageType.Text, true, LocalToken.Token);
        }

        internal async Task<string> Receive(bool promptRemote = true) {
            if (promptRemote)
                await Send("\t");
            byte[] buffer = new byte[2048];
            var response = await Channel.ReceiveAsync(new ArraySegment<byte>(buffer), LocalToken.Token);
            // TODO: Ensure complete message is received
            return Encoding.ASCII.GetString(buffer, 0, response.Count);
        }
    }
}
