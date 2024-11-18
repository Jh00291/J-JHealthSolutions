using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace J_JHealthSolutions.ViewModel
{
    public class VisitViewModel : INotifyPropertyChanged
    {
        private readonly VisitDal _visitDal;

        public VisitViewModel()
        {
            _visitDal = new VisitDal();
            LoadVisits();

            SearchCommand = new RelayCommand(ExecuteSearch);
            ClearCommand = new RelayCommand(ExecuteClear);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEditOrCheckUp);
            CheckUpCommand = new RelayCommand(ExecuteCheckUp, CanExecuteEditOrCheckUp);
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
                }
            }
        }

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
                    ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)CheckUpCommand).RaiseCanExecuteChanged();
                }
            }
        }

        // Commands
        public ICommand SearchCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand CheckUpCommand { get; }

        // Methods
        private void LoadVisits()
        {
            var visits = _visitDal.GetVisits();
            var visitsWithCounts = new ObservableCollection<Visit>();

            foreach (var visit in visits)
            {
                visit.NumberOfTests = TestOrderDal.GetNumberOfTestsForVisit((int)visit.VisitId);
                visit.NumberOfAbnormalTests = TestOrderDal.GetNumberOfAbnormalTestsForVisit((int)visit.VisitId);
                visitsWithCounts.Add(visit);
            }

            Visits = visitsWithCounts;
        }

        private void ExecuteSearch(object parameter)
        {
            var visits = _visitDal.SearchVisits(SearchPatientName, SearchDOB);
            Visits = new ObservableCollection<Visit>(visits);
        }

        private void ExecuteClear(object parameter)
        {
            SearchPatientName = string.Empty;
            SearchDOB = null;
            LoadVisits();
        }

        private bool CanExecuteEditOrCheckUp(object parameter)
        {
            return SelectedVisit != null;
        }

        private void ExecuteEdit(object parameter)
        {
            if (SelectedVisit != null)
            {
                var editVisitWindow = new EditVisit(SelectedVisit);
                bool? dialogResult = editVisitWindow.ShowDialog();
                if (dialogResult == true)
                {
                    LoadVisits();
                }
            }
        }

        private void ExecuteCheckUp(object parameter)
        {
            if (SelectedVisit != null)
            {
                var checkUpWindow = new CheckUpWindow(SelectedVisit);
                checkUpWindow.ShowDialog();
            }
        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
