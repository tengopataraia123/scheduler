using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ProgramServer.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ProgramServer.Application.Repository;

namespace ProgramServer.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ApiControllerBase : Controller
    {
        public UserInfo CurrentUser { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var c = context.Controller as ApiControllerBase;
            var emailClaim = c.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Email);
            var userId = Convert.ToInt32(c.User.Claims.FirstOrDefault(o=>o.Type == ClaimTypes.NameIdentifier)?.Value);
            var role = c.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value ?? string.Empty;
            
            c.CurrentUser = new UserInfo()
            {
                Id = userId,
                RoleName = role
            };
        }
    }
}
