using System;

namespace FrontWeb.Services
{
    public class LoadingService
    {
        // Evento que notifica qualquer componente inscrito sobre mudanças de estado
        public event Action? OnChange;

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    NotifyStateChanged(); // Dispara o evento
                }
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public void Show() => IsLoading = true;
        public void Hide() => IsLoading = false;
    }
}