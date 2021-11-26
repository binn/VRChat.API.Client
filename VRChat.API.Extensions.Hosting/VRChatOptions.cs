namespace VRChat.API.Extensions.Hosting
{
    /// <summary>
    /// IConfiguration options binding class, can be consumed in publically shipped-APIs.
    /// </summary>
    public class VRChatOptions
    {
        /// <summary>
        /// The username of the VRChat account to login with
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password of the VRChat account to login with
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// HTTP Request pool timeout, in miliseconds
        /// </summary>
        public int? Timeout { get; set; }

        /// <summary>
        /// Proxy URL, if any. Bypass will be enabled by default.
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// The User-Agent header to attach with the client.
        /// </summary>
        public string UserAgent { get; set; }
    }
}