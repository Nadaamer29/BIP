using BIP.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.DataAccess.Entities
{
    public class Role : IdentityRole<string>
    {

        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
