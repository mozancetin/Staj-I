using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PassKeeperWeb.Models
{
    public partial class Password
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PasswordId { get; set; }
        public string Name { get; set; }
        public string Password1 { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}
