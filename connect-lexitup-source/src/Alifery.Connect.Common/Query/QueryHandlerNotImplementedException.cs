using System;

namespace Alifery.Connect.Common.Query
{
    public class QueryHandlerNotImplementedException : Exception
    {
        public QueryHandlerNotImplementedException(Type type) : base("Could not find handler for type " + type)
        {
        }
    }
}