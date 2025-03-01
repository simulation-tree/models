using System;
using System.Diagnostics;
using Unmanaged;

namespace Models.Components
{
    public struct IsModelRequest
    {
        public readonly ulong extension;
        public FixedString address;
        public TimeSpan timeout;
        public TimeSpan duration;
        public Status status;

        public readonly FixedString Extension
        {
            get
            {
                USpan<char> chars = stackalloc char[8];
                for (int i = 0; i < chars.Length; i++)
                {
                    char c = (char)((extension >> (i * 8)) & 0xFF);
                    if (c == default)
                    {
                        return new FixedString(chars.GetSpan((uint)i));
                    }

                    chars[(uint)i] = c;
                }

                return new FixedString(chars);
            }
        }

        [Obsolete("Default constructor not supported", true)]
        public IsModelRequest()
        {
            throw new NotSupportedException();
        }

        public IsModelRequest(USpan<char> extension, FixedString address, TimeSpan timeout)
        {
            ThrowIfExtensionIsTooLong(extension);

            this.extension = default;
            for (int i = 0; i < extension.Length; i++)
            {
                this.extension |= (ulong)extension[(uint)i] << (i * 8);
            }

            this.address = address;
            this.timeout = timeout;
            duration = TimeSpan.Zero;
            status = Status.Submitted;
        }

        public IsModelRequest(FixedString extension, FixedString address, TimeSpan timeout)
        {
            ThrowIfExtensionIsTooLong(extension.ToString().AsSpan());

            this.extension = default;
            for (int i = 0; i < extension.Length; i++)
            {
                this.extension |= (ulong)extension[(uint)i] << (i * 8);
            }

            this.address = address;
            this.timeout = timeout;
            duration = TimeSpan.Zero;
            status = Status.Submitted;
        }

        public IsModelRequest(string extension, string address, TimeSpan timeout) : this(extension.AsSpan(), address, timeout)
        {
        }

        public readonly IsModelRequest BecomeLoaded()
        {
            IsModelRequest request = this;
            request.status = Status.Loaded;
            return request;
        }

        public readonly uint CopyExtensionBytes(USpan<byte> destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                byte b = (byte)((extension >> (i * 8)) & 0xFF);
                if (b == default)
                {
                    return (uint)i;
                }

                destination[(uint)i] = b;
            }

            return default;
        }

        public readonly uint CopyExtensionCharacters(USpan<char> destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                char c = (char)((extension >> (i * 8)) & 0xFF);
                if (c == default)
                {
                    return (uint)i;
                }

                destination[(uint)i] = c;
            }

            return default;
        }

        [Conditional("DEBUG")]
        private static void ThrowIfExtensionIsTooLong(USpan<char> extension)
        {
            if (extension.Length > sizeof(ulong))
            {
                throw new ArgumentException("Extension is too long", nameof(extension));
            }
        }

        public enum Status : byte
        {
            Submitted,
            Loading,
            Loaded,
            NotFound
        }
    }
}
