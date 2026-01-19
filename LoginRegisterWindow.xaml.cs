using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Proyecto_Interfaces
{
    public partial class LoginRegisterWindow : Window
    {
        private const string PlaceholderUsuario = "Nombre de usuario";
        private const string PlaceholderEmail = "Email (opcional)";
        private bool _isRegisterMode = false;

        public string CurrentUserName { get; private set; } = string.Empty;

        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private void TxtUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtUsuario.Text == PlaceholderUsuario)
            {
                TxtUsuario.Text = "";
                TxtUsuario.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TxtUsuario_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtUsuario.Text))
            {
                TxtUsuario.Text = PlaceholderUsuario;
                TxtUsuario.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void TxtEmail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtEmail.Text == PlaceholderEmail)
            {
                TxtEmail.Text = "";
                TxtEmail.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TxtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                TxtEmail.Text = PlaceholderEmail;
                TxtEmail.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        private void OnToggleModeClick(object sender, MouseButtonEventArgs e)
        {
            _isRegisterMode = !_isRegisterMode;

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

            HideMessage();
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

            if (nombre == PlaceholderUsuario || string.IsNullOrWhiteSpace(nombre))
            {
                ShowMessage("Por favor, introduce un nombre de usuario.", true);
                return;
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                ShowMessage("Por favor, introduce una contraseña.", true);
                return;
            }

            if (email == PlaceholderEmail) email = "";

            if (App.DbService.RegistrarUsuario(nombre, contraseña, email))
            {
                ShowMessage("Usuario registrado exitosamente.", false);
                // Opcional: cerrar ventana o limpiar campos
            }
            else
            {
                ShowMessage("El usuario ya existe.", true);
            }
        }

        private void IniciarSesion()
        {
            string nombre = TxtUsuario.Text;
            string contraseña = TxtPassword.Password;

            if (nombre == PlaceholderUsuario || string.IsNullOrWhiteSpace(nombre))
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