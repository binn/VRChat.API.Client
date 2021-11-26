using System;
using System.Net;

namespace VRChat.API.Client
{
    public class VRChatClientBuilder
    {
        private const string _defaultUserAgent = "VRChat.API.Client/1.0 (.NET) netstandard2.0 (https://dot.net) VRChat.API/RestSharp";
        private const string _defaultApiKey = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26";
        private readonly Configuration _configuration;

        /// <summary>
        /// Initializes a blank <see cref="VRChatClientBuilder"/> <br />
        /// </summary>
        public VRChatClientBuilder() : this(null) { }

        /// <summary>
        /// Initializes a <see cref="VRChatClientBuilder"/> from a <see cref="global::VRChat.API.Client.Configuration"/> (if any)
        /// <br /> <b style="color: red">Note: <em>This should not be used unless you know what you're doing</em></b>
        /// </summary>
        /// <param name="incomingConfiguration">The <see cref="Configuration"/> to initialize with as a base</param>
        public VRChatClientBuilder(Configuration incomingConfiguration)
        {
            _configuration = incomingConfiguration ?? new Configuration();
            if(_configuration.UserAgent == null)
                this.WithUserAgent(_defaultUserAgent);
        }

        public void WithCredentials(object username, object password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a <see cref="VRChatClientBuilder"/> from a <see cref="Configuration"/>
        /// <br /> <b style="color: red">Note: <em>This should not be used unless you know what you're doing</em></b>
        /// </summary>
        /// <param name="incomingConfiguration">The <see cref="Configuration"/> to initialize with as a base</param>
        public static VRChatClientBuilder From(Configuration incomingConfiguration) =>
            new VRChatClientBuilder(incomingConfiguration);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="auth"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithCredentials(string username, string password, string auth, string apiKey) => this
            .WithUsername(username)
            .WithPassword(password)
            .WithAuthCookie(auth)
            .WithApiKey(apiKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithCredentials(string username, string password, string apiKey) =>
            this.WithCredentials(username, password, null, apiKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithCredentials(string username, string password) =>
            this.WithCredentials(username, password, null, null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithUsername(string username)
        {
            _configuration.Username = username;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithPassword(string password)
        {
            _configuration.Password = password;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithApiKey(string apiKey)
        {
            _configuration.AddApiKey("apiKey", apiKey ?? _defaultApiKey);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithAuthCookie(string auth)
        {
            if (auth != null)
                _configuration.AddApiKey("auth", auth);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithUserAgent(string userAgent)
        {
            _configuration.UserAgent = userAgent ?? _defaultUserAgent;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithTimeout(TimeSpan timeout)
        {
            _configuration.Timeout = (int)timeout.TotalMilliseconds; // Using Miliseconds over TotalMilliseconds can cause issues when the timespan is empty
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithProxy(WebProxy proxy)
        {
            _configuration.Proxy = proxy;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bypass"></param>
        /// <returns></returns>
        public VRChatClientBuilder WithProxy(string url, bool bypass = true) =>
            this.WithProxy(new WebProxy(url, bypass));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="useWithoutCredentials"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IVRChat Build(bool useWithoutCredentials = true)
        {
            if(!useWithoutCredentials)
            {
                if(_configuration.Username == null || _configuration.Password == null)
                {
                    if (_configuration.GetApiKeyWithPrefix("auth") == null)
                        throw new ArgumentException("No credentials have been set up, and useWithoutCredentials is false");
                }
            }

            return VRChat.CreateInternal(_configuration);
        }
    }
}
