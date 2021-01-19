namespace PetCareIoTMiddleware.Authentication
{
    /// <summary>
    /// Represents return value for HTTP Post requests.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Response status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Full response message.
        /// </summary>
        public string Message { get; set; }
    }
}