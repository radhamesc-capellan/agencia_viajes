using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_Interfaces.Views
{
    public partial class ViajesView : UserControl
    {
        public ViajesView()
        {
            InitializeComponent();
        }

        private void OnViewDetailsClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var producto = new DetalleProductoView.Producto
            {
                Titulo = "Ruta por el Sur de España",
                Descripcion = "8 días explorando Sevilla, Córdoba y Granada. Incluye guía turístico, alojamiento y entradas a monumentos.",
                Precio = 600m,
                ImagenPrincipal = "/Assets/Viajes/viaje.jpg",
                Galeria = new List<string> {
                    "/Assets/Viajes/viaje.jpg",
                    "/Assets/Viajes/viaje1.jpg",
                    "/Assets/Viajes/viaje2.jpg",
                    "/Assets/Viajes/viaje3.jpg",
                    "/Assets/Viajes/viaje4.jpg"
                }
            };

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.MostrarDetalleProducto(producto);
        }

        private void OnReserveClick(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show("¡Reserva de viaje iniciada!", "Viajes", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OnCardClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var producto = new DetalleProductoView.Producto
            {
                Titulo = "Andalucía Completa",
                Descripcion = "8 días recorriendo Sevilla, Córdoba, Granada y Ronda. Incluye visitas guiadas y alojamiento en hoteles de 4 estrellas.",
                Precio = 700m,
                ImagenPrincipal = "/Assets/Viajes/viaje1.jpg",
                Galeria = new List<string> {
                    "/Assets/Viajes/viaje1.jpg",
                    "/Assets/Viajes/viaje2.jpg",
                    "/Assets/Viajes/viaje3.jpg",
                    "/Assets/Viajes/viaje4.jpg",
                    "/Assets/Viajes/viaje5.jpg"
                }
            };

            var mainWindow = Window.GetWindow(this) as MainWindow;
            mainWindow?.MostrarDetalleProducto(producto);
        }
    }
}