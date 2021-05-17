using System;

namespace Core.Exceptions
{
    public class SqlTransactionNotInitializedException : Exception
    {
        public SqlTransactionNotInitializedException() { }
        public SqlTransactionNotInitializedException(string message) : base(message) { }
    }
}
