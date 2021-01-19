using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PetCareIoT.Models;
using PetCareIoT.Extensions;
namespace PetCareIoT.Communication
{
    public static class SocketCommunicationService
    {
        /// <summary>
        /// Start water pump.
        /// </summary>
        /// <returns>
        /// True if it's done, false if fails.
        /// </returns>
        public static async Task<bool> StartPump()
        {
            using var socket = new ClientWebSocket();
            try
            {
                //await socket.ConnectAsync(new Uri("ws://sadreactonly-001-site1.dtempurl.com"), CancellationToken.None);
                await socket.ConnectAsync(new Uri("ws://192.168.0.132:61955"), CancellationToken.None);
                var message = CreateMessage(EventType.PUMP_START);
                var json = message.ToJson();
                await Send(socket, message.ToJson());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - {ex.Message}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Start feeder pump.
        /// </summary>
        /// <returns>
        /// True if it's done, false if fails.
        /// </returns>
        public static async Task<bool> StartFeeder()
        {
            using var socket = new ClientWebSocket();
            
            try
            {
                //await socket.ConnectAsync(new Uri("ws://sadreactonly-001-site1.dtempurl.com"), CancellationToken.None);
                await socket.ConnectAsync(new Uri("ws://192.168.0.132:61955"), CancellationToken.None);

                var message = CreateMessage(EventType.FEEDER_START);

                await Send(socket, message.ToJson());
                // await Receive(socket);      
              
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR - {ex.Message}");
                return false;
            }
            return true;
        }
        public static async Task SetLight(int state)
        {

            using var socket = new ClientWebSocket();
            try
            {
                //await socket.ConnectAsync(new Uri("ws://sadreactonly-001-site1.dtempurl.com"), CancellationToken.None);
                await socket.ConnectAsync(new Uri("ws://192.168.0.132:61955"), CancellationToken.None);

                var message = CreateMessage((EventType)state); 
                await Send(socket, message.ToJson());
                // await Receive(socket);

            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"ERROR - {ex.Message}");
            }
        }

        private static Message CreateMessage(EventType type)
        {
            return new Message()
            {
                From = "Arduino",
                Data = new Data
                {
                    RequestId = Guid.NewGuid().ToString(),
                    Type = type
                }
            };
        }

        private static async Task Send(ClientWebSocket socket, string data)
        {
            await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}