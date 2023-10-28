using MainServer.Api.Filters;
using MainServer.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainServer.Api.Controllers
{
    [ApiController]
    [UserFilter]
    public class ApiControllerBase : ControllerBase
    {
        public UserInfo UserInfo { get; set; }
    }
}
