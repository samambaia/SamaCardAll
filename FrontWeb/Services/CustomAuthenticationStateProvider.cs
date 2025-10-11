using FrontWeb.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace FrontWeb.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly TokenService _tokenService;

        public CustomAuthenticationStateProvider(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetTokenAsync();

            if (string.IsNullOrEmpty(token))
            {
                // Não autenticado
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                // Decodifica o JWT (sem validar assinatura — só para leitura de claims)
                var payload = GetPayloadFromJwt(token);
                var claims = new List<Claim>();

                // Adiciona claims do token (ex: sub, email, FullName)
                if (payload.TryGetValue("sub", out var userId))
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
                if (payload.TryGetValue("email", out var email))
                    claims.Add(new Claim(ClaimTypes.Email, email.ToString()));
                if (payload.TryGetValue("FullName", out var fullName))
                    claims.Add(new Claim(ClaimTypes.Name, fullName.ToString()));

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                // Token inválido? Trata como não autenticado
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        // Notifica o app que o estado de autenticação mudou (ex: após login/logout)
        public void NotifyUserAuthentication(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());
            if (!string.IsNullOrEmpty(token))
            {
                var payload = GetPayloadFromJwt(token);
                var claims = new List<Claim>();
                if (payload.TryGetValue("email", out var email))
                    claims.Add(new Claim(ClaimTypes.Email, email.ToString()));
                if (payload.TryGetValue("FullName", out var fullName))
                    claims.Add(new Claim(ClaimTypes.Name, fullName.ToString()));

                authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
            }

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        // Método auxiliar para decodificar o payload do JWT
        private static Dictionary<string, object> GetPayloadFromJwt(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length != 3)
                throw new ArgumentException("Token JWT inválido");

            var payloadJson = Base64UrlDecode(parts[1]);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(payloadJson)
                   ?? new Dictionary<string, object>();
        }

        private static string Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 2: output += "=="; break;
                case 3: output += "="; break;
            }
            var bytes = Convert.FromBase64String(output);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}