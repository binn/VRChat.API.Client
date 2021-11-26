<center>
    <img src="vrc_cat.png" width="150" /> <br />
    <a href="https://www.nuget.org/packages/VRChat.API.Client">
        <img src="https://img.shields.io/nuget/v/VRChat.API.Client.svg?style=flat-square" />
    </a>
    <a href="https://www.nuget.org/packages/VRChat.API.Extensions.Hosting">
        <img src="https://img.shields.io/nuget/v/VRChat.API.Extensions.Hosting.svg?style=flat-square" />
    </a>
</center>

# VRChat Fluent API

This is a fluent library wrapping the [VRChat.API](https://github.com/vrchatapi/vrchatapi-csharp) library. Designed around Microsoft's Fluent standard, providing a similar experience towards Azure SDKs.

I am writing basic documentation right now, but will update this in the future (especially if community help provided).

API reference is available at https://vrc-fluent-docs.netlify.app/

This library provides Fluent APIs around the VRChat.API clients, specifically exposing an `IVRChat` interface, a `VRChatClientBuilder`, and a `VRCGuid` near-primitive for verifying VRChat Guids; as well as implementations for .NET services with DI.

**View samples in the `VRChat.API.UnitSample` and `VRChat.API.UnitSample.AspNetCore` folders**

## NuGet Packages

**Fluent API for .NET**, base library

- VRChat.API.Client [![NuGet version (VRChat.API.Client)](https://img.shields.io/nuget/v/VRChat.API.Client.svg?style=flat-square)](https://www.nuget.org/packages/VRChat.API.Client/)

**Dependency Injection for ASP.NET / .NET Services**

- VRChat.API.Extensions.Hosting [![NuGet version (VRChat.API.Extensions.Hosting)](https://img.shields.io/nuget/v/VRChat.API.Extensions.Hosting.svg?style=flat-square)](https://www.nuget.org/packages/VRChat.API.Extensions.Hosting/)


## Usage

Make sure to have the [VRChat.API.Client](https://www.nuget.org/packages/VRChat.API.Client/) package installed from NuGet.

```csharp
using VRChat.API.Client;

IVRChat vrchat = new VRChatClientBuilder() // You can store this builder and use it to re-create new clients whenever
    .WithUsername("username") // .WithCredentials(username, password) can also work
    .WithPassword("password")
    .Build();

if(!vrchat.TryLoginAsync()) // This method calls GetCurrentUserAsync and checks to see if login was successful.
    Console.WriteLine("There was a problem logging in!");

var user = await vrchat.Authentication.GetCurrentUserAsync(); // This method will log you in by default
Console.WriteLine("Logged in as {0}", user.Username);

// The property IVRChat.IsLoggedIn can be used to check, only when using the LoginAsync and TryLoginAsync methods to login.
```

```csharp
using VRChat; // VRCGuid is located in the VRChat namespace

// VRCGuid usage
string input = Console.ReadLine(); // usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469

if(!VRCGuid.TryParse(input, out VRCGuid id))
{
    Console.WriteLine("That ID wasn't a valid VRC ID!");
    return;
}

if(id.Kind != VRCKind.Avatar)
    Console.WriteLine("That isn't an avatar ID! You gave me a '{0}' ID", id.Kind.AsVRChatDescriptor());

// ToString doesn't work on the .Kind enum, you'll need to call .AsVRChatDescriptor() to get the VRChat API-compatible formatted string
// This behaviour may change in the future (looking for a workaround to implement strings)

Console.WriteLine(id.Guid.ToString());
Console.WriteLine(id.ToString());
// The VRCGuid type can be used for strong and fast ID validation when building fluent web services or user-input related services.
```

# Microsoft DI / ASP.NET Core

This library provides Fluent APIs for DI in your .NET services, here's some example usage to get you started.

A detailed sample application is available in the [VRChat.API.UnitSample.AspNetCore](VRChat.API.UnitSample.AspNetCore) folder.

Make sure to have the [VRChat.API.Extensions.Hosting](https://www.nuget.org/packages/VRChat.API.Extensions.Hosting/) package installed from NuGet.

```csharp
using VRChat.API.Extensions.Hosting;

public void ConfigureService(IServiceCollection services)
{
    services.AddVRChat(); // by default, it uses the VRCHAT_USERNAME and VRCHAT_PASSWORD environment variable
    services.AddVRChat("Other"); // Named clients are also available, to be consumed via an IVRChatClientFactory similar to IHttpClientFactory
    services.AddVRChat(Configuration.GetSection("VRChat")); // Configure with IConfiguration, or..
    services.AddVRChat(builder => builder.WithCredentials("username", "password"))); // Use the fluent builder to do whatever you'd like.
    services.AddVRChat("WorldsClient", builder => builder.WithCredentials("username", "password"))); // Use the fluent builder to do whatever you'd like.
}

public void Configure(IVRChatClientFactory factory)
{
    // ... setup app middleware

    factory.LoginAllClients().Wait(); // Login all the clients to give them valid authcookies (unless you explicitly gave a custom Configuration or auth cookie)
}
```

```csharp
using VRChat.API.Extensions.Hosting;

// Consume like so
public class UsersController : ControllerBase
{
   [Route("/{id}")] 
   public async Task<IActionResult> GetUserAsync(string id, [FromServices] IVRChat vrchat)
   {
       var user = await vrchat.Users.GetUserAsync(id);
       return Ok(new { user.Username });
   }
}
```

### Working with Named Clients

```csharp
using VRChat.API.Extensions.Hosting;

// When using named clients, you'll need to use IVRChatClientFactory
// This is always available regardless if you use named clients or not
// Calling CreateClient without any arguments gives you the default IVRChat
// & the implementation is extremely similar to IHttpClientFactory
public class UsersController : ControllerBase
{
    private readonly IVRChat _vrchat;

    public UsersController(IVRChatClientFactory factory) =>
        _vrchat = factory.CreateClient("WorldsClient");

    [Route("/worlds/{id}")] 
    public async Task<IActionResult> GetWorldAsync(string id)
    {
       var world = await vrchat.Worlds.GetWorldAsync(id);
       return Ok(new { world.Name }); // {"name": "world_name"}
    }
}
```

# How is this similar to the Azure SDK?

In the Azure SDK, once an `IAzure` is configured, you can access Azure resources via the properties. 

For example, to list Container Groups: `_azure.ContainerGroups.ListByResourceGroupAsync("eus-rg")`

In this library, similar fluent properties exist, e.g. `_vrchat.Users.GetUserAsync("userId")`

Resources are referred to with a strong property type, `ContainerGroups` vs `Users` or, `NetworkInterfaces` vs `Moderations`

# Where is the API reference for talking with VRChat?

That library is a fluent wrapper around the [VRChat.API library](https://github.com/vrchatapi/vrchatapi-csharp), which is a C# library maintained by [@vrchatapi](https://github.com/vrchatapi)

Refer to their repository for the API reference.

# Contribution

Please help contribute to the API reference and documentation for this library! There is a lot of undocumented behavior.
