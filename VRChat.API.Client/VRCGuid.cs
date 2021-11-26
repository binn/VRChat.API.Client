﻿using System;

namespace VRChat
{
    /// <summary>
    /// The <see cref="VRCGuid"/> struct represents a <see cref="Guid"/> tied in with an underlying <see cref="VRCGuidType"/> <br />
    /// which may represent a <c>file</c>, <c>avatar</c>, <c>world</c>, or <c>user</c>.
    /// <br />This is a helper class used to validate and .NET-ify VRChat's IDs.
    /// </summary>
    public struct VRCGuid
    {
        private readonly VRCGuidType _type;
        private readonly Guid _guid;

        private VRCGuid(VRCGuidType type, Guid id)
        {
            this._type = type;
            this._guid = id;
        }

        /// <summary>
        /// The underlying <see cref="Guid"/> that this <see cref="VRCGuid"/> represents.
        /// </summary>
        public Guid Guid => _guid;

        /// <summary>
        /// The underlying <see cref="VRCGuidType"/> that this <see cref="VRCGuid"/> represents.
        /// </summary>
        public VRCGuidType Type => _type;

        /// <summary>
        /// A default <see cref="VRCGuid"/> that is empty with "file" as it's type. Keep in mind VRCGuid's can not be null.
        /// </summary>
        public static VRCGuid Empty => VRCGuid.Create(VRCGuidType.File, Guid.Empty);

        /// <summary>
        /// Formats the current <see cref="VRCGuid"/> as a string with the <c>[type]_[guid]</c> format standard for VRChat.
        /// </summary>
        /// <returns>A string representing the current <see cref="VRCGuid"/>, formatted with <c>[type]_[guid]</c></returns>
        public override string ToString()
        {
            return string.Concat( // Maybe there is a faster implementation?
                    _type.AsVRChatDescriptor(),
                    "_",
                    _guid.ToString()
                );
        }

        /// <summary>
        /// Creates a <see cref="VRCGuid"/> with the specified parameters
        /// </summary>
        /// <param name="type">The <see cref="VRCGuidType"/> type of <see cref="VRCGuid"/> to create.</param>
        /// <param name="underlyingId">The <see cref="Guid"/> of the <see cref="VRCGuid"/> to create.</param>
        /// <returns>A new <see cref="VRCGuid"/>.</returns>
        public static VRCGuid Create(VRCGuidType type, Guid underlyingId) =>
            new VRCGuid(type, underlyingId);

        /// <summary>
        /// Creates a new <see cref="VRCGuid"/> randomly, with the specified <see cref="VRCGuidType"/>, otherwise <c>VRCGuidType.File</c>
        /// </summary>
        /// <param name="type">The <see cref="VRCGuidType"/> type of <see cref="VRCGuid"/> to create.</param>
        /// <returns>A new <see cref="VRCGuid"/>.</returns>
        public static VRCGuid NewGuid(VRCGuidType type = VRCGuidType.File) =>
            VRCGuid.Create(type, Guid.NewGuid());

        /// <summary>
        /// Tries to parse a <see cref="VRCGuid"/> from the input string provided, and returns a <see cref="bool"/> representing the status of the operation.
        /// </summary>
        /// <param name="incoming">The input string to parse, e.g. <b><c>usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469</c></b></param>
        /// <param name="vrcid">The result <see cref="VRCGuid"/> of the operation to be returned.</param>
        /// <returns>A <see cref="bool"/> value representing the success of the operation</returns>
        public static bool TryParse(string incoming, out VRCGuid vrcid)
        {
            vrcid = VRCGuid.Empty;
            string[] parts = incoming.Split('_');

            if (parts.Length != 2)
                return false;

            if (!VRCGuidTypeEnum.TryParse(parts[0], out VRCGuidType type)) // There could be a better implementation of this in the future
                return false;

            if (!Guid.TryParse(parts[1], out Guid id))
                return false;

            vrcid = VRCGuid.Create(type, id);
            return true;
        }

        /// <summary>
        /// Parses a <see cref="VRCGuid"/> from the input string provided, otherwise throws a <see cref="FormatException"/> if unable to parse it.
        /// </summary>
        /// <param name="id">The input string to parse, e.g. <b><c>usr_c1644b5b-3ca4-45b4-97c6-a2a0de70d469</c></b></param>
        /// <returns>A <see cref="VRCGuid"/> value representing the input string</returns>
        public static VRCGuid Parse(string id)
        {
            if (!VRCGuid.TryParse(id, out VRCGuid vrcid))
                throw new FormatException("Input string was not in the correct format for a " + nameof(VRCGuid) + ".");

            return vrcid;
        }
    }
}
