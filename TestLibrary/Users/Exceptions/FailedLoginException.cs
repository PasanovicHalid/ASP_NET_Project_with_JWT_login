using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary.Users.Exceptions
{

    public class FailedLoginException : Exception
    {
        public FailedLoginException(string? message) : base(message)
        {
        }
    }
}
