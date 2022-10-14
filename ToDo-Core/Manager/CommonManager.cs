using AutoMapper;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo_Common.Exceptions;
using ToDo_Core.Manager.Interface;
using ToDo_MainProject.Model;
using ToDo_ModelView.ModelView;

namespace ToDo_Core.Manager
{
    public class CommonManager : ICommonManager
    {

        #region public

            private DB_ToDoContext _DB;
            private IMapper _mapper;

            public CommonManager(DB_ToDoContext csvdbContext, IMapper mapper)
            {
                _DB = csvdbContext;
                _mapper = mapper;
            }

            public UserModel GetUserRole(UserModel user)
            {
                var dbUser = _DB.Users
                                          .FirstOrDefault(a => a.Id == user.Id)
                                          ?? throw new ServiceValidationException("Invalid user id received");

                return _mapper.Map<UserModel>(dbUser);
            }
       
        #endregion public

    }
}
