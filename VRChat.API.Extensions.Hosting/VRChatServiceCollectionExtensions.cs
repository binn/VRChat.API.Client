using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using VRChat.API.Client;

namespace VRChat.API.Extensions.Hosting // Maybe we should change the namespace to Microsoft.Extensions.DependencyInjection ?
{
    public static class VRChatServiceCollectionExtensions
    {
        // We don't specify the <returns> or <param> for IServiceCollection because it is inferred that the user knows what they're doing
        
        /// <summary>
        /// Registers an <see cref="IVRChatClient"/> to the service collection as the default <see cref="IVRChatClient"/>.
        /// </summary>
        /// <param name="builder">A configuration action to configure the <see cref="IVRChatClient"/> with.</param>
        public static IServiceCollection AddVRChatClient(this IServiceCollection services, Action<VRChatClientBuilder> builder = null) =>
            AddVRChatClient(services, "vrc_default", builder); // Implement a factory pattern in the future

        /// <summary>
        /// Registers a named <see cref="IVRChatClient"/> to the service collection.
        /// </summary>
        /// <param name="clientName">The name used to refer to this <see cref="IVRChatClient"/> with.</param>
        /// <param name="builder">A configuration action to configure the <see cref="IVRChatClient"/> with.</param>
        public static IServiceCollection AddVRChatClient(this IServiceCollection services, string clientName, Action<VRChatClientBuilder> builder = null)
        {
            var vcb = new VRChatClientBuilder();

            if(services == null)
                throw new ArgumentNullException(nameof(services));

            if(clientName == null)
                throw new ArgumentNullException(nameof(clientName));

            if (builder != null)
                builder(vcb);
            else
                TryFillVRChatFromEnv(ref vcb); // If they don't specify a config action, we'll just use env vars to try and set it up

            services.TryAddSingleton<DefaultVRChatClientFactory>();
            services.TryAddSingleton<IVRChatClientFactory>(srv =>
            {
                IVRChatClient client = vcb.Build();
                var factory = srv.GetRequiredService<DefaultVRChatClientFactory>();

                factory.TryAddClient(clientName, vcb, overrideIfExists: clientName == "vrc_default");

                if (factory.IsDefaultRegistered)
                    factory.TryAddClient("vrc_default", vcb); // We'll register the current one as the default just in case none exist already

                return factory;
            });

            if(clientName == "vrc_default")
                services.TryAddSingleton<IVRChatClient>(srv => vcb.Build()); 

            return services;
        }

        private static void TryFillVRChatFromEnv(ref VRChatClientBuilder vcb) // This method may even be useless
        {
            string username = Environment.GetEnvironmentVariable("VRCHAT_USERNAME");
            string password = Environment.GetEnvironmentVariable("VRCHAT_PASSWORD");
            string timeout = Environment.GetEnvironmentVariable("VRCHAT_TIMEOUT"); // add support for IConfiguration maybe? and proxies too?

            if (username != null && password != null)
            {
                vcb.WithCredentials(username, password);
                if (timeout != null && int.TryParse(timeout, out int millis))
                {
                    vcb.WithTimeout(TimeSpan.FromMilliseconds(millis));
                }
            }
        }
    }
}