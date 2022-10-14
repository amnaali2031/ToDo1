using AutoMapper;
using System.Linq;
using ToDo_Core.Manager.Interface;
using ToDo_MainProject.Model;
using ToDo_ModelView.ModelView;

namespace ToDo_Core.Manager
{
    public class RoleManager : IRoleManager
    {

        #region public

            private DB_ToDoContext _DB;
            private IMapper _mapper;

            public RoleManager(DB_ToDoContext db, IMapper mapper)
            {
                _DB = db;
                _mapper = mapper;
            }

            public bool CheckAccess(UserModel userModel)
            {
                var isAdmin = _DB.Users
                                       .Any(a => a.Id == userModel.Id
                                        && a.IsAdmin == 1);
                return isAdmin;
            }
        
        #endregion public

    }
}
