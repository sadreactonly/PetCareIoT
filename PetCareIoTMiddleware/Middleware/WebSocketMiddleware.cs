using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PetCareIoTMiddleware.Authentication;
using PetCareIoTMiddleware.Helpers;
using PetCareIoTMiddleware.Models;
using PetCareIoTMiddleware.Services;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetCareIoTMiddleware.Middleware
{
    /// <summary>
    /// Represents middleware.
    /// </summary>
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketServerConnectionManager _manager;
        private readonly TokenValidator _tokenValidator;
        private readonly MessageProcessor _messageProcessor;

        /// <summary>
        /// Create new WebSocketServerMiddleware object.
        /// </summary>
        /// <param name="next">Function that can process Http request.</param>
        /// <param name="manager">Connection manager.</param>
        /// <param name="tokenValidator">Class responsible for token validtion.</param>
        public WebSocketServerMiddleware(RequestDelegate next, WebSocketServerConnectionManager manager, TokenValidator tokenValidator, MessageProcessor messageProcessor)
        {
            _next = next;
            _manager = manager;
            _tokenValidator = tokenValidator;
            _messageProcessor = messageProcessor;
        }

        /// <summary>
        /// Process websocket request.
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                //if (context.Request.Headers.ContainsKey("token"))
                //{
                //    if (!_tokenValidator.ValidateCurrentToken((context.Request.Headers["token"])))
                //        return;
                //}

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                string connectionId = _manager.AddSocket(webSocket);

                await SendConnectionIdAsync(webSocket, connectionId);

                await ReceiveMessage(webSocket, async (result, buffer) =>
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        await RouteMessageAsync(Encoding.UTF8.GetString(buffer, 0, result.Count));
                        return;
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        string id = _manager.AllSockets.FirstOrDefault(s => s.Value == webSocket).Key;
                        _manager.AllSockets.TryRemove(id, out WebSocket sock);

                        await sock.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                        return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }

        /// <summary>
        /// Send connection ID to client.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="connID"></param>
        /// <returns></returns>
        private async Task SendConnectionIdAsync(WebSocket socket, string connID)
        {
            var buffer = Encoding.UTF8.GetBytes("ConnectionId: " + connID);
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Recive message handler.
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="handleMessage"></param>
        /// <returns></returns>
        private async Task ReceiveMessage(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }

        /// <summary>
        /// Route message to other clients.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task RouteMessageAsync(string message)
        {
            // Process message
            var messageRaw = JsonConvert.DeserializeObject<Message>(message);

            if (Guid.TryParse(messageRaw.To, out Guid guidOutput))
            {
                var sock = _manager.AllSockets.FirstOrDefault(s => s.Key == messageRaw.To);

                if (sock.Value != null)
                {
                    if (sock.Value.State == WebSocketState.Open)
                    {
                        await sock.Value.SendAsync(Encoding.UTF8.GetBytes(messageRaw.Data.ToJson()),
                           WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            else
            {
                foreach (var sock in _manager.AllSockets)
                {
                    if (sock.Value.State == WebSocketState.Open)
                    {
                        await sock.Value.SendAsync(Encoding.UTF8.GetBytes(messageRaw.Data.ToJson()),
                            WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }

            _messageProcessor.ProcessData(messageRaw.Data);
        }
    }
}