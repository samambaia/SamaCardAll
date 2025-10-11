using FrontWeb.Services;
using Microsoft.JSInterop;
using System.Net;
using System.Net.Http.Headers;

namespace FrontWeb.Handlers
{
    public class TokenRefreshHandler : DelegatingHandler
    {
        private readonly IServiceProvider _serviceProvider;

        // Injetamos o AuthenticationStateProvider e o IJSRuntime
        public TokenRefreshHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // 🚨 RESOLVE O AUTH PROVIDER DENTRO DO SCOPE NECESSÁRIO
            // usando o IServiceProvider para garantir a instância correta
            var authProvider = _serviceProvider.GetRequiredService<CustomAuthenticationStateProvider>();
            var jsRuntime = _serviceProvider.GetRequiredService<IJSRuntime>();

            // 1. Tentar a requisição original
            var response = await base.SendAsync(request, cancellationToken);

            // 2. Se falhou por motivo de token expirado (401)
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken");

                if (string.IsNullOrEmpty(refreshToken))
                {
                    await authProvider.LogoutAsync();
                    return response;
                }

                // 3. Chamar a lógica de Refresh (usando a instância resolvida)
                var refreshSuccessful = await authProvider.RefreshTokensAsync(refreshToken);

                if (refreshSuccessful)
                {
                    var newAccessToken = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);

                    // 4. Tentar a requisição original novamente
                    response.Dispose();
                    return await base.SendAsync(request, cancellationToken);
                }
                else
                {
                    await authProvider.LogoutAsync();
                    throw new HttpRequestException("Sessão expirada. Renovação de token falhou.", null, HttpStatusCode.Unauthorized);
                }
            }

            return response;
        }
    }
}
