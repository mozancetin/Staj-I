using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PassKeeperWeb.Models;

namespace PassKeeperWeb
{
    public class AllData
    {
        public User MyUser { get; set; }
        public List<Password> MyPasswords { get; set; }
        public List<Category> MyCategories { get; set; }
    }
}
