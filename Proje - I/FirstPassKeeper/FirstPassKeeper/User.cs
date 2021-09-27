using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPassKeeper
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<List<string>> AllPasswords { get; set; } // 1 Password: [string name, string password, string category]
        public List<string> Categories { get; set; }
    }
}
