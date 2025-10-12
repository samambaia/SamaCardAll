#nullable enable
using Microsoft.JSInterop;

namespace FrontWeb.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _jsRuntime;

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // 🚨 NOVO: Salva AMBOS os tokens após login/refresh
        public async Task SetTokensAsync(string accessToken, string refreshToken)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accessToken", accessToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "refreshToken", refreshToken);
        }

        // 🚨 NOVO: Obtém o Access Token (usado pelo AuthHeaderHandler)
        public async Task<string?> GetAccessTokenAsync()
        {
            // Tenta obter o token com o nome correto.
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "accessToken");

            return token;
        }

        // 🚨 NOVO: Obtém o Refresh Token (usado pelo TokenRefreshHandler)
        public async Task<string?> GetRefreshTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "refreshToken");
        }

        // 🚨 NOVO: Remove AMBOS os tokens no Logout
        public async Task RemoveTokensAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "accessToken");
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
        }
    }
}