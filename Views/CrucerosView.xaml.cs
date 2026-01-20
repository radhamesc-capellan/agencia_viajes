using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Proyecto_Interfaces;

namespace Proyecto_Interfaces.Views
{
    public partial class CrucerosView : UserControl
    {
        public CrucerosView()
        {
            InitializeComponent();
        }

        private void OnViewDetailsClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var producto = new DetalleProductoView.Producto
            {
                Titulo = "Crucero Mediterráneo Premium",
                Descripcion = "Disfruta de 7 días navegando por las costas más bellas del Mediterráneo. Incluye todas las comidas, bebidas y actividades a bordo.",
                Precio = 1200m,
                ImagenPrincipal = "/Assets/Cruceros/crucero.jpg",
                Galeria = new List<string> {
                    "/Assets/Cruceros/crucero.jpg",
                    "/Assets/Cruceros/crucero1.jpg",
                    "/Assets/Cruceros/crucero2.jpg",
                    "/Assets/Cruceros/crucero3.jpg",
                    "/Assets/Cruceros/crucero4.jpg"
                }
            };

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.MostrarDetalleProducto(producto);
        }

        private void OnReserveClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("¡Reserva iniciada!", "Cruceros", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnCardClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var producto = new DetalleProductoView.Producto
            {
                Titulo = "Crucero Caribe Exótico",
                Descripcion = "10 días explorando las islas más hermosas del Caribe. Incluye excursiones a playas paradisíacas y snorkel en arrecifes de coral.",
                Precio = 1500m,
                ImagenPrincipal = "/Assets/Cruceros/crucero1.jpg",
                Galeria = new List<string> {
                    "/Assets/Cruceros/crucero1.jpg",
                    "/Assets/Cruceros/crucero2.jpg",
                    "/Assets/Cruceros/crucero3.jpg",
                    "/Assets/Cruceros/crucero4.jpg",
                    "/Assets/Cruceros/crucero5.jpg"
                }
            };

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.MostrarDetalleProducto(producto);
        }
    }
}