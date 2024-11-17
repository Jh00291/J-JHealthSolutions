using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public AddEditTestOrderViewModel()
        {
            // Initialize the Tests collection (replace with your actual data retrieval logic)
            Tests = GetAvailableTests();

            TestDate = DateTime.Now;
        }

        public DateTime TestDate { get; }

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

                // Update the Unit based on the selected test
                Unit = (UnitOfMeasure)_selectedTest?.Unit;

                // Optionally, update other properties related to the selected test
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
            }
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
                        // Cannot be in the past
                        throw new ArgumentException("Test Performed Date cannot be in the past.");
                    }
                    if (value.Value.Date > DateTime.Today)
                    {
                        // Cannot be in the future
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

        public IEnumerable<UnitOfMeasure> UnitsOfMeasures { get; } = Enum.GetValues(typeof(UnitOfMeasure)).Cast<UnitOfMeasure>();

        // IDataErrorInfo implementation
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
                }
                return error;
            }
        }

        public string Error => null;

        public bool Save()
        {
            // Set the flag to true to activate validation
            IsSaveAttempted = true;

            // Notify the UI that properties have changed to re-trigger validation
            OnPropertyChanged(nameof(SelectedTest));
            OnPropertyChanged(nameof(Result));

            // Check if there are any validation errors
            if (!HasErrors())
            {
                // Proceed with saving data
                // Add your save logic here

                return true; // Indicate success
            }

            return false; // Indicate failure due to validation errors
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Placeholder method to get available tests
        private IEnumerable<Test> GetAvailableTests()
        {
            return TestDal.GetTests();
        }

        private bool HasErrors()
        {
            return !ValidateSelectedTest() || !ValidateResult();
        }

        private bool ValidateSelectedTest()
        {
            return SelectedTest != null;
        }

        private bool ValidateResult()
        {
            return !string.IsNullOrWhiteSpace(Result) && double.TryParse(Result, out _);
        }
    }
}
