using System.Windows;

using Proyecto_Interfaces.Services;
using Proyecto_Interfaces.Views;

namespace Proyecto_Interfaces
{
    public partial class App : Application
    {
        public static DatabaseService DbService { get; } = new DatabaseService();
    }
}
