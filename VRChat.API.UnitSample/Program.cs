using VRChat.API.Client;

namespace VRChat.API.UnitSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var client = new VRChatClientBuilder()
                .WithUsername("dot bin")
                .WithPassword("[redacted]")
                .WithTimeout(TimeSpan.FromSeconds(1))
                .Build();

            var id = VRCGuid.Parse("usr_39033345-2273-4929-95ab-a4a53105980a");
            Console.WriteLine(id.ToString());
            Console.WriteLine(id.Type.AsVRChatDescriptor());
            Console.WriteLine(id.Guid.ToString());

            var user = await client.Authentication.GetCurrentUserAsync();
            Console.WriteLine("Logged in as: {0} ({1})", user.Username, user.Id);
        }
    }
}