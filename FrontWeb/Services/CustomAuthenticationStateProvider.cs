using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using SamaCardAll.Shared.Contracts.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace FrontWeb.Services
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly TokenService _tokenService;
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthenticationStateProvider(TokenService tokenService, HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetAccessTokenAsync();

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

        // Este método notifica o Blazor para redefinir o estado.
        public void NotifyUserLogout()
        {
            var anonymousIdentity = new ClaimsIdentity();
            var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);
            var authState = Task.FromResult(new AuthenticationState(anonymousPrincipal));

            // Este é o método interno do AuthenticationStateProvider
            NotifyAuthenticationStateChanged(authState);
        }

        public async Task LoginAsync(string accessToken, string refreshToken)
        {
            // 1. Salvar AMBOS os tokens
            await _tokenService.SetTokensAsync("accessToken", "refreshToken");

            // 2. Notificar o Blazor
            NotifyUserAuthentication(accessToken);
        }

        public async Task LogoutAsync()
        {
            // 1. Obter o refresh token atual (para revogar)
            var refreshToken = await _tokenService.GetRefreshTokenAsync();

            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    // 2. Chamar a API para revogar o token no banco de dados (RDS)
                    // Usamos StringContent, pois é o que você prefere.
                    var revokePayload = new { RefreshToken = refreshToken };
                    var content = new StringContent(
                        JsonSerializer.Serialize(revokePayload),
                        System.Text.Encoding.UTF8,
                        "application/json"
                    );

                    // Não verificamos o sucesso aqui, apenas tentamos invalidar o token no servidor.
                    await _httpClient.PostAsync("api/auth/revoke", content);
                }
                catch (Exception ex)
                {
                    // Opcional: logar o erro. Mas o logout do cliente deve continuar.
                    Console.WriteLine($"Erro ao revogar token: {ex.Message}");
                }
            }

            // 3. Remover tokens do armazenamento local (SEMPRE)
            await _tokenService.RemoveTokensAsync();

            // 4. Notificar o Blazor (força o redirecionamento para o /login)
            NotifyUserLogout();
        }

        public async Task<bool> RefreshTokensAsync(string refreshToken)
        {
            try
            {
                // 1. Preparar e Enviar a requisição para a API
                var content = new StringContent(
                    JsonSerializer.Serialize(new { RefreshToken = refreshToken }),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("api/auth/refresh", content);

                if (response.IsSuccessStatusCode)
                {
                    // 2. Deserializar novos tokens (usando sua sintaxe de StringContent e JsonSerializer)
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    // Usamos PropertyNameCaseInsensitive = true para maior robustez na desserialização
                    var tokens = JsonSerializer.Deserialize<TokenResponse>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (tokens is null || string.IsNullOrEmpty(tokens.AccessToken))
                    {
                        // Resposta válida, mas dados inválidos. Trata como falha.
                        throw new ApplicationException("API returned success but token data was missing.");
                    }

                    // 3. Salvar os novos tokens (usando o método auxiliar SetToken baseado em IJSRuntime)
                    await _tokenService.SetTokensAsync(tokens.AccessToken, tokens.RefreshToken);

                    // 4. Notificar o Blazor sobre a mudança de estado
                    // (Isso chama o ParseClaimsFromJwt e NotifyAuthenticationStateChanged)
                    await MarkUserAsAuthenticated(tokens.AccessToken);

                    return true; // Sucesso na renovação
                }
                else
                {
                    await _tokenService.RemoveTokensAsync();
                    NotifyUserLogout();
                    return false; // Falha na renovação
                }
            }
            catch (Exception ex)
            {
                // Logar o erro (opcional)
                Console.WriteLine($"Refresh Token falhou devido a exceção: {ex.Message}");
            }

            // 5. Falha no refresh (código de status HTTP não-sucesso ou exceção)
            // Chamamos LogoutAsync, que lida com a revogação na API e o NotifyUserLogout.
            await LogoutAsync();
            return false;
        }

        // Assuma que este método existe e você o chama para autenticar o usuário após o login/refresh
        public Task MarkUserAsAuthenticated(string accessToken)
        {
            // Método que você usa para parsear o JWT e criar as Claims
            var claims = ParseClaimsFromJwt(accessToken);

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(user));

            // Notifica o Blazor para re-renderizar a UI
            NotifyAuthenticationStateChanged(authState);

            return Task.CompletedTask;
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

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var handler = new JwtSecurityTokenHandler();

            // Tenta decodificar o token
            if (handler.CanReadToken(jwt))
            {
                var token = handler.ReadJwtToken(jwt);

                // Adiciona todas as claims encontradas no token
                claims.AddRange(token.Claims);
            }

            // Opcional: Adicionar claims padrão se o token for inválido/vazio
            if (!claims.Any())
            {
                // Exemplo: new Claim(ClaimTypes.Name, "Anonimo")
            }

            return claims;
        }
    }
}