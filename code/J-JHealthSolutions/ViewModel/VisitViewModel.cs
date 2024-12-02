using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.Model.Domain;
using J_JHealthSolutions.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace J_JHealthSolutions.ViewModel
{
    public class VisitViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                    ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                }
            }
        }

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
                    LoadVisits();
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
                    LoadVisits();
                }
            }
        }

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
                    LoadVisits();
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
                    LoadVisits();
                }
            }
        }

        public ICommand ClearCommand { get; }
        public ICommand EditCommand { get; }

        public VisitViewModel()
        {
            ClearCommand = new RelayCommand(ExecuteClearSearch);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEditOrCheckUp);
            LoadVisits();
        }

        /// <summary>
        /// Loads visits from the database based on current search criteria and populates the Visits collection.
        /// </summary>
        private void LoadVisits()
        {
            try
            {
                var visitsFromDb = VisitDal.SearchVisitsAdvanced(SearchPatientName, SearchDOB, SearchDoctorName, SearchVisitDate);
                var visitsWithCounts = new ObservableCollection<Visit>();

                foreach (var visit in visitsFromDb)
                {
                    // Assuming TestOrderDal is available and properly implemented
                    visit.NumberOfTests = TestOrderDal.GetNumberOfTestsForVisit((int)visit.VisitId);
                    visit.NumberOfAbnormalTests = TestOrderDal.GetNumberOfAbnormalTestsForVisit((int)visit.VisitId);
                    visitsWithCounts.Add(visit);
                }

                Visits = visitsWithCounts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading visits: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
