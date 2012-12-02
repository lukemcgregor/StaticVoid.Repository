using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticVoid.Repository
{
    public class RepositoryTrackingException : Exception
    {
        public RepositoryTrackingException(String message, Exception inner) : base(message, inner) { }
    }
}
