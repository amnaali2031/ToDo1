using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo_ModelView.ModelView
{
    public class UserUpdatedModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DefaultValue("")]
        public string Images { get; set; }
        public string ImageString { get; set; }
    }
}
