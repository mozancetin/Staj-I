using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PassKeeperWeb.Models
{
    public partial class User
    {
        public User()
        {
            Categories = new HashSet<Category>();
            Passwords = new HashSet<Password>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Password> Passwords { get; set; }
    }
}
