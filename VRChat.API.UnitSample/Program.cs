using VRChat.API.Client;

namespace VRChat.API.UnitSample
{
    // While it would be nice to use the new .NET 6 template, it seems reasonable enough
    // to use the classic Program.Main here, for ease of understand (for the end-user).
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string username = Environment.GetEnvironmentVariable("VRCHAT_USERNAME");
            string password = Environment.GetEnvironmentVariable("VRCHAT_PASSWORD");

            var client = new VRChatClientBuilder()
                .WithUsername(username)
                .WithPassword(password)
                .Build();

            var user = await client.Authentication.GetCurrentUserAsync();
            Console.WriteLine("Logged in as: {0} ({1})", user.Username, user.Id);
        }
    }
}