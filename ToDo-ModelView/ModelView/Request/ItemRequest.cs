using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_ModelView.ModelView.Request
{
    public class ItemRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageString { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }

    }
}
