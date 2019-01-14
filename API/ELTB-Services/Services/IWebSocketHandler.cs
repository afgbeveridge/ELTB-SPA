using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.WebSockets;

namespace ELTB.Services {

    public interface IWebSocketHandler {
        Task<IWebSocketHandler> Host(WebSocket socket);
        Task Process();
    }

}
