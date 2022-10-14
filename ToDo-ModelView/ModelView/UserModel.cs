using System;
using System.ComponentModel;

namespace ToDo_ModelView.ModelView
{
    public class UserModel
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public byte IsAdmin { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public byte Archived { get; set; }
        public int? UpdatedBy { get; set; }

        [DefaultValue("")]
        public string Images { get; set; }
        public string ImageString { get; set; }



    }
}
