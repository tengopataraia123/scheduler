using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Services.RequestDecrypt;
using System.Security.Cryptography;
using System.Text;

namespace ProgramServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ApiControllerBase
    {
        private readonly IRequestDecryptService service;
        public TestController(IRequestDecryptService service)
        {
            this.service = service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestKey()
        {
            await service.RenewPrivateKey();
            var crypto = new RSACryptoServiceProvider(1024);

            var message = "hello";
            crypto.ImportFromPem(await service.GetDecryptionKey());

            var result = crypto.Encrypt(Encoding.Unicode.GetBytes(message), true);

            return Ok(Encoding.Unicode.GetString(result));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> TestAuth()
        {
            return Ok();
        }
    }
}
