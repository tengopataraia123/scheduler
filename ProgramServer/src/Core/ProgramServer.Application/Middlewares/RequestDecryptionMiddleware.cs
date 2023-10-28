using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Services.RequestDecrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.Middlewares
{
    public class RequestDecryptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IRequestDecryptService _requestDecryptService;

        public RequestDecryptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IRequestDecryptService requestDecryptService)
        {
            _next = next;
            _logger = logger;
            _requestDecryptService = requestDecryptService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await DecryptReqeust(httpContext);
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
            }
        }

        private async Task DecryptReqeust(HttpContext httpContext)
        {
            var privateKey = await _requestDecryptService.GetDecryptionKey();
            var cryptoService = new RSACryptoServiceProvider(1024);

            cryptoService.ImportFromPem(privateKey);

            //cryptoService.ImportRSAPrivateKey(Encoding.UTF8.GetBytes(privateKey), out int bitesRead);

            var original = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
            var bytes = Convert.FromBase64String(original);

            var decrypted = cryptoService.Decrypt(bytes, false);

            httpContext.Response.Body = new MemoryStream(decrypted);

            var newBody = Encoding.Default.GetString(decrypted);

        }
    }
}
