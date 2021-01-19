namespace PetCareIoTMiddleware.Models
{
    /// <summary>
    /// Message from clients.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Message sender
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Message reciever.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Represents actual message that will be routed from one client to another.
        /// </summary>
        public Data Data { get; set; }
    }
}