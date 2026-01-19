using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Proyecto_Interfaces
{
    public partial class MainWindow : Window
    {
        public string CurrentUserName { get; set; } = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            // Al iniciar, mostramos el logo (ninguna sección seleccionada)
            MostrarLogo();
        }

        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            var media = sender as MediaElement;
            media?.Stop();
            media?.Play();
        }

        private void OnVideoOpened(object sender, RoutedEventArgs e)
        {
            var media = sender as MediaElement;
            if (media != null && media.Visibility == Visibility.Visible)
            {
                media.Play();
            }
        }

        private void OnVideoFailed(object sender, System.Windows.ExceptionRoutedEventArgs e)
        {
            VideoPlaceholder.Visibility = Visibility.Collapsed;
            MessageBox.Show($"Error al cargar el video: {e.ErrorException.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void MostrarContenido(UIElement contenido)
        {
            VideoPlaceholder.Visibility = Visibility.Collapsed;
            MainContent.Content = contenido;
            MainContent.Visibility = Visibility.Visible;
        }

        private void MostrarLogo()
        {
            VideoPlaceholder.Visibility = Visibility.Visible;
            MainContent.Visibility = Visibility.Collapsed;
            VideoPlaceholder.Play();
            
        }
        private void OnInicioClick(object sender, RoutedEventArgs e)
        {
            MostrarLogo();
            
        }



        private void CargarVistaViajes()
        {
            MostrarContenido(new ViajesView());
        }

        private void CargarVistaCruceros()
        {
            MostrarContenido(new CrucerosView());
        }

        private void CargarVistaTrenes()
        {
            MostrarContenido(new TrenesView());
        }

        private void CargarVistaEurotrip()
        {
            MostrarContenido(new EurotripView());
        }

        

        private void OnViajesClick(object sender, RoutedEventArgs e) => CargarVistaViajes();
        private void OnCrucerosClick(object sender, RoutedEventArgs e) => CargarVistaCruceros();
        private void OnTrenesClick(object sender, RoutedEventArgs e) => CargarVistaTrenes();
        private void OnEurotripClick(object sender, RoutedEventArgs e) => CargarVistaEurotrip();

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginRegisterWindow();
            loginWindow.Owner = this;

            if (loginWindow.ShowDialog() == true)
            {
              
                CurrentUserName = loginWindow.CurrentUserName;
                LoginButton.Content = CurrentUserName;
            }
        }
        private void OnMisReservasClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentUserName))
            {
                MessageBox.Show("Debes iniciar sesión para ver tus reservas.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MostrarContenido(new MisReservasView());
        }


        public void MostrarDetalleProducto(DetalleProductoView.Producto producto)
        {
            MostrarContenido(new DetalleProductoView(producto, this));
        }
    }
}