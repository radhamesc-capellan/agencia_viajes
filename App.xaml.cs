using System.Windows;

namespace Proyecto_Interfaces
{
    public partial class App : Application
    {
        public static DatabaseService DbService { get; } = new DatabaseService();
    }
}
