
using System.Collections.Generic;
using ToDo_ModelView.ModelView;

namespace ToDo_Core.Manager.Interface
{
    public interface IUserManager : IManager
    {
        LoginUserResponse Login(LoginModelView userReg);
        LoginUserResponse SignUp(UserRegistrationModel userReg);
        void DeleteUser(UserModel currentUser, int id);
        UserModel UpdateProfile(UserModel currentUser, UserUpdatedModel request);
        List<UserModel> GetAll();


    }
}
