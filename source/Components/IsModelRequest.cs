using System;
using Unmanaged;

namespace Models.Components
{
    public struct IsModelRequest
    {
        public readonly ASCIIText8 extension;
        public ASCIIText256 address;
        public TimeSpan timeout;
        public TimeSpan duration;
        public Status status;

        [Obsolete("Default constructor not supported", true)]
        public IsModelRequest()
        {
            throw new NotSupportedException();
        }

        public IsModelRequest(ReadOnlySpan<char> extension, ASCIIText256 address, TimeSpan timeout)
        {
            this.extension = new(extension);
            this.address = address;
            this.timeout = timeout;
            duration = TimeSpan.Zero;
            status = Status.Submitted;
        }

        public IsModelRequest(ASCIIText8 extension, ASCIIText256 address, TimeSpan timeout)
        {
            this.extension = extension;
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
            return extension.CopyTo(destination);
        }

        public readonly int CopyExtensionCharacters(Span<char> destination)
        {
            return extension.CopyTo(destination);
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
