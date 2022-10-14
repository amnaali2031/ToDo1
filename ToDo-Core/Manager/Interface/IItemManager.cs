using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_ModelView.ModelView;
using ToDo_ModelView.ModelView.Request;

namespace ToDo_Core.Manager.Interface
{
    public interface IItemManager : IManager
    {
        ItemResponse GetItems(UserModel currentUser,bool IsReadDataFilter,int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "");

        ItemModelView GetItem(UserModel currentUser, int id);

        ItemModelView PutItem(UserModel currentUser, ItemRequest request);

        void ArchiveItem(UserModel currentUser, int id);

        void AssignItem(UserModel currentUser, int UserId , int ItemId);

    }
}
