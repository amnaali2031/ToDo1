using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ToDo_Core.Manager.Interface;
using ToDo_MainProject.Attributes;
using ToDo_ModelView.ModelView.Request;

namespace ToDo_MainProject.Controllers
{

    [ApiController]
    public class ItemController : ApiBaseController
    {
        private IItemManager _itemManager;
        private readonly ILogger<UserController> _logger;


        public ItemController(ILogger<UserController> logger,
                              IItemManager itemManager)
        {
            _logger = logger;
            _itemManager = itemManager;
        }

        [Route("api/Items")]
        [HttpGet]
        public IActionResult GetItems(bool IsReadData, int page = 1, int pageSize = 5, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {

            var result = _itemManager.GetItems(LoggedInUser, IsReadData, page, pageSize, sortColumn, sortDirection, searchText);
            return Ok(result);
        }


        [Route("api/Item/{id}")]
        [HttpGet]
        public IActionResult GetItem(int id)
        {
            var result = _itemManager.GetItem(LoggedInUser,id);
            return Ok(result);
        }

        [Route("api/Admin/Item/{id}")]
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ToDoAuthrize()]
        public IActionResult ArchiveItem(int id)
        {
            _itemManager.ArchiveItem(LoggedInUser, id);
            return Ok();
        }

        [Route("api/Admin/Item")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ToDoAuthrize()]
        public IActionResult PutItem(ItemRequest itemRequest)
        {
            var result = _itemManager.PutItem(LoggedInUser, itemRequest);
            return Ok(result);
        }


        [Route("api/Admin/Assign")]
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ToDoAuthrize()]
        public IActionResult AssignItem(ItemAssignRequest request)
        {
            _itemManager.AssignItem(LoggedInUser, request);
            return Ok();
        }
    }
}
