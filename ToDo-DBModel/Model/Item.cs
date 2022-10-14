using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace ToDo_MainProject.Model
{
    public partial class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [DefaultValue("")]
        public string Image { get; set; }
        public byte IsReadData { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public byte Archived { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public object ToList()
        {
            throw new NotImplementedException();
        }
    }
}
