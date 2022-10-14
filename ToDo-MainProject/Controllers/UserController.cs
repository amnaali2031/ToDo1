using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDo_Core.Manager.Interface;
using ToDo_MainProject.Attributes;
using ToDo_ModelView.ModelView;

namespace ToDo_MainProject.Controllers
{
    
    [ApiController]
    public class UserController : ApiBaseController
    {
        private IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,
                              IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }


        [Route("api/user/signUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] UserRegistrationModel userReg)
        {
            var res = _userManager.SignUp(userReg);
            return Ok(res);
        }


        [Route("api/user/login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModelView userReg)
        {
            var res = _userManager.Login(userReg);
            return Ok(res);
        }


        [Route("api/user/UpdateProfile")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateMyProfile(UserUpdatedModel request)
        {
            var user = _userManager.UpdateProfile(LoggedInUser, request);
            return Ok(user);
        }


        [HttpDelete]
        [Route("api/user/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _userManager.DeleteUser(LoggedInUser, id);
            return Ok();
        }


        [HttpGet]
        [Route("api/user/GetAllUsres")]
        [Authorize]
        [ToDoAuthrize()]
        public IActionResult ExportToCsv()
        {
           var res = _userManager.GetAll();
            return Ok(res);
        }

    }
}
