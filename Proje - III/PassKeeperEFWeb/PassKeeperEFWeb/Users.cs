using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKeeperEFWeb
{
    public class Users
    {
        public int UserID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
