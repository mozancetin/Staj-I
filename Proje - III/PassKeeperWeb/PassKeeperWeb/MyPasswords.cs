using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PassKeeperWeb
{
    public class MyPasswords
    {
        public int PasswordID { get; set; }
        public int UserID { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public int CategoryID { get; set; }
    }
}
