using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Proyecto_Interfaces;
using Proyecto_Interfaces.Services;

namespace Proyecto_Interfaces.Views
{
    public partial class DetalleProductoView : UserControl
    {
        public class Producto
        {
            public string Titulo { get; set; } = string.Empty;
            public string Descripcion { get; set; } = string.Empty;
            public decimal Precio { get; set; }
            public string ImagenPrincipal { get; set; } = string.Empty;
            public List<string> Galeria { get; set; } = new();
        }

        private Producto _producto;
        private MainWindow _mainWindow;

        public DetalleProductoView(Producto producto, MainWindow mainWindow)
        {
            InitializeComponent();
            _producto = producto;
            _mainWindow = mainWindow;
            CargarDatos();
        }

        private void CargarDatos()
        {
            TituloLabel.Text = _producto.Titulo;
            DescripcionLabel.Text = _producto.Descripcion;
            PrecioLabel.Text = $"Precio: €{_producto.Precio:F2}";

            ImagenPrincipal.Source = new BitmapImage(new System.Uri(_producto.ImagenPrincipal, System.UriKind.RelativeOrAbsolute));

            foreach (var imagen in _producto.Galeria)
            {
                var img = new Image
                {
                    Source = new BitmapImage(new System.Uri(imagen, System.UriKind.RelativeOrAbsolute)),
                    Stretch = System.Windows.Media.Stretch.UniformToFill,
                    Height = 100,
                    Width = 150,
                    Margin = new Thickness(5),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                img.MouseLeftButtonDown += (s, e) => {
                    ImagenPrincipal.Source = new BitmapImage(new System.Uri(imagen, System.UriKind.RelativeOrAbsolute));
                };
                GaleriaImagenesPanel.Children.Add(img);
            }
        }

        private void OnReservarClick(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario logueado
            int usuarioId = App.DbService.ObtenerUsuarioIdPorNombre(_mainWindow.CurrentUserName);
            if (usuarioId == -1)
            {
                MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtener el ID del producto
            int productoId = App.DbService.ObtenerProductoIdPorTitulo(_producto.Titulo);
            if (productoId == -1)
            {
                MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            App.DbService.CrearReserva(usuarioId, productoId);
            MessageBox.Show($"Reserva iniciada para: {_producto.Titulo}\nPrecio: €{_producto.Precio * 0.30m:F2}", "Reserva", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void OnComprarClick(object sender, RoutedEventArgs e)
        {
            // Obtener el ID del usuario logueado
            int usuarioId = App.DbService.ObtenerUsuarioIdPorNombre(_mainWindow.CurrentUserName);
            if (usuarioId == -1)
            {
                MessageBox.Show("Usuario no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtener el ID del producto
            int productoId = App.DbService.ObtenerProductoIdPorTitulo(_producto.Titulo);
            if (productoId == -1)
            {
                MessageBox.Show("Producto no encontrado.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            App.DbService.CrearReserva(usuarioId, productoId); 
            MessageBox.Show($"Compra realizada: {_producto.Titulo}\nPrecio: €{_producto.Precio:F2}", "Compra", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}