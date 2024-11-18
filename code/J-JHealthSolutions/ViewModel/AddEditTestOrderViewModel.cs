using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.ViewModel
{
    public class AddEditTestOrderViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private IEnumerable<Test> _tests;
        private Test _selectedTest;
        private DateTime _testDate;
        private string _result;
        private UnitOfMeasure _unit;
        private bool _isSaveAttempted;
        private bool _isEditAttempt;
        private Visit _currentVisit;
        private string _abnormal;
        private Doctor _visitDoctor;
        private readonly IDialogService _dialogService;
        private TestOrder _existingTestOrder;
        public string TestOrderedBy { get; set; }

        public AddEditTestOrderViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            Initialize();
        }

        public AddEditTestOrderViewModel(Visit currentVisit, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _currentVisit = currentVisit;
            TestOrderedBy = DoctorDal.GetDoctor(_currentVisit.DoctorId).ToString();
            Initialize();
        }

        public AddEditTestOrderViewModel(TestOrder testOrder, Visit currentVisit, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _existingTestOrder = testOrder;
            _currentVisit = currentVisit;
            TestOrderedBy = DoctorDal.GetDoctor(_currentVisit.DoctorId).ToString();
            _isEditAttempt = true;
            Initialize();
            LoadExistingTestOrder();
        }

        private void Initialize()
        {
            TestDate = DateTime.Now;
            SaveCommand = new RelayCommand(param => SaveTestOrder(), param => CanSaveTestOrder());
            CancelCommand = new RelayCommand(param => Cancel(), param => true);
            Tests = new ObservableCollection<Test>(TestDal.GetTests());
        }

        private void SaveTestOrder()
        {
            if (Save())
            {
                OnTestOrderSaved?.Invoke(this, EventArgs.Empty);
            }
        }


        private bool CanSaveTestOrder()
        {
            return true; 
        }

        private void LoadExistingTestOrder()
        {
            if (_existingTestOrder != null)
            {
                SelectedTest = Tests.FirstOrDefault(t => t.TestCode == _existingTestOrder.TestCode);
                TestPerformedDate = _existingTestOrder.PerformedDateTime;
                Result = _existingTestOrder.Result?.ToString();
                Abnormal = _existingTestOrder.Abnormal.HasValue ? (_existingTestOrder.Abnormal.Value ? "Yes" : "No") : null;
            }
        }

        public DateTime TestDate { get; internal set; }

        public IEnumerable<Test> Tests
        {
            get => _tests;
            set
            {
                _tests = value;
                OnPropertyChanged(nameof(Tests));
            }
        }

        public Test SelectedTest
        {
            get => _selectedTest;
            set
            {
                _selectedTest = value;
                OnPropertyChanged(nameof(SelectedTest));
                OnPropertyChanged(nameof(IsAbnormalVisible));

                Unit = (UnitOfMeasure)_selectedTest?.Unit;
            }
        }

        public ICommand SaveCommand { get; internal set; }
        public ICommand CancelCommand { get; internal set; }
        public event EventHandler OnCancelRequested;
        public event EventHandler OnTestOrderSaved;

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
                OnPropertyChanged(nameof(IsAbnormalVisible));
            }
        }

        private void Cancel()
        {
            OnCancelRequested?.Invoke(this, EventArgs.Empty);
        }

        private DateTime? _testPerformedDate;
        public DateTime? TestPerformedDate
        {
            get => _testPerformedDate;
            set
            {
                if (value != null)
                {
                    if (value.Value.Date < DateTime.Today)
                    {
                        throw new ArgumentException("Test Performed Date cannot be in the past.");
                    }
                    if (value.Value.Date > DateTime.Today)
                    {
                        throw new ArgumentException("Test Performed Date cannot be in the future.");
                    }
                }

                _testPerformedDate = value;
                OnPropertyChanged(nameof(TestPerformedDate));
                OnPropertyChanged(nameof(IsResultEnabled));
            }
        }

        public DateTime MinTestPerformedDate => DateTime.Today;
        public DateTime MaxTestPerformedDate => DateTime.Today;

        public UnitOfMeasure Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                OnPropertyChanged(nameof(Unit));
            }
        }

        public bool IsResultEnabled => TestPerformedDate != null;

        public bool IsSaveAttempted
        {
            get => _isSaveAttempted;
            set
            {
                _isSaveAttempted = value;
                OnPropertyChanged(nameof(IsSaveAttempted));
            }
        }

        public string Abnormal
        {
            get => _abnormal;
            set
            {
                _abnormal = value;
                OnPropertyChanged(nameof(Abnormal));
            }
        }

        private static readonly List<string> _abnormalOptions = new List<string> { "Yes", "No" };

        public IEnumerable<string> AbnormalOptions => _abnormalOptions;

        public bool IsAbnormalVisible
        {
            get
            {
                if (SelectedTest != null && !string.IsNullOrWhiteSpace(Result))
                {
                    bool lowOrHighValueIsNull = SelectedTest.LowValue == null || SelectedTest.HighValue == null;
                    return lowOrHighValueIsNull;
                }
                return false;
            }
        }

        public IEnumerable<UnitOfMeasure> UnitsOfMeasures { get; } = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>();

        public string this[string columnName]
        {
            get
            {
                if (!IsSaveAttempted)
                    return string.Empty;

                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(SelectedTest):
                        if (SelectedTest == null)
                            error = "Test is required.";
                        break;
                    case nameof(Result):

                        if (IsResultEnabled && string.IsNullOrWhiteSpace(Result))
                        {
                            error = "Result is required when Test Performed Date is selected.";
                                
                        } else if (IsResultEnabled && !double.TryParse(Result, out _))
                        {
                            error = "Result must be a valid number.";
                        }
                        break;
                    case nameof(Abnormal):
                        if (IsAbnormalVisible && string.IsNullOrEmpty(Abnormal))
                        {
                            error = "Please select if the result is abnormal.";
                        }
                        break;
                }
                return error;
            }
        }

        public string Error => null;

        public bool Save()
        {

            IsSaveAttempted = true;

            OnPropertyChanged(nameof(SelectedTest));
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(Abnormal));

            if (!HasErrors())
            {
                if (_isEditAttempt)
                {
                    return UpdateTestOrder();
                }
                else
                {
                    return AddTestOrder();
                }
            }

            return false; 
        }

        private bool AddTestOrder()
        {
            if (TestTypeExistsForVisit())
            {
                if (ConfirmToAddDuplicateTestType())
                {
                    TestOrderDal.CreateTestOrder(CreateTestOrder());
                    return true;
                }
            }
            else
            {
                TestOrderDal.CreateTestOrder(CreateTestOrder());
                return true;
            }

            return false;
        }

        private bool UpdateTestOrder()
        {
            return TestOrderDal.UpdateTestOrder(CreateTestOrder());
        }

        private bool ConfirmToAddDuplicateTestType()
        {
            var existingTestOrder = TestOrderDal.GetTestOrderByVisitAndTestCode((int)_currentVisit.VisitId, SelectedTest.TestCode);

            string message = $"A test of this type has already been ordered for this visit:\n" +
                             $"Test: {existingTestOrder.Test.TestName}\n" +
                             $"Ordered Date: {existingTestOrder.OrderDateTime}\n" +
                             $"Performed Date: {existingTestOrder.PerformedDateTime}\n" +
                             $"Result: {existingTestOrder.Result}\n" +
                             $"Abnormal: {(existingTestOrder.Abnormal == true ? "Yes" : "No")}\n\n" +
                             "Do you want to add another test of this type?";

            return _dialogService.ShowConfirmationDialog("Confirm Add Test", message);
        }

        private bool TestTypeExistsForVisit()
        {
            if (_currentVisit == null)
            {
                return false;
            }
            return TestOrderDal.TestTypeExistsForVisit((int)_currentVisit.VisitId, SelectedTest.TestCode);
        }

        private TestOrder CreateTestOrder()
        {
            return CreateTestOrder(null);
        }

        private TestOrder CreateTestOrder(int? testOrderID)
        {
            double? resultValue = null;
            bool? abnormalValue = null;

            if (!string.IsNullOrWhiteSpace(Result))
            {
                if (double.TryParse(Result, out double parsedResult))
                {
                    resultValue = parsedResult;
                    abnormalValue = Abnormal == "Yes";
                }
                else
                {
                    throw new InvalidOperationException("Invalid Result value. Please enter a valid number.");
                }
            }
            else
            {
                abnormalValue = null;
            }

            return new TestOrder
            {
                TestOrderID = testOrderID,
                VisitId = (int)_currentVisit.VisitId,
                TestCode = SelectedTest.TestCode,
                OrderDateTime = DateTime.Now,
                PerformedDateTime = TestPerformedDate,
                Result = resultValue,
                Abnormal = abnormalValue
            };
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool HasErrors()
        {
            var result = ValidateSelectedTest();
            if (IsResultEnabled)
            {
                result &= ValidateResult();
                result &= ValidateAbnormal();
            }
            return !result;
        }

        private bool ValidateSelectedTest()
        {
            return SelectedTest != null;
        }

        private bool ValidateResult()
        {
            return !string.IsNullOrWhiteSpace(Result) && double.TryParse(Result, out _);
        }

        private bool ValidateAbnormal()
        {
            if (IsAbnormalVisible)
            {
                return !string.IsNullOrEmpty(Abnormal);
            }
            return true;
        }
    }
}
