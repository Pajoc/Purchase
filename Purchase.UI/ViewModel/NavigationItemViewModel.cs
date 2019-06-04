using Prism.Commands;
using Prism.Events;
using Purchase.UI.Event;
using System;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase 
    {
        private string _displayMember;
        private IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            OpenFriendDetailViewCommand = new DelegateCommand(OnOpenFriendDetailView);

            _eventAggregator = eventAggregator;
        }

        public int Id { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnpropertyChanged();
            }
        }

        public ICommand OpenFriendDetailViewCommand { get; }

        private void OnOpenFriendDetailView()
        {
            _eventAggregator.GetEvent<OpenSupplierDtlViewEvent>().Publish(Id);
        }

    }
}
