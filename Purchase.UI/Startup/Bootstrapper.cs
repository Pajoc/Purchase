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

            builder.RegisterType<PurchaseDbContext>().AsSelf();

            builder.RegisterType<MainWindow>().AsSelf();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            //builder.RegisterType<SupplierDetailViewModel>().As<ISupplierDetailViewModel>();
            builder.RegisterType<SupplierDetailViewModel>().Keyed<IDetailViewModel>(nameof(SupplierDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<SupplierTypeDetailViewModel>().Keyed<IDetailViewModel>(nameof(SupplierTypeDetailViewModel));

            //Vou ter mais que uma
            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<SupplierRepository>().As<ISupplierRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<SupplierTypeRepository>().As<ISupplierTypeRepository>();


            return builder.Build();
        }
    }
}
