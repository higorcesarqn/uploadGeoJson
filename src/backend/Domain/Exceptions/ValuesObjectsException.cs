using Core.Models;
using System;

namespace Domain.Exceptions
{
    public class ValuesObjectsException<TValueObject> : Exception
        where TValueObject : ValueObject
    {
        public ValuesObjectsException(TValueObject valueObject, string message) : base(message)
        {
            ValueObject = valueObject;
        }

        public ValuesObjectsException(TValueObject valueObject, string message, Exception innerException) : base(message, innerException)
        {
            ValueObject = valueObject;
        }

        public TValueObject ValueObject { get; private set; }
    }
}
