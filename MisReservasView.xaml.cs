using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_Interfaces
{
    public partial class MisReservasView : UserControl
    {
        public MisReservasView()
        {
            InitializeComponent();
            CargarReservas();
        }

        private void CargarReservas()
        {
            // Aquí debes obtener el ID del usuario logueado
            // Por ahora, usaremos un ID fijo como ejemplo
            int usuarioId = 1; // Cambia esto por el ID real del usuario logueado

            var reservas = App.DbService.ObtenerReservasDeUsuario(usuarioId);
            ReservasList.ItemsSource = reservas;
        }

        private void OnCancelarReservaClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var reservaId = (int)button.Tag;

            if (MessageBox.Show("¿Estás seguro de que deseas cancelar esta reserva?", "Cancelar Reserva", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                App.DbService.CancelarReserva(reservaId);
                CargarReservas(); // Refrescar la lista
                MessageBox.Show("Reserva cancelada exitosamente.", "Cancelar Reserva", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}