using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary.Users.Exceptions
{
    public class UserDoesntExistException : Exception
    {
        public UserDoesntExistException(string? message) : base(message)
        {
        }
    }
}
