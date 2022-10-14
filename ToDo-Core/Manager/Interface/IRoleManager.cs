using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_ModelView.ModelView;

namespace ToDo_Core.Manager.Interface
{
    public interface IRoleManager
    {
        bool CheckAccess(UserModel userModel);

    }
}
