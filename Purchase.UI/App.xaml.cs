using Purchase.UI.Data;
using Purchase.UI.ViewModel;
using System.Windows;

namespace Purchase.UI
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindows = new MainWindow(new MainViewModel(new SupplierDataService()));
            mainWindows.Show();
        }
    }
}
