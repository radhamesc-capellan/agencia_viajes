using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Proyecto_Interfaces
{
    public partial class TrenesView : UserControl
    {
        // Datos simulados de precios y destinos
        private readonly Dictionary<string, Dictionary<string, decimal>> _trenesYPrecios = new()
        {
            { "AVE", new Dictionary<string, decimal> {
                { "Madrid - Barcelona", 120m },
                { "Madrid - Sevilla", 90m },
                { "Barcelona - Valencia", 85m }
            }},
            { "Alvia", new Dictionary<string, decimal> {
                { "Madrid - León", 60m },
                { "Barcelona - Tarragona", 40m },
                { "Sevilla - Córdoba", 30m }
            }},
            { "Renfe", new Dictionary<string, decimal> {
                { "Madrid - Guadalajara", 15m },
                { "Barcelona - Lleida", 20m },
                { "Valencia - Castellón", 18m }
            }},
            { "Media Distancia", new Dictionary<string, decimal> {
                { "Bilbao - San Sebastián", 25m },
                { "Málaga - Granada", 22m },
                { "Zaragoza - Huesca", 18m }
            }}
        };

        public TrenesView()
        {
            InitializeComponent();
            Loaded += OnTrenesViewLoaded;
        }

        private void OnTrenesViewLoaded(object sender, RoutedEventArgs e)
        {
            ActualizarDestinos();
            ActualizarPrecio();
        }

        private void OnTipoTrenChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarDestinos();
            ActualizarPrecio();
        }

        private void OnDestinoChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarPrecio();
        }

        private void ActualizarDestinos()
        {
            if (TipoTrenComboBox?.SelectedItem is ComboBoxItem tipoSeleccionado)
            {
                var tipo = tipoSeleccionado.Content.ToString();

                if (DestinoComboBox != null && _trenesYPrecios.ContainsKey(tipo))
                {
                    DestinoComboBox.Items.Clear();

                    foreach (var destino in _trenesYPrecios[tipo])
                    {
                        DestinoComboBox.Items.Add(new ComboBoxItem { Content = destino.Key });
                    }

                    DestinoComboBox.SelectedIndex = 0;
                }
            }
        }

        private void ActualizarPrecio()
        {
            if (TipoTrenComboBox?.SelectedItem is ComboBoxItem tipoSeleccionado &&
                DestinoComboBox?.SelectedItem is ComboBoxItem destinoSeleccionado &&
                PrecioLabel != null && PrecioReservaLabel != null)
            {
                var tipo = tipoSeleccionado.Content.ToString();
                var destino = destinoSeleccionado.Content.ToString();

                if (_trenesYPrecios.ContainsKey(tipo) && _trenesYPrecios[tipo].ContainsKey(destino))
                {
                    var precioTotal = _trenesYPrecios[tipo][destino];
                    var precioReserva = precioTotal * 0.30m;

                    PrecioLabel.Text = $"Precio total: €{precioTotal:F2}";
                    PrecioReservaLabel.Text = $"Reserva (30%): €{precioReserva:F2}";
                }
            }
        }

        private void OnReservarClick(object sender, RoutedEventArgs e)
        {
            if (TipoTrenComboBox?.SelectedItem is ComboBoxItem tipoSeleccionado &&
                DestinoComboBox?.SelectedItem is ComboBoxItem destinoSeleccionado &&
                FechaDatePicker.SelectedDate.HasValue)
            {
                var tipo = tipoSeleccionado.Content.ToString();
                var destino = destinoSeleccionado.Content.ToString();
                var fecha = FechaDatePicker.SelectedDate.Value.ToString("dd/MM/yyyy");
                var hora = (HoraTrenComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var precioReserva = decimal.Parse(PrecioReservaLabel.Text.Split('€')[1].TrimEnd(')'));

                MessageBox.Show($"Reserva realizada:\nTren: {tipo}\nDestino: {destino}\nFecha: {fecha}\nHora: {hora}\nPrecio de reserva: €{precioReserva:F2}", "Reserva", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un tipo de tren, destino, fecha y hora.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OnComprarClick(object sender, RoutedEventArgs e)
        {
            if (TipoTrenComboBox?.SelectedItem is ComboBoxItem tipoSeleccionado &&
                DestinoComboBox?.SelectedItem is ComboBoxItem destinoSeleccionado &&
                FechaDatePicker.SelectedDate.HasValue)
            {
                var tipo = tipoSeleccionado.Content.ToString();
                var destino = destinoSeleccionado.Content.ToString();
                var fecha = FechaDatePicker.SelectedDate.Value.ToString("dd/MM/yyyy");
                var hora = (HoraTrenComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var precioTotal = decimal.Parse(PrecioLabel.Text.Split('€')[1].TrimEnd(')'));

                MessageBox.Show($"Compra realizada:\nTren: {tipo}\nDestino: {destino}\nFecha: {fecha}\nHora: {hora}\nPrecio total: €{precioTotal:F2}", "Compra", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un tipo de tren, destino, fecha y hora.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}