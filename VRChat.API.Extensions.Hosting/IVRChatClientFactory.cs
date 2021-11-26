using VRChat.API.Client;

namespace VRChat.API.Extensions.Hosting
{
    public interface IVRChatClientFactory
    {
        IVRChatClient CreateClient();
        IVRChatClient CreateClient(string name);
    }
}
