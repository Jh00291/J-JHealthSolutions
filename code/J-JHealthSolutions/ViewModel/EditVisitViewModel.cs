using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.Views;

namespace J_JHealthSolutions.ViewModels
{
    public class EditVisitViewModel : INotifyPropertyChanged
    {
        private Visit _visit;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IDialogService _dialogService;

        public EditVisitViewModel(Visit visit, IDialogService dialogService)
        {
            _visit = visit ?? throw new ArgumentNullException(nameof(visit));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            InitialDiagnosis = _visit.InitialDiagnosis;
            FinalDiagnosis = _visit.FinalDiagnosis;
            SelectedStatus = _visit.VisitStatus;

            IsReadOnlyMode = _visit.VisitStatus == "Completed" || !string.IsNullOrWhiteSpace(_visit.FinalDiagnosis);

            Statuses = new ObservableCollection<string> { "Completed", "InProgress", "Pending" };

            TestOrders = new ObservableCollection<TestOrder>(TestOrderDal.GetTestOrdersFromVisit((int)visit.VisitId));

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            EditTestOrderCommand = new RelayCommand(EditTestOrder, CanEditOrDeleteTestOrder);
            DeleteTestOrderCommand = new RelayCommand(DeleteTestOrder, CanEditOrDeleteTestOrder);
            AddTestOrderCommand = new RelayCommand(AddTestOrder, CanSave);
            _dialogService=dialogService;
        }

        #region Properties

        private string _initialDiagnosis;
        public string InitialDiagnosis
        {
            get => _initialDiagnosis;
            set
            {
                _initialDiagnosis = value;
                OnPropertyChanged(nameof(InitialDiagnosis));
            }
        }

        public bool IsStatusEnabled => !_isReadOnlyMode;
        private bool _isReadOnlyMode;
        public bool IsReadOnlyMode
        {
            get => _isReadOnlyMode;
            set
            {
                _isReadOnlyMode = value;
                OnPropertyChanged(nameof(IsReadOnlyMode));
                OnPropertyChanged(nameof(IsStatusEnabled));
            }
        }

        private string _finalDiagnosis;
        public string FinalDiagnosis
        {
            get => _finalDiagnosis;
            set
            {
                if (_finalDiagnosis != value)
                {
                    _finalDiagnosis = value;
                    OnPropertyChanged(nameof(FinalDiagnosis));
                }
            }
        }

        public ObservableCollection<string> Statuses { get; set; }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged(nameof(SelectedStatus));
                }
            }
        }

        public ObservableCollection<TestOrder> TestOrders { get; set; }

        private TestOrder _selectedTestOrder;
        public TestOrder SelectedTestOrder
        {
            get => _selectedTestOrder;
            set
            {
                _selectedTestOrder = value;
                OnPropertyChanged(nameof(SelectedTestOrder));
                // Notify that CanExecute for commands might have changed
                (EditTestOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteTestOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditTestOrderCommand { get; }
        public ICommand DeleteTestOrderCommand { get; }
        public ICommand AddTestOrderCommand { get; }

        #endregion

        #region Command Methods

        public event EventHandler VisitUpdated;
        private void Save(object parameter)
        {
            if (!ValidateStatus())
                return;

            if (SelectedStatus == "Completed" || !string.IsNullOrWhiteSpace(FinalDiagnosis))
            {
                bool confirmation = _dialogService.ShowConfirmation(
                    "Setting the status to 'Completed' or providing a final diagnosis will make the visit information permanent. Are you sure you want to save these changes?",
                    "Confirm Save");

                if (!confirmation)
                {
                    return;
                }
            }

            try
            {
                var visitDal = new VisitDal();
                _visit.InitialDiagnosis = this.InitialDiagnosis;
                _visit.FinalDiagnosis = this.FinalDiagnosis;
                _visit.VisitStatus = this.SelectedStatus;
                visitDal.UpdateVisit(_visit);

                if (!string.IsNullOrWhiteSpace(FinalDiagnosis))
                {
                    IsReadOnlyMode = true;
                }

                VisitUpdated?.Invoke(this, EventArgs.Empty);

                CloseWindow();
            }
            catch (Exception ex)
            {
                _dialogService.ShowConfirmation($"Error saving visit: {ex.Message}", "Error");
            }
        }

        private bool CanSave(object parameter)
        {
            return !IsReadOnlyMode && !string.IsNullOrEmpty(SelectedStatus);
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }

        private void EditTestOrder(object parameter)
        {
            bool? dialogResult = new AddEditTestOrder(SelectedTestOrder, _visit).ShowDialog();
        }

        private void DeleteTestOrder(object parameter)
        {
            if (SelectedTestOrder == null)
                return;

            bool confirmation = _dialogService.ShowConfirmation(
                "Are you sure you want to delete the selected test order?",
                "Confirm Delete");

            if (confirmation)
            {
                try
                {
                    bool deleteResult = TestOrderDal.DeleteTestOrder((int)SelectedTestOrder.TestOrderID);
                    if (deleteResult)
                    {
                        TestOrders.Remove(SelectedTestOrder);
                        SelectedTestOrder = null;
                    }
                    else
                    {
                        _dialogService.ShowConfirmation("Failed to delete the test order from the database.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowConfirmation($"Error deleting test order: {ex.Message}", "Error");
                }
            }
        }

        private void AddTestOrder(object parameter)
        {
           bool? dialogResult = new AddEditTestOrder(_visit).ShowDialog();
           if (dialogResult == true)
           {
               TestOrders = new ObservableCollection<TestOrder>(TestOrderDal.GetTestOrdersFromVisit((int)_visit.VisitId));
           }
        }


        private bool CanEditOrDeleteTestOrder(object parameter)
        {
            return SelectedTestOrder != null && SelectedTestOrder.PerformedDateTime == null;
        }

        #endregion

        #region Helper Methods

        private bool ValidateStatus()
        {
            if (string.IsNullOrEmpty(SelectedStatus))
            {
                _dialogService.ShowConfirmation("Please select a status for the visit.", "Error");
                return false;
            }
            return true;
        }

        private void CloseWindow()
        {
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Event to Request Window Close

        public event EventHandler RequestClose;

        #endregion
    }
}
