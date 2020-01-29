﻿using System;

namespace MongoDb.Atlas.Client.AtlasComponent.Domain.Exceptions
{
    public class ConnectivityException : Exception
    {
        public ConnectivityException()
        {
        }

        public ConnectivityException(string message) : base(message)
        {
        }

        public ConnectivityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
