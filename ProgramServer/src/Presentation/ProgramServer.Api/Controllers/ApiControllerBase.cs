using ProgramServer.Api.Filters;
using ProgramServer.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProgramServer.Api.Controllers
{
    [ApiController]
    [UserFilter]
    public class ApiControllerBase : ControllerBase
    {
        public UserInfo UserInfo { get; set; }
    }
}
