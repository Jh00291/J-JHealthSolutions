using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace J_JHealthSolutions.ViewModel
{
    public class PatientViewModel : INotifyPropertyChanged
    {
        private readonly PatientDal _patientDal;

        // Event required by INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Collection for DataGrid
        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set
            {
                if (_patients != value)
                {
                    _patients = value;
                    OnPropertyChanged(nameof(Patients));
                }
            }
        }

        // ICollectionView for filtering
        private ICollectionView _patientsView;
        public ICollectionView PatientsView
        {
            get => _patientsView;
            private set
            {
                if (_patientsView != value)
                {
                    _patientsView = value;
                    OnPropertyChanged(nameof(PatientsView));
                }
            }
        }

        // Selected Patient
        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                if (_selectedPatient != value)
                {
                    _selectedPatient = value;
                    OnPropertyChanged(nameof(SelectedPatient));
                    // Notify that EditCommand and DeleteCommand's CanExecute might have changed
                    ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                }
            }
        }

        // Properties for Search Fields
        private string _searchLastName;
        public string SearchLastName
        {
            get => _searchLastName;
            set
            {
                if (_searchLastName != value)
                {
                    _searchLastName = value;
                    OnPropertyChanged(nameof(SearchLastName));
                    PatientsView.Refresh();
                }
            }
        }

        private string _searchFirstName;
        public string SearchFirstName
        {
            get => _searchFirstName;
            set
            {
                if (_searchFirstName != value)
                {
                    _searchFirstName = value;
                    OnPropertyChanged(nameof(SearchFirstName));
                    PatientsView.Refresh();
                }
            }
        }

        private DateTime? _searchDOB;
        public DateTime? SearchDOB
        {
            get => _searchDOB;
            set
            {
                if (_searchDOB != value)
                {
                    _searchDOB = value;
                    OnPropertyChanged(nameof(SearchDOB));
                    PatientsView.Refresh();
                }
            }
        }

        // Commands
        public ICommand ClearCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        // Constructor
        public PatientViewModel()
        {
            _patientDal = new PatientDal();
            ClearCommand = new RelayCommand(ExecuteClearSearch);
            AddCommand = new RelayCommand(ExecuteAddPatient);
            EditCommand = new RelayCommand(ExecuteEditPatient, CanExecuteEditOrDelete);
            DeleteCommand = new RelayCommand(ExecuteDeletePatient, CanExecuteEditOrDelete);
            LoadPatients();

            // Initialize the CollectionView for filtering
            PatientsView = CollectionViewSource.GetDefaultView(Patients);
            PatientsView.Filter = FilterPatients;
        }

        /// <summary>
        /// Loads patients from the database and populates the Patients collection.
        /// </summary>
        private void LoadPatients()
        {
            try
            {
                var patientsFromDb = _patientDal.GetPatients();
                Patients = new ObservableCollection<Patient>(patientsFromDb);

                // Re-initialize the CollectionView with the new Patients collection
                PatientsView = CollectionViewSource.GetDefaultView(Patients);
                PatientsView.Filter = FilterPatients;
                OnPropertyChanged(nameof(PatientsView));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patients: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Filter logic based on search criteria: Last Name, First Name, and DOB.
        /// </summary>
        private bool FilterPatients(object obj)
        {
            if (obj is Patient patient)
            {
                bool matchesLastName = string.IsNullOrWhiteSpace(SearchLastName) ||
                    patient.LName.IndexOf(SearchLastName, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesFirstName = string.IsNullOrWhiteSpace(SearchFirstName) ||
                    patient.FName.IndexOf(SearchFirstName, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesDOB = !SearchDOB.HasValue ||
                    patient.DOB.Date == SearchDOB.Value.Date;

                return matchesLastName && matchesFirstName && matchesDOB;
            }
            return false;
        }

        /// <summary>
        /// Executes the Clear Search command to reset all search criteria.
        /// </summary>
        private void ExecuteClearSearch(object parameter)
        {
            SearchLastName = string.Empty;
            SearchFirstName = string.Empty;
            SearchDOB = null;
        }

        /// <summary>
        /// Determines whether the Edit and Delete commands can execute based on whether a patient is selected.
        /// </summary>
        private bool CanExecuteEditOrDelete(object parameter)
        {
            return SelectedPatient != null;
        }

        /// <summary>
        /// Executes the Add command to open the AddPatientWindow for adding a new patient.
        /// </summary>
        private void ExecuteAddPatient(object parameter)
        {
            var addEditPatientWindow = new AddEditPatientWindow();
            if (addEditPatientWindow.ShowDialog() == true)
            {
                LoadPatients();
            }
        }

        /// <summary>
        /// Executes the Edit command to open the EditPatientWindow for editing the selected patient.
        /// </summary>
        private void ExecuteEditPatient(object parameter)
        {
            if (SelectedPatient == null)
                return;

            var addEditPatientWindow = new AddEditPatientWindow(SelectedPatient);
            if (addEditPatientWindow.ShowDialog() == true)
            {
                LoadPatients();
            }
        }

        /// <summary>
        /// Executes the Delete command to remove the selected patient after confirmation.
        /// </summary>
        private void ExecuteDeletePatient(object parameter)
        {
            if (SelectedPatient == null)
                return;

            var result = MessageBox.Show($"Are you sure you want to delete patient {SelectedPatient.FName} {SelectedPatient.LName}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _patientDal.DeletePatient((int)SelectedPatient.PatientId);
                    Patients.Remove(SelectedPatient);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting patient: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event for a given property name.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
