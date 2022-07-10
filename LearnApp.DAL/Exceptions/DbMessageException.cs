using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Exceptions
{
    [Serializable]
    public class DbMessageException : Exception
    {
        public DbMessageException() { }
        public DbMessageException(string message) : base(message) { }
        public DbMessageException(string message, Exception inner) : base(message, inner) { }
        protected DbMessageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
