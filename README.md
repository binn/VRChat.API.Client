# VRChat Fluent API

This is a fluent library wrapping the [VRChat.API](https://github.com/vrchatapi/vrchatapi-csharp) library. Designed around Microsoft's Fluent standard, providing a similar experience towards Azure SDKs.

I am writing basic documentation right now, but will update this in the future (especially if community help provided).

API reference is available at https://vrc-fluent-docs.netlify.app/

This library provides Fluent APIs around the VRChat.API clients, specifically exposing an `IVRChat` interface, a `VRChatClientBuilder`, and a `VRCGuid` near-primitive for verifying VRChat Guids; as well as implementations for .NET services with DI.

**View samples in the `VRChat.API.UnitSample` and `VRChat.API.UnitSample.AspNetCore` folders**

## Usage

```csharp
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

```csharp
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

# Contribution

Please help contribute to the API reference and documentation for this library! There is a lot of undocumented behavior.