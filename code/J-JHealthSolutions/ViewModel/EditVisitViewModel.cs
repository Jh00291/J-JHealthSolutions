using System.Collections.ObjectModel;
using System.ComponentModel;
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

            IsReadOnlyMode = _visit.VisitStatus == "Completed";

            Statuses = new ObservableCollection<string> { "Completed", "InProgress", "Pending" };

            TestOrders = new ObservableCollection<TestOrder>(TestOrderDal.GetTestOrdersFromVisit((int)visit.VisitId));

            // Initialize Check-Up Properties
            Weight = _visit.Weight;
            Height = _visit.Height;
            BloodPressureSystolic = _visit.BloodPressureSystolic;
            BloodPressureDiastolic = _visit.BloodPressureDiastolic;
            Temperature = _visit.Temperature;
            Pulse = _visit.Pulse;
            Symptoms = _visit.Symptoms;
            HeightString = Height?.ToString() ?? string.Empty;
            TemperatureString = Temperature?.ToString() ?? string.Empty;
            WeightString = Weight?.ToString() ?? string.Empty;


            // Initialize Commands
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            EditTestOrderCommand = new RelayCommand(EditTestOrder, CanEditOrDeleteTestOrder);
            DeleteTestOrderCommand = new RelayCommand(DeleteTestOrder, CanEditOrDeleteTestOrder);
            AddTestOrderCommand = new RelayCommand(AddTestOrder, CanSave);

            // Initialize Error Visibility Flags
            IsWeightErrorVisible = false;
            IsHeightErrorVisible = false;
            IsBPSystolicErrorVisible = false;
            IsBPDiastolicErrorVisible = false;
            IsTemperatureErrorVisible = false;
            IsPulseErrorVisible = false;
        }

        #region Main Visit Properties

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

        public bool IsStatusEnabled => !IsReadOnlyMode;
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

        private string _heightString;
        public string HeightString
        {
            get => _heightString;
            set
            {
                _heightString = value;
                OnPropertyChanged(nameof(HeightString));

                if (decimal.TryParse(value, out var parsedHeight))
                {
                    Height = parsedHeight;
                    IsHeightErrorVisible = false;
                }
                else
                {
                    IsHeightErrorVisible = !string.IsNullOrWhiteSpace(value);
                    HeightError = "Invalid height. Please enter a valid decimal number.";
                }
            }
        }

        private string _temperatureString;
        public string TemperatureString
        {
            get => _temperatureString;
            set
            {
                _temperatureString = value;
                OnPropertyChanged(nameof(TemperatureString));

                if (decimal.TryParse(value, out var parsedTemperature))
                {
                    Temperature = parsedTemperature;
                    IsTemperatureErrorVisible = false;
                }
                else
                {
                    IsTemperatureErrorVisible = !string.IsNullOrWhiteSpace(value);
                    TemperatureError = "Invalid temperature. Please enter a valid decimal number.";
                }
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
                (EditTestOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteTestOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Check-Up Properties

        private decimal? _weight;
        public decimal? Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        private string _weightError;
        public string WeightError
        {
            get => _weightError;
            set
            {
                _weightError = value;
                OnPropertyChanged(nameof(WeightError));
            }
        }

        private bool _isWeightErrorVisible;
        public bool IsWeightErrorVisible
        {
            get => _isWeightErrorVisible;
            set
            {
                _isWeightErrorVisible = value;
                OnPropertyChanged(nameof(IsWeightErrorVisible));
            }
        }

        private decimal? _height;
        public decimal? Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string _heightError;
        public string HeightError
        {
            get => _heightError;
            set
            {
                _heightError = value;
                OnPropertyChanged(nameof(HeightError));
            }
        }

        private bool _isHeightErrorVisible;
        public bool IsHeightErrorVisible
        {
            get => _isHeightErrorVisible;
            set
            {
                _isHeightErrorVisible = value;
                OnPropertyChanged(nameof(IsHeightErrorVisible));
            }
        }

        private string _weightString;
        public string WeightString
        {
            get => _weightString;
            set
            {
                _weightString = value;
                OnPropertyChanged(nameof(WeightString));

                if (decimal.TryParse(value, out var parsedWeight))
                {
                    Weight = parsedWeight;
                    IsWeightErrorVisible = false;
                }
                else
                {
                    IsWeightErrorVisible = !string.IsNullOrWhiteSpace(value);
                    WeightError = "Invalid weight. Please enter a valid decimal number.";
                }
            }
        }


        private int? _bloodPressureSystolic;
        public int? BloodPressureSystolic
        {
            get => _bloodPressureSystolic;
            set
            {
                _bloodPressureSystolic = value;
                OnPropertyChanged(nameof(BloodPressureSystolic));
            }
        }

        private string _bpsystolicError;
        public string BPSystolicError
        {
            get => _bpsystolicError;
            set
            {
                _bpsystolicError = value;
                OnPropertyChanged(nameof(BPSystolicError));
            }
        }

        private bool _isBPSystolicErrorVisible;
        public bool IsBPSystolicErrorVisible
        {
            get => _isBPSystolicErrorVisible;
            set
            {
                _isBPSystolicErrorVisible = value;
                OnPropertyChanged(nameof(IsBPSystolicErrorVisible));
            }
        }

        private int? _bloodPressureDiastolic;
        public int? BloodPressureDiastolic
        {
            get => _bloodPressureDiastolic;
            set
            {
                _bloodPressureDiastolic = value;
                OnPropertyChanged(nameof(BloodPressureDiastolic));
            }
        }

        private string _bpdiastolicError;
        public string BPDiastolicError
        {
            get => _bpdiastolicError;
            set
            {
                _bpdiastolicError = value;
                OnPropertyChanged(nameof(BPDiastolicError));
            }
        }

        private bool _isBPDiastolicErrorVisible;
        public bool IsBPDiastolicErrorVisible
        {
            get => _isBPDiastolicErrorVisible;
            set
            {
                _isBPDiastolicErrorVisible = value;
                OnPropertyChanged(nameof(IsBPDiastolicErrorVisible));
            }
        }

        private decimal? _temperature;
        public decimal? Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }

        private string _temperatureError;
        public string TemperatureError
        {
            get => _temperatureError;
            set
            {
                _temperatureError = value;
                OnPropertyChanged(nameof(TemperatureError));
            }
        }

        private bool _isTemperatureErrorVisible;
        public bool IsTemperatureErrorVisible
        {
            get => _isTemperatureErrorVisible;
            set
            {
                _isTemperatureErrorVisible = value;
                OnPropertyChanged(nameof(IsTemperatureErrorVisible));
            }
        }

        private int? _pulse;
        public int? Pulse
        {
            get => _pulse;
            set
            {
                _pulse = value;
                OnPropertyChanged(nameof(Pulse));
            }
        }

        private string _pulseError;
        public string PulseError
        {
            get => _pulseError;
            set
            {
                _pulseError = value;
                OnPropertyChanged(nameof(PulseError));
            }
        }

        private bool _isPulseErrorVisible;
        public bool IsPulseErrorVisible
        {
            get => _isPulseErrorVisible;
            set
            {
                _isPulseErrorVisible = value;
                OnPropertyChanged(nameof(IsPulseErrorVisible));
            }
        }

        private string _symptoms;
        public string Symptoms
        {
            get => _symptoms;
            set
            {
                _symptoms = value;
                OnPropertyChanged(nameof(Symptoms));
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

            if (SelectedStatus == "Completed")
            {
                if (!AreAllTestsPerformed())
                {
                    _dialogService.ShowMessage(
                        "Cannot close the visit. Not all tests have been performed.",
                        "Incomplete Tests");
                    return;
                }

                if (string.IsNullOrWhiteSpace(FinalDiagnosis))
                {
                    _dialogService.ShowMessage(
                        "Please enter a final diagnosis before closing the visit.",
                        "Final Diagnosis Required");
                    return;
                }

                bool confirmation = _dialogService.ShowConfirmation(
                    "Setting the status to 'Completed' will make the visit information permanent. Are you sure you want to save these changes?",
                    "Confirm Save");

                if (!confirmation)
                {
                    return;
                }
            }

            // Validate Check-Up Information
            if (!ValidateCheckUpInformation())
                return;

            try
            {
                _visit.InitialDiagnosis = this.InitialDiagnosis;
                _visit.FinalDiagnosis = this.FinalDiagnosis;
                _visit.VisitStatus = this.SelectedStatus;

                // Assign Check-Up Data
                _visit.Weight = this.Weight;
                _visit.Height = this.Height;
                _visit.BloodPressureSystolic = this.BloodPressureSystolic;
                _visit.BloodPressureDiastolic = this.BloodPressureDiastolic;
                _visit.Temperature = this.Temperature;
                _visit.Pulse = this.Pulse;
                _visit.Symptoms = this.Symptoms;

                VisitDal.UpdateVisit(_visit);

                if (SelectedStatus == "Completed")
                {
                    IsReadOnlyMode = true;
                    AppointmentDal.CompleteAppointment((int)_visit.VisitId);
                }

                VisitUpdated?.Invoke(this, EventArgs.Empty);

                CloseWindow();
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Error saving visit: {ex.Message}", "Error");
            }
        }

        private bool CanSave(object parameter)
        {
            return !IsReadOnlyMode;
        }

        private void Cancel(object parameter)
        {
            CloseWindow();
        }

        private bool AreAllTestsPerformed()
        {
            var allTestOrders = TestOrderDal.GetTestOrdersFromVisit((int)_visit.VisitId);
            return allTestOrders.All(to => to.PerformedDateTime.HasValue && !string.IsNullOrWhiteSpace(to.ResultWithUnit));
        }

        private void EditTestOrder(object parameter)
        {
            bool? dialogResult = new AddEditTestOrderWindow(SelectedTestOrder, _visit).ShowDialog();
            if (dialogResult == true)
            {
                TestOrders = new ObservableCollection<TestOrder>(TestOrderDal.GetTestOrdersFromVisit((int)_visit.VisitId));
                OnPropertyChanged(nameof(TestOrders));
            }
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
                        _dialogService.ShowMessage("Failed to delete the test order from the database.", "Error");
                    }
                }
                catch (Exception ex)
                {
                    _dialogService.ShowMessage($"Error deleting test order: {ex.Message}", "Error");
                }
            }
        }

        private void AddTestOrder(object parameter)
        {
            bool? dialogResult = new AddEditTestOrderWindow(_visit).ShowDialog();
            if (dialogResult == true)
            {
                TestOrders = new ObservableCollection<TestOrder>(TestOrderDal.GetTestOrdersFromVisit((int)_visit.VisitId));
                OnPropertyChanged(nameof(TestOrders));
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
                _dialogService.ShowMessage("Please select a status for the visit.", "Error");
                return false;
            }
            return true;
        }

        private bool ValidateCheckUpInformation()
        {
            bool hasError = false;

            // Validate Weight
            if (!Weight.HasValue)
            {
                WeightError = "Weight is required.";
                IsWeightErrorVisible = true;
                hasError = true;
            }
            else if (Weight <= 0)
            {
                WeightError = "Weight must be a positive number.";
                IsWeightErrorVisible = true;
                hasError = true;
            }
            else
            {
                IsWeightErrorVisible = false;
            }

            // Validate Height
            if (!Height.HasValue)
            {
                HeightError = "Height is required.";
                IsHeightErrorVisible = true;
                hasError = true;
            }
            else if (Height <= 0)
            {
                HeightError = "Height must be a positive number.";
                IsHeightErrorVisible = true;
                hasError = true;
            }
            else
            {
                IsHeightErrorVisible = false;
            }

            // Validate Blood Pressure Systolic
            if (!BloodPressureSystolic.HasValue)
            {
                BPSystolicError = "Systolic blood pressure is required.";
                IsBPSystolicErrorVisible = true;
                hasError = true;
            }
            else if (BloodPressureSystolic <= 0)
            {
                BPSystolicError = "Systolic blood pressure must be a positive number.";
                IsBPSystolicErrorVisible = true;
                hasError = true;
            }
            else
            {
                IsBPSystolicErrorVisible = false;
            }

            // Validate Blood Pressure Diastolic
            if (!BloodPressureDiastolic.HasValue)
            {
                BPDiastolicError = "Diastolic blood pressure is required.";
                IsBPDiastolicErrorVisible = true;
                hasError = true;
            }
            else if (BloodPressureDiastolic <= 0)
            {
                BPDiastolicError = "Diastolic blood pressure must be a positive number.";
                IsBPDiastolicErrorVisible = true;
                hasError = true;
            }
            else
            {
                IsBPDiastolicErrorVisible = false;
            }

            // Validate Temperature
            if (!Temperature.HasValue)
            {
                TemperatureError = "Temperature is required.";
                IsTemperatureErrorVisible = true;
                hasError = true;
            }
            else if (Temperature <= 0)
            {
                TemperatureError = "Temperature must be a positive number.";
                IsTemperatureErrorVisible = true;
                hasError = true;
            }
            else
            {
                IsTemperatureErrorVisible = false;
            }

            // Validate Pulse
            if (!Pulse.HasValue)
            {
                PulseError = "Pulse is required.";
                IsPulseErrorVisible = true;
                hasError = true;
            }
            else if (Pulse <= 0)
            {
                PulseError = "Pulse must be a positive number.";
                IsPulseErrorVisible = true;
                hasError = true;
            }
            else
            {
                IsPulseErrorVisible = false;
            }

            return !hasError;
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
