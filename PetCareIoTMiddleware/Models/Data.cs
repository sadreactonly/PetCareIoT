namespace PetCareIoTMiddleware.Models
{
    /// <summary>
    /// Represents actual message that will be routed from one client to another.
    /// It's part of Message.
    /// </summary>
    public class Data
    {
        /// <summary>
        /// Event type.
        /// </summary>
        public EventType Type { get; set; }

        /// <summary>
        /// Global ID of event.
        /// </summary>
        public string RequestId { get; set; }
    }
}