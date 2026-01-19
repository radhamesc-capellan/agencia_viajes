using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Proyecto_Interfaces
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "agencia.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
            CrearTablasSiNoExisten();
        }

        private void CrearTablasSiNoExisten()
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Usuarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NombreUsuario TEXT NOT NULL UNIQUE,
                    Contraseña TEXT NOT NULL,
                    Email TEXT
                );

                CREATE TABLE IF NOT EXISTS Productos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo TEXT NOT NULL,
                    Descripcion TEXT,
                    Tipo INTEGER NOT NULL, -- 0 = Viaje, 1 = Crucero, 2 = Tren, 3 = Eurotrip
                    Precio REAL NOT NULL,
                    ImagenPrincipal TEXT, -- Ruta de la imagen
                    Destacado BOOLEAN DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS Reservas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    UsuarioId INTEGER NOT NULL,
                    ProductoId INTEGER NOT NULL,
                    FechaReserva DATETIME DEFAULT CURRENT_TIMESTAMP,
                    Estado TEXT DEFAULT 'activa', -- 'activa', 'cancelada'
                    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id),
                    FOREIGN KEY (ProductoId) REFERENCES Productos(Id)
                );
            ";

            using var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();

            // Añadir productos iniciales si no existen
            string checkSql = "SELECT COUNT(1) FROM Productos";
            using var checkCommand = new SQLiteCommand(checkSql, connection);
            int count = Convert.ToInt32(checkCommand.ExecuteScalar());

            if (count == 0)
            {
                string insertSql = @"
                    INSERT INTO Productos (Titulo, Descripcion, Tipo, Precio, ImagenPrincipal) VALUES
                    ('Crucero Mediterráneo Premium', '7 días navegando por las costas más bellas del Mediterráneo...', 1, 1200, '/Assets/Cruceros/crucero.jpg'),
                    ('Ruta por el Sur de España', '8 días explorando Sevilla, Córdoba y Granada...', 0, 600, '/Assets/Viajes/viaje.jpg'),
                    ('Tren AVE Madrid - Barcelona', 'Viaje rápido y cómodo entre las dos ciudades más importantes de España...', 2, 80, '/Assets/Trenes/ave.jpg');
                ";
                using var insertCommand = new SQLiteCommand(insertSql, connection);
                insertCommand.ExecuteNonQuery();
            }
        }

        // Método para registrar un usuario
        public bool RegistrarUsuario(string nombre, string contraseña, string email)
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                var hashedPass = HashPassword(contraseña);

                string sql = "INSERT INTO Usuarios (NombreUsuario, Contraseña, Email) VALUES (@nombre, @pass, @email)";
                using var command = new SQLiteCommand(sql, connection);
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@pass", hashedPass);
                command.Parameters.AddWithValue("@email", email);
                command.ExecuteNonQuery();


                MessageBox.Show("Usuario registrado correctamente con el nombre :" + @nombre);
                return true;
            }
            catch (SQLiteException ex)
            {
                // Usuario ya existe
                MessageBox.Show("Usuario ya existe con el nombre :" + @nombre);
                return false;
                
            }
        }

        // Método para login
        public bool Login(string nombre, string contraseña)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var hashedPass = HashPassword(contraseña);

            string sql = "SELECT COUNT(1) FROM Usuarios WHERE NombreUsuario = @nombre AND Contraseña = @pass";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", nombre);
            command.Parameters.AddWithValue("@pass", hashedPass);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }

        // Método para crear una reserva
        public void CrearReserva(int usuarioId, int productoId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = "INSERT INTO Reservas (UsuarioId, ProductoId) VALUES (@uid, @pid)";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@uid", usuarioId);
            command.Parameters.AddWithValue("@pid", productoId);
            command.ExecuteNonQuery();
        }

        // Método para obtener las reservas de un usuario
        public List<ReservaConProducto> ObtenerReservasDeUsuario(int usuarioId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = @"
                SELECT r.Id, r.FechaReserva, r.Estado, p.Titulo, p.Descripcion, p.Precio, p.Tipo, p.ImagenPrincipal
                FROM Reservas r
                JOIN Productos p ON r.ProductoId = p.Id
                WHERE r.UsuarioId = @uid AND r.Estado = 'activa'";

            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@uid", usuarioId);

            using var reader = command.ExecuteReader();
            var reservas = new List<ReservaConProducto>();

            while (reader.Read())
            {
                reservas.Add(new ReservaConProducto
                {
                    Reserva = new Reserva
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                        Estado = reader["Estado"].ToString()
                    },
                    Producto = new DetalleProductoView.Producto
                    {
                        Titulo = reader["Titulo"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        ImagenPrincipal = reader["ImagenPrincipal"].ToString()
                    }
                });
            }

            return reservas;
        }

        // Método para cancelar una reserva
        public void CancelarReserva(int reservaId)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = "UPDATE Reservas SET Estado = 'cancelada' WHERE Id = @id";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@id", reservaId);
            command.ExecuteNonQuery();
        }

        // Método para obtener el ID del usuario por nombre
        public int ObtenerUsuarioIdPorNombre(string nombre)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = "SELECT Id FROM Usuarios WHERE NombreUsuario = @nombre";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@nombre", nombre);

            var result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // Método para obtener el ID del producto por título
        public int ObtenerProductoIdPorTitulo(string titulo)
        {
            using var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            string sql = "SELECT Id FROM Productos WHERE Titulo = @titulo";
            using var command = new SQLiteCommand(sql, connection);
            command.Parameters.AddWithValue("@titulo", titulo);

            var result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }

        // Método para encriptar la contraseña
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    // Clase para combinar reserva y producto
    public class ReservaConProducto
    {
        public Reserva Reserva { get; set; } = new();
        public DetalleProductoView.Producto Producto { get; set; } = new();
    }

    public class Reserva
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public DateTime FechaReserva { get; set; }
        public string Estado { get; set; } = string.Empty;
    }
}