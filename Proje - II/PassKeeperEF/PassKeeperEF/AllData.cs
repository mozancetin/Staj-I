using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKeeperEF
{
    public class AllData
    {
        public List<Users> MyUsers { get; set; }
        public List<Passwords> MyPasswords { get; set; }
        public List<Categories> MyCategories { get; set; }
    }
}
