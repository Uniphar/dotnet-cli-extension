
using Microsoft.Identity.Client;

namespace Uniphar.Dotnet.Cli.Token
{
    internal interface ITokenService
    {
        Task<string> GetAccessTokenAsync(string tenantId, string clientId);
    }

    internal class TokenService : ITokenService
    {
        private const string Scope = ".default";

        public async Task<string> GetAccessTokenAsync(string tenantId, string clientId)
        {
            var app = PublicClientApplicationBuilder.Create(clientId)
                                                    .WithTenantId(tenantId)
                                                    .WithDefaultRedirectUri()
                                                    .Build();
            try
            {
                var result = await app.AcquireTokenInteractive([$"{clientId}/{Scope}"]).ExecuteAsync();
                return result.AccessToken;
            }
            catch (MsalClientException msalEx)
            {
                throw new Exception($"MSAL Client Error: {msalEx.Message}");
            }
            catch (MsalServiceException serviceEx)
            {
                throw new Exception($"MSAL Service Error: {serviceEx.Message}");
            }
        }
    }
}
