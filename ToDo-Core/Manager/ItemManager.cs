using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDo_Common.Exceptions;
using ToDo_Common.Extentions;
using ToDo_Common.Helper;
using ToDo_Core.Manager.Interface;
using ToDo_MainProject.Model;
using ToDo_ModelView.ModelView;
using ToDo_ModelView.ModelView.Request;

namespace ToDo_Core.Manager
{
    public class ItemManager : IItemManager
    {

        #region public

            private DB_ToDoContext _DB;
            private IMapper _mapper;

            public ItemManager(DB_ToDoContext db, IMapper mapper)
            {
                _DB = db;
                _mapper = mapper;
            }

            public void ArchiveItem(UserModel currentUser, int id)
            {

                if (currentUser.IsAdmin == 0)
                {
                    throw new ServiceValidationException("You don't have permission to archive ToDoItem");
                }

                var data = _DB.Items
                                    .FirstOrDefault(a => a.Id == id)
                                     ?? throw new ServiceValidationException("Invalid ToDoItem id received");
                data.Archived = 1;
                _DB.SaveChanges();
            }

            public ItemResponse GetItems(UserModel currentUser,
                                         bool IsReadDataFilter,
                                         int page = 1,
                                         int pageSize = 10,
                                         string sortColumn = "",
                                         string sortDirection = "ascending",
                                         string searchText = "")
            {
                
                var x = IsReadDataFilter? 1 : 0;
                var queryRes = _DB.Items
                                        .Where(a => (a.IsReadData == x) 
                                                    &&(string.IsNullOrWhiteSpace(searchText)
                                                    || (a.Title.Contains(searchText)
                                                    || a.Content.Contains(searchText))));

                if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryRes = queryRes.OrderBy(sortColumn);
                }
                else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
                {
                    queryRes = queryRes.OrderByDescending(sortColumn);
                }

                var res = queryRes.GetPaged(page, pageSize);

                if (currentUser.IsAdmin == 0)
                {
                        var user = _DB.Users.
                                             Where(a => a.Id == currentUser.Id)
                                             .ToDictionary(a => a.Id, x => _mapper.Map<UserResult>(x))
                                             ?? throw new ServiceValidationException("un Authoriza ");    


                    
                        var items = _DB.Items
                                            .Where(a => (a.UserId == currentUser.Id)
                                                        && (a.IsReadData == x)).ToList();
                        res.Data = items;

                        var Item1 = _mapper.Map<PagedResult<ItemModelView>>(res);

                        var dataa = new ItemResponse
                        {
                            Item = _mapper.Map<PagedResult<ItemModelView>>(res),
                            User = user
                        };

                        dataa.Item.Sortable.Add("Title", "Title");
                        dataa.Item.Sortable.Add("CreatedDate", "Created Date");

                        return dataa;
            }


                        var userIds = res.Data
                                         .Select(a => a.UserId)
                                         .Distinct()
                                         .ToList();

                        var users = _DB.Users
                                             .Where(a => userIds.Contains(a.Id))
                                             .ToDictionary(a => a.Id, x => _mapper.Map<UserResult>(x));
                     
                var data = new ItemResponse
                {
                    Item = _mapper.Map<PagedResult<ItemModelView>>(res),
                    User = users
                };

                data.Item.Sortable.Add("Title", "Title");
                data.Item.Sortable.Add("CreatedDate", "Created Date");

                return data;
            }


            public ItemModelView GetItem(UserModel currentUser, int id)
            {
                var res = _DB.Items
                                   .Include("User")
                                   .FirstOrDefault(a => a.Id == id)
                                    ?? throw new ServiceValidationException("Invalid ToDoItem id received");

                if (currentUser.IsAdmin == 0)
                {
                    res.IsReadData = 1;
                    _DB.SaveChanges();
                }

                return _mapper.Map<ItemModelView>(res);
            }


            public ItemModelView PutItem(UserModel currentUser, ItemRequest request)
        {
            Item item = null;
            var userId = 0;

            var url = "";
            var Imageurl = "";

            if (!string.IsNullOrWhiteSpace(request.ImageString))
            {
                url = Helper.SaveImage(request.ImageString, "ItemsImages");
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44309/";
                Imageurl = @$"{baseURL}/api/v1/user/fileretrive/profilepic?filename={url}";
            }

            
            if (currentUser.IsAdmin == 0)
            {
                userId = currentUser.Id;
                throw new ServiceValidationException("You don't have permission to add or update blog");
            }


            if (request.Id > 0)
            {
                item = _DB.Items
                                .FirstOrDefault(a => a.Id == request.Id)
                                 ?? throw new ServiceValidationException("Invalid Item id received");

                
                item.Title = request.Title;
                item.Content = request.Content;
                item.Image = Imageurl;
                item.UserId = currentUser.IsAdmin == 1 ? request.UserId : userId;
                

            }
            else
            {

                item = _DB.Items.Add(new Item
                {
                    Title = request.Title,
                    Content = request.Content,
                    UserId = currentUser.IsAdmin == 1? request.UserId : userId,
                    Image = Imageurl

                }).Entity;
            }



            _DB.SaveChanges();
            return _mapper.Map<ItemModelView>(item);
        }

        public ItemModelView AssignItem(UserModel currentUser, ItemAssignRequest request)
        {

           
            if (currentUser.IsAdmin == 0)
            {
                throw new ServiceValidationException("You don't have permission to add or update blog");
            }


            var res = _DB.Items
                                .Include("User")
                                .FirstOrDefault(a => a.Id == request.ItemId)
                                 ?? throw new ServiceValidationException("Invalid ToDoItem id received");


            res.UserId = request.UserId;
            _DB.Items.Update(res);

            _DB.SaveChanges();
            return _mapper.Map<ItemModelView>(res);


        }

        #endregion public

    }
}
