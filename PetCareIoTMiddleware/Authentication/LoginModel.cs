namespace PetCareIoTMiddleware.Authentication
{
    /// <summary>
    /// Wrapper class for user's credentials for login request.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Client's credentials.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Client's password.
        /// </summary>
        public string Password { get; set; }
    }
}