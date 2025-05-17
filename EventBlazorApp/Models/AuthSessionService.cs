using Blazored.LocalStorage;

namespace EventBlazorApp.Models
{
    public class AuthSessionService
    {
        private readonly ILocalStorageService? _localStorage;
        private const string TokenKey = "auth_token";
        private const string UsernameKey = "auth_username";

        public string? JwtToken { get; set; }
        public string? Username { get; set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(JwtToken);

        public AuthSessionService()
        {
        }

        public AuthSessionService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task LoadAsync()
        {
            if (_localStorage == null) return;
            JwtToken = await _localStorage.GetItemAsync<string>(TokenKey);
            Username = await _localStorage.GetItemAsync<string>(UsernameKey);
        }

        public async Task SaveAsync()
        {
            if (_localStorage == null) return;
            if (!string.IsNullOrEmpty(JwtToken))
                await _localStorage.SetItemAsync(TokenKey, JwtToken);
            else
                await _localStorage.RemoveItemAsync(TokenKey);

            if (!string.IsNullOrEmpty(Username))
                await _localStorage.SetItemAsync(UsernameKey, Username);
            else
                await _localStorage.RemoveItemAsync(UsernameKey);
        }

        public async Task ClearAsync()
        {
            JwtToken = null;
            Username = null;
            if (_localStorage != null)
            {
                await _localStorage.RemoveItemAsync(TokenKey);
                await _localStorage.RemoveItemAsync(UsernameKey);
            }
        }

        public void Clear()
        {
            JwtToken = null;
            Username = null;
        }
    }
}
