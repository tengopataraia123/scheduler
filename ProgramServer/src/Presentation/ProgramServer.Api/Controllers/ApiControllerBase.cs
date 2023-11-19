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
            var userRepositry = context.HttpContext.RequestServices.GetService<IRepository<Domain.Users.User>>();
            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

            if (userRepositry == null || cache == null)
                throw new ApplicationException();
            
            var email = "";
            var user = new Domain.Users.User();
            var c = context.Controller as ApiControllerBase;
            var emailClaim = c.User.Claims.FirstOrDefault(o => o.Type.Contains("emailaddress"));

            if (emailClaim != null)
                email = emailClaim.Value;

            if (!cache.TryGetValue(email, out user))
            {
                user = userRepositry.Where(o => o.Email == email).Include(o => o.Role).FirstOrDefault();

                if (user == null)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                cache.Set(email, user);
            }

            c.CurrentUser = new UserInfo()
            {
                Id = user.Id,
                RoleName = user.Role.RoleName
            };
        }
    }
}
