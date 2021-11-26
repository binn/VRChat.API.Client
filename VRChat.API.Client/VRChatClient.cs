using System.Threading;
using System.Threading.Tasks;
using VRChat.API.Api;
using VRChat.API.Model;

namespace VRChat.API.Client
{
    public interface IVRChatClient
    {
        /// <summary>
        /// <b>The Files API for VRChat.</b> <br />
        /// The Files API provides access to creating, deleting, uploading, and managing files for avatars, worlds, thumbnails, and user profiles on VRChat.
        /// </summary>
        FilesApi Files { get; }

        /// <summary>
        /// <b>The Users API for VRChat.</b> <br />
        /// The Users API provides access to searching for, querying, and updating user information on VRChat.
        /// </summary>
        UsersApi Users { get; }

        /// <summary>
        /// <b>The System API for VRChat.</b> <br />
        /// The System API for VRChat provides access to fetching configurations, system informations, and other general system tasks. <br />
        /// <em>Operations include:</em>
        /// <list type="bullet">
        ///     <item>
        ///         <term>Fetch API configuration</term>
        ///     </item>
        ///     <item>
        ///         <term>Download CSS or JavaScript files</term>
        ///     </item>
        ///     <item>
        ///         <term>Get remote system time</term>
        ///     </item>
        ///     <item>
        ///         <term>Get currently online users</term>
        ///     </item>
        /// </list>
        /// </summary>
        SystemApi System { get; }

        /// <summary>
        /// <b>The Worlds API for VRChat.</b> <br />
        /// The Worlds API provides access to querying, creating, updating, and general world resource management on VRChat.
        /// </summary>
        WorldsApi Worlds { get; }

        /// <summary>
        /// <b>The Invites API for VRChat.</b> <br />
        /// The Invites API provides access to requesting invites, listing invites, sending invites, and managing invites on VRChat.
        /// </summary>
        InviteApi Invites { get; }

        /// <summary>
        /// <b>The Avatars API for VRChat.</b> <br />
        /// The Avatars API provides access to querying, creating, updating, and general avatar resource management on VRChat.
        /// </summary>
        AvatarsApi Avatars { get; }

        /// <summary>
        /// <b>The Economy API for VRChat.</b> <br />
        /// The Economy API provides access to information on the current user's transactions toward VRChat Inc.
        /// </summary>
        EconomyApi Economy { get; }

        /// <summary>
        /// <b>The Friends API for VRChat.</b> <br />
        /// The Friends API provides access to managing relationships with other users on VRChat.
        /// </summary>
        FriendsApi Friends { get; }

        /// <summary>
        /// <b>The Instances API for VRChat.</b> <br />
        /// The Instances API provides access to querying instance information and requesting instance access on VRChat.
        /// </summary>
        InstancesApi Instances { get; }

        /// <summary>
        /// <b>The Favorites API for VRChat.</b> <br />
        /// The Favorites API provides access to managing avatar and world favorites, and favorite groups with VRChat.
        /// </summary>
        FavoritesApi Favorites { get; }

        /// <summary>
        /// <b>The Permissions API for VRChat.</b> <br />
        /// The Permissions API provides access to querying permissions & assigned permissions on VRChat
        /// <br /> This API is used for managing VRChat+ related permissions (inferred).
        /// </summary>
        PermissionsApi Permissions { get; }

        /// <summary>
        /// <b>The Notifications API for VRChat.</b> <br />
        /// The Notifications API provides access to listing, deleting, acknowledging, and clearning notifications on VRChat.
        /// <br /> <em>An important use-case of this API is to accept a friend request notification.</em>
        /// </summary>
        NotificationsApi Notifications { get; }

        /// <summary>
        /// <b>The Moderations API for VRChat.</b> <br />
        /// The Moderations API provides access to blocking/unblocking users, muting users, and managing general moderations on VRChat.
        /// </summary>
        PlayermoderationApi Moderations { get; }

        /// <summary>
        /// <b>The Authentication API for VRChat.</b> <br />
        /// The Authentication API provides access to basic authentication (login/logout), 2FA access, user deletion, and checking for taken usernames/emails.
        /// </summary>
        AuthenticationApi Authentication { get; }

        /// <summary>
        /// Will be <c>true</c> if the current client is successfully logged in as a user.
        /// <br /> Otherwise, this will return <c>false</c>.
        /// <br /> <em>This property automatically updates when calling <see cref="IVRChatClient.TryLoginAsync(CancellationToken)"/> or <see cref="IVRChatClient.LoginAsync(bool, CancellationToken)"/></em>
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Tries to login as the currently configured user, and returns a <see cref="bool"/> with the result.
        /// </summary>
        /// <param name="ct">Cancellation token for cancelling any asynchronous operations</param>
        /// <returns>A <see cref="bool"/> specifying if the current client is logged in successfully.</returns>
        Task<bool> TryLoginAsync(CancellationToken ct = default);

        /// <summary>
        /// Logs in as the currently configured user. Will throw an exception unless <b><c>throwOnFail</c></b> is set to <b><c>true</c></b>.
        /// <br /> If successful, the <see cref="CurrentUser"/> will be returned, otherwise <b><c>null</c></b> (if <b><c>throwOnFail</c></b> is false).
        /// </summary>
        /// <param name="throwOnFail">If you are simply checking for valid authentication, leave this as false</param>
        /// <param name="ct">Cancellation token for cancelling any asynchronous operations</param>
        /// <returns>A <see cref="CurrentUser"/> with the currently logged in user, if successful.</returns>
        Task<CurrentUser> LoginAsync(bool throwOnFail = false, CancellationToken ct = default);
    }

    public class VRChatClient : IVRChatClient
    {
        private VRChatClient(Configuration configuration)
        {
            _files = new FilesApi(configuration);
            _users = new UsersApi(configuration);
            _system = new SystemApi(configuration);
            _worlds = new WorldsApi(configuration);
            _invites = new InviteApi(configuration);
            _avatars = new AvatarsApi(configuration);
            _economy = new EconomyApi(configuration);
            _friends = new FriendsApi(configuration);
            _instances = new InstancesApi(configuration);
            _favorites = new FavoritesApi(configuration);
            _permissions = new PermissionsApi(configuration);
            _notifications = new NotificationsApi(configuration);
            _moderations = new PlayermoderationApi(configuration);
            _authentication = new AuthenticationApi(configuration);
        }

        private readonly FilesApi _files;
        private readonly UsersApi _users;
        private readonly SystemApi _system;
        private readonly WorldsApi _worlds;
        private readonly InviteApi _invites;
        private readonly AvatarsApi _avatars;
        private readonly EconomyApi _economy;
        private readonly FriendsApi _friends;
        private readonly InstancesApi _instances;
        private readonly FavoritesApi _favorites;
        private readonly PermissionsApi _permissions;
        private readonly NotificationsApi _notifications;
        private readonly PlayermoderationApi _moderations;
        private readonly AuthenticationApi _authentication;

        public FilesApi Files => _files;
        public UsersApi Users => _users;
        public SystemApi System => _system;
        public WorldsApi Worlds => _worlds;
        public InviteApi Invites => _invites;
        public AvatarsApi Avatars => _avatars;
        public EconomyApi Economy => _economy;
        public FriendsApi Friends => _friends;
        public InstancesApi Instances => _instances;
        public FavoritesApi Favorites => _favorites;
        public PermissionsApi Permissions => _permissions;
        public NotificationsApi Notifications => _notifications;
        public PlayermoderationApi Moderations => _moderations;
        public AuthenticationApi Authentication => _authentication;

        public bool IsLoggedIn { get; private set; }

        // Creates a new VRChatClient internally, compatible with IVRChatClient
        internal static VRChatClient CreateInternal(Configuration configuration) =>
            new VRChatClient(configuration);

        public async Task<bool> TryLoginAsync(CancellationToken ct = default)
        {
            if (this.IsLoggedIn)
                return true;

            return (await this.LoginAsync(throwOnFail: false, ct)) != null;
        }

        public async Task<CurrentUser> LoginAsync(bool throwOnFail = false, CancellationToken ct = default)
        {
            CurrentUser user = null;

            try
            {
                user = await this.Authentication.GetCurrentUserAsync(ct);
            }
            catch
            {
                if (throwOnFail)
                    throw;
            }

            this.IsLoggedIn = user != null;
            return user;
        }
    }
}