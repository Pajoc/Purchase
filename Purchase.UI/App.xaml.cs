using Autofac;
using Purchase.UI.Data;
using Purchase.UI.Startup;
using Purchase.UI.ViewModel;
using System.Windows;

namespace Purchase.UI
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            // var mainWindow = new MainWindow(new MainViewModel(new SupplierDataService()));

            var mainWindow = container.Resolve<MainWindow>();

            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

        }
    }
}
