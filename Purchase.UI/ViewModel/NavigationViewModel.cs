using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data;
using Purchase.UI.Data.Loockups;
using Purchase.UI.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private ILookupSupplierDataService _supplierLookUpDServ;
        private IMeetingLookupDataService _meetingLookupService;
        private IEventAggregator _eventAggregator;

        public NavigationViewModel(ILookupSupplierDataService supplierLookUpDServ,IMeetingLookupDataService meetingLookupService, IEventAggregator eventAggregator)
        {
            _supplierLookUpDServ = supplierLookUpDServ;
            _meetingLookupService = meetingLookupService;
            _eventAggregator = eventAggregator;
            Suppliers = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookUp = await _supplierLookUpDServ.GetSupplierLookupAsync();
            Suppliers.Clear();
            foreach (var supplier in lookUp)
            {
                //Suppliers.Add(supplier);
                Suppliers.Add(new NavigationItemViewModel(supplier.Id,supplier.DisplayMember,nameof(SupplierDetailViewModel), _eventAggregator));
            }

            lookUp = await _meetingLookupService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var meeting in lookUp)
            {
                Meetings.Add(new NavigationItemViewModel(meeting.Id, meeting.DisplayMember, nameof(MeetingDetailViewModel), _eventAggregator));
            }
        }




        public ObservableCollection<NavigationItemViewModel> Suppliers { get; }

        public ObservableCollection<NavigationItemViewModel> Meetings { get; }


        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {

            switch (args.ViewModelName)
            {
                case nameof(SupplierDetailViewModel):
                    AfterDetailDeleted(Suppliers, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }

        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(s => s.Id == args.Id);

            if (item != null)
            {
                Suppliers.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {

            switch (obj.ViewModelName)
            {
                case nameof(SupplierDetailViewModel):
                    AfterDetailSaved(Suppliers, obj);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, obj);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(l => l.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, args.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }
}
