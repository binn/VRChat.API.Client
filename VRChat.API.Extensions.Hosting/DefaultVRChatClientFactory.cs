using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VRChat.API.Client;

namespace VRChat.API.Extensions.Hosting
{
    internal class DefaultVRChatClientFactory : IVRChatClientFactory
    {
        private readonly IDictionary<string, VRChatClientBuilder> _builders;

        public DefaultVRChatClientFactory() =>
            _builders = new Dictionary<string, VRChatClientBuilder>();

        internal bool IsDefaultRegistered => _builders.ContainsKey("vrc_default"); // Not sure where I was going with this, but I'll keep it in here for now

        public IVRChat CreateClient() // Maybe we should throw an exception if the default was not registered? 
            // It'll ensure that users of the library don't end up accidentally using a client that isn't registered (inconsistent library design)
        {
            if (!_builders.TryGetValue("vrc_default", out VRChatClientBuilder vcb))
            {
                if (_builders.Count > 0)
                    vcb = _builders.First().Value;
                else
                    vcb = new VRChatClientBuilder(); // Because CreateClient can never return a null client even if the default doesn't exist.

                Trace.WriteLine("warn: the default for IVRChatClient was not registered in the factory, so the first available/empty one was created instead.");
            }

            return vcb.Build();
        }

        public IVRChat CreateClient(string name)
        {
            if (_builders.ContainsKey(name))
                return _builders[name].Build();
            else
                throw new NullReferenceException("The specified client does not exist!");
        }

        internal bool TryAddClient(string clientName, VRChatClientBuilder vcb, bool overrideIfExists = false)
        {
            if (_builders.ContainsKey(clientName) && !overrideIfExists) // So that the default may be registered in the case of it being nonexistant
                return false;
            else
                _builders.Add(clientName, vcb);

            return true;
        }
    }
}
