namespace VRChat
{
    public enum VRCGuidType
    {
        File,
        User,
        World,
        Avatar
    }

    public static class VRCGuidTypeEnum
    {
        /// <summary>
        /// Formats the <see cref="VRCGuidType"/> to it's respective VRChat descriptor.
        /// </summary>
        /// <param name="type">The <see cref="VRCGuidType"/> to format</param>
        /// <returns>The string representation of the incoming type, otherwise "file".</returns>
        public static string AsVRChatDescriptor(this VRCGuidType type)
        {
            switch (type)
            {
                case VRCGuidType.Avatar:
                    return "avtr";
                case VRCGuidType.World:
                    return "wrld";
                case VRCGuidType.User:
                    return "usr";
                case VRCGuidType.File:
                default:
                    return "file";
            }
        }
        
        // Maybe make this public?
        internal static bool TryParse(string input, out VRCGuidType type)
        {
            VRCGuidType? attempt = null;
            switch (input)
            {
                case "avtr":
                    attempt = VRCGuidType.Avatar;
                    break;
                case "wrld":
                    attempt = VRCGuidType.World;
                    break;
                case "usr":
                    attempt = VRCGuidType.User;
                    break;
                case "file":
                    attempt = VRCGuidType.File;
                    break;
            }

            type = attempt.Value;
            return attempt.HasValue; // Not sure if this is the best implementation
        }
    }
}