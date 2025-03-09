using System;
using System.Diagnostics;
using Unmanaged;

namespace Models.Components
{
    public struct IsModelRequest
    {
        public readonly ulong extension;
        public ASCIIText256 address;
        public TimeSpan timeout;
        public TimeSpan duration;
        public Status status;

        public readonly ASCIIText256 Extension
        {
            get
            {
                Span<char> chars = stackalloc char[8];
                for (int i = 0; i < chars.Length; i++)
                {
                    char c = (char)((extension >> (i * 8)) & 0xFF);
                    if (c == default)
                    {
                        return new ASCIIText256(chars.Slice(0, i));
                    }

                    chars[i] = c;
                }

                return new ASCIIText256(chars);
            }
        }

        [Obsolete("Default constructor not supported", true)]
        public IsModelRequest()
        {
            throw new NotSupportedException();
        }

        public IsModelRequest(ReadOnlySpan<char> extension, ASCIIText256 address, TimeSpan timeout)
        {
            ThrowIfExtensionIsTooLong(extension);

            this.extension = default;
            for (int i = 0; i < extension.Length; i++)
            {
                this.extension |= (ulong)extension[i] << (i * 8);
            }

            this.address = address;
            this.timeout = timeout;
            duration = TimeSpan.Zero;
            status = Status.Submitted;
        }

        public IsModelRequest(ASCIIText256 extension, ASCIIText256 address, TimeSpan timeout)
        {
            ThrowIfExtensionIsTooLong(extension.ToString().AsSpan());

            this.extension = default;
            for (int i = 0; i < extension.Length; i++)
            {
                this.extension |= (ulong)extension[i] << (i * 8);
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

        public readonly int CopyExtensionBytes(Span<byte> destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                byte b = (byte)((extension >> (i * 8)) & 0xFF);
                if (b == default)
                {
                    return i;
                }

                destination[i] = b;
            }

            return default;
        }

        public readonly int CopyExtensionCharacters(Span<char> destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                char c = (char)((extension >> (i * 8)) & 0xFF);
                if (c == default)
                {
                    return i;
                }

                destination[i] = c;
            }

            return default;
        }

        [Conditional("DEBUG")]
        private static void ThrowIfExtensionIsTooLong(ReadOnlySpan<char> extension)
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
