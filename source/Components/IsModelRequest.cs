using System;
using Unmanaged;

namespace Models.Components
{
    public struct IsModelRequest
    {
        public readonly ASCIIText8 extension;
        public ASCIIText256 address;
        public double timeout;
        public double duration;
        public Status status;

        [Obsolete("Default constructor not supported", true)]
        public IsModelRequest()
        {
            throw new NotSupportedException();
        }

        public IsModelRequest(ReadOnlySpan<char> extension, ASCIIText256 address, double timeout)
        {
            this.extension = new(extension);
            this.address = address;
            this.timeout = timeout;
            duration = 0;
            status = Status.Submitted;
        }

        public IsModelRequest(ASCIIText8 extension, ASCIIText256 address, double timeout)
        {
            this.extension = extension;
            this.address = address;
            this.timeout = timeout;
            duration = 0;
            status = Status.Submitted;
        }

        public IsModelRequest(string extension, string address, double timeout)
        {
            this.extension = extension;
            this.address = address;
            this.timeout = timeout;
            duration = 0;
            status = Status.Submitted;
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
