using System;
using System.Collections.Generic;

#nullable disable

namespace ToDo_MainProject.Model
{
    public partial class User
    {
        public User()
        {
            InverseUpdatedByNavigation = new HashSet<User>();
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public byte Archived { get; set; }
        public int? UpdatedBy { get; set; }
        public string Images { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public virtual User UpdatedByNavigation { get; set; }
        public virtual ICollection<User> InverseUpdatedByNavigation { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
