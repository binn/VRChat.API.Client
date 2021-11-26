namespace VRChat
{
    /// <summary>
    /// A kind formatter for specifying the <see cref="VRCGuid.Kind"/> property
    /// <br /> This can be used to ensure proper IDs are used in business-first applications.
    /// </summary>
    public enum VRCKind
    {
        /// <summary>
        /// A VRChat ID of type "file", representing a File object.
        /// </summary>
        File,

        /// <summary>
        /// A VRChat ID of type "usr", representing a User object.
        /// </summary>
        User,

        /// <summary>
        /// A VRChat ID of type "wrld", representing a World object.
        /// </summary>
        World,

        /// <summary>
        /// A VRChat ID of type "avtr", representing an Avatar object.
        /// </summary>
        Avatar
    }

    /// <summary>
    /// Extensions methods for the <see cref="VRCKind"/> enum.
    /// </summary>
    public static class VRCKindEnum
    {
        /// <summary>
        /// Formats the <see cref="VRCKind"/> to it's respective VRChat descriptor.
        /// </summary>
        /// <param name="type">The <see cref="VRCKind"/> to format</param>
        /// <returns>The string representation of the incoming type, otherwise "file".</returns>
        public static string AsVRChatDescriptor(this VRCKind type)
        {
            switch (type)
            {
                case VRCKind.Avatar:
                    return "avtr";
                case VRCKind.World:
                    return "wrld";
                case VRCKind.User:
                    return "usr";
                case VRCKind.File:
                default:
                    return "file";
            }
        }
        
        // Maybe make this public?
        internal static bool TryParse(string input, out VRCKind type)
        {
            VRCKind? attempt = null;
            switch (input)
            {
                case "avtr":
                    attempt = VRCKind.Avatar;
                    break;
                case "wrld":
                    attempt = VRCKind.World;
                    break;
                case "usr":
                    attempt = VRCKind.User;
                    break;
                case "file":
                    attempt = VRCKind.File;
                    break;
            }

            type = attempt.Value;
            return attempt.HasValue; // Not sure if this is the best implementation
        }
    }
}