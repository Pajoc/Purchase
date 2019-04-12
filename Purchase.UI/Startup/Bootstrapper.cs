using Autofac;
using Purchase.DataAccess;
using Purchase.UI.Data;
using Purchase.UI.ViewModel;

namespace Purchase.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<SupplierDetailViewModel>().As<ISupplierDetailViewModel>();

            builder.RegisterType<SupplierDataService>().As<ISupplierDataService>();

            builder.RegisterType<PurchaseDbContext>().AsSelf();

            //Vou ter mais que uma
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            

            return builder.Build();
        }
    }
}
