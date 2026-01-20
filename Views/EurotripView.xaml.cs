using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_Interfaces.Views
{
    public partial class EurotripView : UserControl
    {
        public EurotripView()
        {
            InitializeComponent();
        }

        private void OnContactarClick(object sender, RoutedEventArgs e)
        {
            if (ValidarFormulario())
            {
                MessageBox.Show("¡Gracias por tu interés! Te contactaremos pronto para planificar tu Eurotrip ideal.", "Formulario enviado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, revisa los campos e inténtalo de nuevo.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text))
            {
                MessageBox.Show("Por favor, introduce tu nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ApellidosTextBox.Text))
            {
                MessageBox.Show("Por favor, introduce tus apellidos.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(NacionalidadTextBox.Text))
            {
                MessageBox.Show("Por favor, introduce tu nacionalidad.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (CiudadSalidaComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor, selecciona tu ciudad de salida.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text) || !ValidarEmail(EmailTextBox.Text))
            {
                MessageBox.Show("Por favor, introduce un correo electrónico válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(TelefonoTextBox.Text) || !ValidarTelefono(TelefonoTextBox.Text))
            {
                MessageBox.Show("Por favor, introduce un número de teléfono válido.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool ValidarEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        private bool ValidarTelefono(string telefono)
        {
            var regex = new Regex(@"^[0-9+\-\s()]+$");
            return regex.IsMatch(telefono) && telefono.Length >= 9;
        }
    }
}