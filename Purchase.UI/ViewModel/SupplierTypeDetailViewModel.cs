using Prism.Commands;
using Prism.Events;
using Purchase.Model;
using Purchase.UI.Data.Repositories;
using Purchase.UI.View.Services;
using Purchase.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Purchase.UI.ViewModel
{
    public class SupplierTypeDetailViewModel : DetailViewModelBase
    {
        private ISupplierTypeRepository _supplierTypeRepository;
        private SupplierTypeWrapper _selectecSupplierType;

        public SupplierTypeDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, ISupplierTypeRepository supplierTypeRepository) : base(eventAggregator, messageDialogService)
        {
            _supplierTypeRepository = supplierTypeRepository;
            Title = "Supplier types";
            SupplierTypes = new ObservableCollection<SupplierTypeWrapper>();

            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);
        }

        public async override Task LoadAsync(int id)
        {
            Id = id;

            foreach (var wrapper in SupplierTypes)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            SupplierTypes.Clear();

            var types = await _supplierTypeRepository.GetAllAsync();

            foreach (var model in types)
            {
                var wrapper = new SupplierTypeWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                SupplierTypes.Add(wrapper);
            }
        }

        private void Wrapper_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _supplierTypeRepository.HasChanges();
            }
            if (e.PropertyName == nameof(SupplierTypeWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<SupplierTypeWrapper> SupplierTypes { get; }


        public ICommand RemoveCommand { get; }
        public ICommand AddCommand { get; }

        public SupplierTypeWrapper SelectedSupplierType
        {
            get { return _selectecSupplierType; }

            set
            {
                _selectecSupplierType = value;
                OnpropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && SupplierTypes.All(t => !t.HasErrors);
        }

        protected async override void OnSaveExecute()
        {
            try
            {
                await _supplierTypeRepository.SaveAsync();
                HasChanges = _supplierTypeRepository.HasChanges();
                RaiseCollectionSavedEvent();
            }
            catch (Exception ex)
            {

                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }

                MessageDialogService.ShowInfoDialog("Error while saving the entities, " +
                    "the data will be reloaded. Details: " + ex.Message);
                await LoadAsync(Id);
            }
            
        }

        private void OnAddExecute()
        {
            var wrapper = new SupplierTypeWrapper(new SupplierType());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _supplierTypeRepository.Add(wrapper.Model);
            SupplierTypes.Add(wrapper);
            //Trigger validation
            wrapper.Type = "";
        }

        private async void OnRemoveExecute()
        {
            var isReferenced =
                await _supplierTypeRepository.IsReferencedBySupplierAsync(SelectedSupplierType.Id);
            if (isReferenced)
            {
                MessageDialogService.ShowInfoDialog($"The type {SelectedSupplierType.Type} can't be removed, as it is referenced by at least on supplier.");
                return;
            }

            SelectedSupplierType.PropertyChanged -= Wrapper_PropertyChanged;
            _supplierTypeRepository.Remove(SelectedSupplierType.Model);
            SupplierTypes.Remove(SelectedSupplierType);
            SelectedSupplierType = null;
            HasChanges = _supplierTypeRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }


        private bool OnRemoveCanExecute()
        {
            return SelectedSupplierType != null;
        }
    }
}
