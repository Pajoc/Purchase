using Autofac;
using Prism.Events;
using Purchase.DataAccess;
using Purchase.UI.Data;
using Purchase.UI.Data.Loockups;
using Purchase.UI.Data.Repositories;
using Purchase.UI.View.Services;
using Purchase.UI.ViewModel;

namespace Purchase.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<SupplierDetailViewModel>().As<ISupplierDetailViewModel>();

            builder.RegisterType<SupplierRepository>().As<ISupplierRepository>();

            builder.RegisterType<PurchaseDbContext>().AsSelf();

            //Vou ter mais que uma
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();

            

            return builder.Build();
        }
    }
}
