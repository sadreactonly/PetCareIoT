using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace PetCareIoTMiddleware.Middleware
{
    public class WebSocketServerConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        /// <summary>
        /// Collection of all active sockets.
        /// </summary>
        public ConcurrentDictionary<string, WebSocket> AllSockets => _sockets;

        /// <summary>
        /// Add websocket to collection.
        /// </summary>
        /// <param name="socket">Websocket to add.</param>
        /// <returns>Connection ID.</returns>
        public string AddSocket(WebSocket socket)
        {
            string connectionId = Guid.NewGuid().ToString();
            _sockets.TryAdd(connectionId, socket);

            return connectionId;
        }
    }
}