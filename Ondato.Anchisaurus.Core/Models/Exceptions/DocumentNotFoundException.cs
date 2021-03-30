using System;

namespace Ondato.Anchisaurus.Core.Models.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string message)
            : base(message)
        {
        }

        public DocumentNotFoundException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }
}