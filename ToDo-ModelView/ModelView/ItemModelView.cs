using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_ModelView.ModelView
{
    public class ItemModelView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [DefaultValue("")]
        public string Image { get; set; }
        public byte IsReadData { get; set; }
        public int UserId { get; set; }

    }
}
