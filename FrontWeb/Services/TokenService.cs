// Services/TokenService.cs
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

        public async Task SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
        }

        public async Task RemoveTokenAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        }
    }
}