using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using ToDo_Common.Exceptions;
using ToDo_Core.Manager.Interface;
using ToDo_ModelView.ModelView;

namespace ToDo_MainProject.Controllers
{
    public class ApiBaseController : Controller
    {

        private UserModel _loggedInUser;
        protected UserModel LoggedInUser
        {
            get
            {
                if (_loggedInUser != null)
                {
                    return _loggedInUser;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _loggedInUser = null;
                    return _loggedInUser;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");

                _ = int.TryParse(ClaimId.Value, out int idd);

                if (ClaimId == null || !int.TryParse(ClaimId.Value, out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                }

                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;

                _loggedInUser = commonManager.GetUserRole(new UserModel { Id = id });
              
                return _loggedInUser;
            }
        }

        public ApiBaseController()
        {
        }
    }
}
