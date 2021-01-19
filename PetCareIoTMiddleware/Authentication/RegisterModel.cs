namespace PetCareIoTMiddleware.Authentication
{
    /// <summary>
    /// Wrapper class for posting client's registration.
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// Client's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Client's password.
        /// </summary>
        public string Password { get; set; }

        public string Email { get; set; }
    }
}