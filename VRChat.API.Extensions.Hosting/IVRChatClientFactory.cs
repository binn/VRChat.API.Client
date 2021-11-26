using VRChat.API.Client;

namespace VRChat.API.Extensions.Hosting
{
    public interface IVRChatClientFactory
    {
        IVRChat CreateClient();
        IVRChat CreateClient(string name);
    }
}
