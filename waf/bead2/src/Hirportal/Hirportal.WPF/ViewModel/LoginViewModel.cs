using System;
using System.Windows.Controls;
using Hirportal.WPF.Persistence;
using Hirportal.Persistence.DTO;

namespace Hirportal.WPF.ViewModel
{

    public class LoginViewModel : ViewModelBase
    {
        private INewsPersistence _model;

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand LoginCommand { get; private set; }

        public String UserName { get; set; }

        public event EventHandler ExitApplication;

        public event EventHandler<AuthorDTO> LoginSuccess;

        public event EventHandler LoginFailed;

        public LoginViewModel(INewsPersistence model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            UserName = String.Empty;

            ExitCommand = new DelegateCommand(param => OnExitApplication());

            LoginCommand = new DelegateCommand(param => LoginAsync(param as PasswordBox));
        }

        private async void LoginAsync(PasswordBox passwordBox)
        {
            if (passwordBox == null) return;

            try
            {
                AuthorDTO result = await _model.LoginAsync(UserName, passwordBox.Password);

                //_model.LoginAsync(UserName, passwordBox.Password)

                if (result != null)
                    OnLoginSuccess(result);
                else
                    OnLoginFailed();
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("No connection to the service.");
            }
        }

        private void OnLoginSuccess(AuthorDTO author)
        {
            LoginSuccess?.Invoke(this, author);
        }

        private void OnExitApplication()
        {
            ExitApplication?.Invoke(this, EventArgs.Empty);
        }

        private void OnLoginFailed()
        {
            LoginFailed?.Invoke(this, EventArgs.Empty);
        }

    }
}
