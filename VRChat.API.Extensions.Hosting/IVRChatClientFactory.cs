using System.Threading.Tasks;
using VRChat.API.Client;

namespace VRChat.API.Extensions.Hosting
{
    public interface IVRChatClientFactory
    {
        IVRChat CreateClient();
        IVRChat CreateClient(string name);

        Task AttemptLoginForAllClients();
        Task LoginClientAsync(string name = "vrc_default");
    }
}
