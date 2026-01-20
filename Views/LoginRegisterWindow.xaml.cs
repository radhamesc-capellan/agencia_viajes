using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Proyecto_Interfaces;

namespace Proyecto_Interfaces.Views
{
    public partial class LoginRegisterWindow : Window
    {
        private bool _isRegisterMode = false;

        public string CurrentUserName { get; private set; } = string.Empty;

        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnWindowDrag(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void OnToggleModeClick(object sender, MouseButtonEventArgs e)
        {
            _isRegisterMode = !_isRegisterMode;
            UpdateViewForCurrentMode();
            HideMessage();
        }

        private void UpdateViewForCurrentMode()
        {
            if (_isRegisterMode)
            {
                TitleTextBlock.Text = "¡Regístrate, viajero! ✈️";
                PrimaryButton.Content = "📝 Registrarme";
                ToggleTextBlock.Text = "¿Ya tienes cuenta? Inicia sesión aquí";
                TxtEmail.Visibility = Visibility.Visible;
            }
            else
            {
                TitleTextBlock.Text = "¡Hola, viajero! 🌍";
                PrimaryButton.Content = "🔐 Iniciar sesión";
                ToggleTextBlock.Text = "¿Aún no tienes cuenta? Regístrate aquí";
                TxtEmail.Visibility = Visibility.Collapsed;
            }
        }

        private void OnPrimaryButtonClick(object sender, RoutedEventArgs e)
        {
            if (_isRegisterMode)
            {
                RegistrarUsuario();
            }
            else
            {
                IniciarSesion();
            }
        }

        private void RegistrarUsuario()
        {
            string nombre = TxtUsuario.Text;
            string contraseña = TxtPassword.Password;
            string email = TxtEmail.Text;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                ShowMessage("Por favor, introduce un nombre de usuario.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                ShowMessage("Por favor, introduce una contraseña.", true);
                return;
            }

            // Email es opcional, pero si lo agrego tiene que ser valido
            if (!string.IsNullOrWhiteSpace(email) && !IsValidEmail(email))
            {
                ShowMessage("Por favor, introduce un email válido (ej: usuario@dominio.com).", true);
                return;
            }

            if (App.DbService.RegistrarUsuario(nombre, contraseña, email))
            {
                // Éxito: Cambiamos a modo Login automáticamente
                _isRegisterMode = false;
                UpdateViewForCurrentMode();
                TxtPassword.Clear(); // Limpiamos la contraseña por seguridad
                ShowMessage("¡Registro exitoso! Por favor inicia sesión.", false);
            }
            else
            {
                ShowMessage("El usuario ya existe.", true);
            }
        }
        //Validacion del campo email
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex para emails
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private void IniciarSesion()
        {
            string nombre = TxtUsuario.Text;
            string contraseña = TxtPassword.Password;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                ShowMessage("Por favor, introduce un nombre de usuario.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                ShowMessage("Por favor, introduce una contraseña.", true);
                return;
            }

            if (App.DbService.Login(nombre, contraseña))
            {
                CurrentUserName = nombre;
                MessageBox.Show("Inicio de sesión exitoso.", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                ShowMessage("Usuario o contraseña incorrectos.", true);
            }
        }
       

        private void ShowMessage(string message, bool isError)
        {
            MessageTextBlock.Text = message;
            MessageTextBlock.Foreground = isError ? System.Windows.Media.Brushes.Red : System.Windows.Media.Brushes.Green;
            MessageTextBlock.Visibility = Visibility.Visible;
        }


        private void HideMessage()
        {
            MessageTextBlock.Visibility = Visibility.Collapsed;
        }
    }
}