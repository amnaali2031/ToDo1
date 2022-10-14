using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_Common.Extentions;

namespace ToDo_ModelView.ModelView
{
    public class ItemResponse
    {
        public PagedResult<ItemModelView> Item { get; set; }

        public Dictionary<int, UserResult> User { get; set; }
    }
}
