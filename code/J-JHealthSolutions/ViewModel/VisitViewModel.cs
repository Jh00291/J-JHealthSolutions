using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.Model.Domain;
using J_JHealthSolutions.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace J_JHealthSolutions.ViewModel
{
    public class VisitViewModel : INotifyPropertyChanged
    {

        // Event required by INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Collection for DataGrid
        private ObservableCollection<Visit> _visits;
        public ObservableCollection<Visit> Visits
        {
            get => _visits;
            set
            {
                if (_visits != value)
                {
                    _visits = value;
                    OnPropertyChanged(nameof(Visits));
                }
            }
        }

        // ICollectionView for filtering
        private ICollectionView _visitsView;
        public ICollectionView VisitsView
        {
            get => _visitsView;
            private set
            {
                if (_visitsView != value)
                {
                    _visitsView = value;
                    OnPropertyChanged(nameof(VisitsView));
                }
            }
        }

        // Selected Visit
        private Visit _selectedVisit;
        public Visit SelectedVisit
        {
            get => _selectedVisit;
            set
            {
                if (_selectedVisit != value)
                {
                    _selectedVisit = value;
                    OnPropertyChanged(nameof(SelectedVisit));
                    // Notify that EditCommand and CheckUpCommand's CanExecute might have changed
                    ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                }
            }
        }

        // Properties for Search Fields
        private string _searchPatientName;
        public string SearchPatientName
        {
            get => _searchPatientName;
            set
            {
                if (_searchPatientName != value)
                {
                    _searchPatientName = value;
                    OnPropertyChanged(nameof(SearchPatientName));
                    VisitsView.Refresh();
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
                    VisitsView.Refresh();
                }
            }
        }

        // New Search Properties
        private string _searchDoctorName;
        public string SearchDoctorName
        {
            get => _searchDoctorName;
            set
            {
                if (_searchDoctorName != value)
                {
                    _searchDoctorName = value;
                    OnPropertyChanged(nameof(SearchDoctorName));
                    VisitsView.Refresh();
                }
            }
        }

        private DateTime? _searchVisitDate;
        public DateTime? SearchVisitDate
        {
            get => _searchVisitDate;
            set
            {
                if (_searchVisitDate != value)
                {
                    _searchVisitDate = value;
                    OnPropertyChanged(nameof(SearchVisitDate));
                    VisitsView.Refresh();
                }
            }
        }

        // Commands
        public ICommand ClearCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CheckUpCommand { get; }

        // Constructor
        public VisitViewModel()
        {
            ClearCommand = new RelayCommand(ExecuteClearSearch);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEditOrCheckUp);
            LoadVisits();

            // Initialize the CollectionView for filtering
            VisitsView = CollectionViewSource.GetDefaultView(Visits);
            VisitsView.Filter = FilterVisits;
        }

        /// <summary>
        /// Loads visits from the database and populates the Visits collection.
        /// </summary>
        private void LoadVisits()
        {
            try
            {
                var visitsFromDb = VisitDal.GetVisits();
                var visitsWithCounts = new ObservableCollection<Visit>();

                foreach (var visit in visitsFromDb)
                {
                    visit.NumberOfTests = TestOrderDal.GetNumberOfTestsForVisit((int)visit.VisitId);
                    visit.NumberOfAbnormalTests = TestOrderDal.GetNumberOfAbnormalTestsForVisit((int)visit.VisitId);
                    visitsWithCounts.Add(visit);
                }

                Visits = visitsWithCounts;

                // Re-initialize the CollectionView with the new Visits collection
                VisitsView = CollectionViewSource.GetDefaultView(Visits);
                VisitsView.Filter = FilterVisits;
                OnPropertyChanged(nameof(VisitsView));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading visits: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Filter logic based on search criteria: Patient Name, DOB, Doctor Name, and Visit Date.
        /// </summary>
        private bool FilterVisits(object obj)
        {
            if (obj is Visit visit)
            {
                bool matchesPatientName = string.IsNullOrWhiteSpace(SearchPatientName) ||
                    visit.PatientFullName.IndexOf(SearchPatientName, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesDOB = !SearchDOB.HasValue ||
                    visit.PatientDOB.Date == SearchDOB.Value.Date;

                bool matchesDoctorName = string.IsNullOrWhiteSpace(SearchDoctorName) ||
                    visit.DoctorFullName.IndexOf(SearchDoctorName, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesVisitDate = !SearchVisitDate.HasValue ||
                    visit.VisitDateTime.Date == SearchVisitDate.Value.Date;

                return matchesPatientName && matchesDOB && matchesDoctorName && matchesVisitDate;
            }
            return false;
        }

        /// <summary>
        /// Executes the Clear Search command to reset all search criteria.
        /// </summary>
        private void ExecuteClearSearch(object parameter)
        {
            SearchPatientName = string.Empty;
            SearchDOB = null;
            SearchDoctorName = string.Empty;
            SearchVisitDate = null;
        }

        /// <summary>
        /// Determines whether the Edit command can execute based on whether a visit is selected.
        /// </summary>
        private bool CanExecuteEditOrCheckUp(object parameter)
        {
            return SelectedVisit != null;
        }

        /// <summary>
        /// Executes the Edit command to open the EditVisit window for editing the selected visit.
        /// </summary>
        private void ExecuteEdit(object parameter)
        {
            if (SelectedVisit == null)
                return;

            var editVisitWindow = new EditVisitWindow(SelectedVisit);
            if (editVisitWindow.ShowDialog() == true)
            {
                LoadVisits();
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
