using MainServer.Api.Controllers;
using MainServer.Application.Common.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MainServer.Api.Filters
{
    public class UserFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var c = context.Controller as ApiControllerBase;
            c.UserInfo = new UserInfo()
            {
                Id = Convert.ToInt32(c.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                RoleName = c.User.FindFirstValue(ClaimTypes.Role)
            };
        }
    }
}
