using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProgramServer.Application.Services.RequestDecrypt.Models;
namespace ProgramServer.Application.Services.RequestDecrypt
{
    public class RequestDecryptService : IRequestDecryptService
    {
        private readonly DecryptionKeyConfiguration configuration;
        private DecryptionKey _decryptionKey;
        private readonly HttpClient httpClient;
        public RequestDecryptService(IConfiguration config)
        {
            configuration = config.GetSection("DecryptionKeyConfiguration").Get<DecryptionKeyConfiguration>();

            httpClient = new HttpClient();
        }
        public async Task RenewPrivateKey()
        {

            var response = await httpClient.GetAsync(configuration.ProviderUrl + configuration.ProgramId.ToString());

            var key = await response.Content.ReadAsStringAsync();

            _decryptionKey = JsonConvert.DeserializeObject<DecryptionKey>(key);
        }

        public async Task<string> GetDecryptionKey()
        {
            if( _decryptionKey == null )
                await RenewPrivateKey();

            if (_decryptionKey.ValidUntilDate < DateTime.Now)
               await RenewPrivateKey();

            return _decryptionKey.PrivateKey;
        }
    }
}
