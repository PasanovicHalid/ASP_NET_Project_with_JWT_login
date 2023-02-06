using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary.Users.Model
{
    public class UserRole : IdentityRole<Guid>
    {
        public UserRole(string name) : base(name)
        {
        }
    }
}
