﻿using Microsoft.Extensions.Configuration;
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
        /// Registers an <see cref="IVRChat"/> to the service collection as the default <see cref="IVRChat"/>, using an <see cref="IConfigurationSection"/> to configure.
        /// </summary>
        /// <param name="section">A configuration section used to configure the <see cref="IVRChat"/> with.</param>
        public static IServiceCollection AddVRChat(this IServiceCollection services, IConfigurationSection section) =>
            AddVRChat(services, "vrc_default", section);

        /// <summary>
        /// Registers a named <see cref="IVRChat"/> to the service collection, using an <see cref="IConfigurationSection"/> to configure.
        /// <br /> <b>Note, you will need to use <see cref="IVRChatClientFactory"/> to consume named <see cref="IVRChat"/> clients.</b>
        /// </summary>
        /// <param name="section">A configuration section used to configure the <see cref="IVRChat"/> with.</param>
        public static IServiceCollection AddVRChat(this IServiceCollection services, string clientName, IConfigurationSection section)
        {
            VRChatOptions options = section.Get<VRChatOptions>(); // Bind the section into VRChatOptions
            return AddVRChat(services, clientName, builder => // build the IVRChat client using the provided options
            {
                builder.WithCredentials(options.Username, options.Password);

                if (options.Proxy != null)
                    builder.WithProxy(options.Proxy, true);

                if (options.Timeout.HasValue)
                    builder.WithTimeout(TimeSpan.FromMilliseconds(options.Timeout.Value));

                if (options.UserAgent != null)
                    builder.WithUserAgent(options.UserAgent);
            });
        }

        /// <summary>
        /// Registers an <see cref="IVRChat"/> to the service collection as the default <see cref="IVRChat"/>.
        /// </summary>
        /// <param name="builder">A configuration action to configure the <see cref="IVRChat"/> with.</param>
        public static IServiceCollection AddVRChat(this IServiceCollection services, Action<VRChatClientBuilder> builder = null) =>
            AddVRChat(services, "vrc_default", builder); // Implement a factory pattern in the future

        /// <summary>
        /// Registers a named <see cref="IVRChat"/> to the service collection.
        /// <br /> <b>Note, you will need to use <see cref="IVRChatClientFactory"/> to consume named <see cref="IVRChat"/> clients.</b>
        /// </summary>
        /// <param name="clientName">The name used to refer to this <see cref="IVRChat"/> with.</param>
        /// <param name="builder">A configuration action to configure the <see cref="IVRChat"/> with.</param>
        public static IServiceCollection AddVRChat(this IServiceCollection services, string clientName, Action<VRChatClientBuilder> builder = null)
        {
            var vcb = new VRChatClientBuilder();

            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (clientName == null)
                throw new ArgumentNullException(nameof(clientName));

            if (builder != null)
                builder(vcb);
            else
                TryFillVRChatFromEnv(ref vcb); // If they don't specify a config action, we'll just use env vars to try and set it up

            services.TryAddSingleton<DefaultVRChatClientFactory>();
            services.TryAddSingleton<IVRChatClientFactory>(srv =>
            {
                IVRChat client = vcb.Build();
                var factory = srv.GetRequiredService<DefaultVRChatClientFactory>();

                factory.TryAddClient(clientName, vcb, overrideIfExists: clientName == "vrc_default");

                if (factory.IsDefaultRegistered)
                    factory.TryAddClient("vrc_default", vcb); // We'll register the current one as the default just in case none exist already
                // by default, overrideIfExists is false, so this function will do nothing if vrc_default exists
                // TODO: move "vrc_default" into some form of constant
                // Maybe we should setup automatically IVRChat if the default vrc_default is not implemented
                // OR, have it throw an error unless vrc_default is explicitly added (remove adding random clients as the default)

                return factory;
            });

            if (clientName == "vrc_default")
                services.TryAddSingleton<IVRChat>(srv => vcb.Build()); // Build the default vrchat client and throw it in as a singleton

            return services;
        }

        private static void TryFillVRChatFromEnv(ref VRChatClientBuilder vcb) // This method may even be useless
        {
            string useragent = Environment.GetEnvironmentVariable("VRCHAT_USER_AGENT");
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

                if (useragent != null)
                    vcb.WithUserAgent(useragent);
            }
        }
    }
}