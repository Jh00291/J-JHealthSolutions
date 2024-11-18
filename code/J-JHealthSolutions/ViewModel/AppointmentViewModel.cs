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
using System.Windows;
using System.Windows.Data;

namespace J_JHealthSolutions.ViewModel
{
    public class AppointmentViewModel : INotifyPropertyChanged
    {
        // Event required by INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        // Collection of appointments bound to the DataGrid
        private ObservableCollection<Appointment> _appointments;
        public ObservableCollection<Appointment> Appointments
        {
            get => _appointments;
            set
            {
                if (_appointments != value)
                {
                    _appointments = value;
                    OnPropertyChanged(nameof(Appointments));
                }
            }
        }

        // ICollectionView for filtering
        private ICollectionView _appointmentsView;
        public ICollectionView AppointmentsView
        {
            get => _appointmentsView;
            private set
            {
                if (_appointmentsView != value)
                {
                    _appointmentsView = value;
                    OnPropertyChanged(nameof(AppointmentsView));
                }
            }
        }

        // Currently selected appointment in the DataGrid
        private Appointment _selectedAppointment;
        public Appointment SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                if (_selectedAppointment != value)
                {
                    _selectedAppointment = value;
                    OnPropertyChanged(nameof(SelectedAppointment));
                    // Notify that EditCommand's CanExecute might have changed
                    ((RelayCommand)EditCommand).RaiseCanExecuteChanged();
                }
            }
        }

        // Search Criteria Properties
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
                    AppointmentsView.Refresh();
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
                    AppointmentsView.Refresh();
                }
            }
        }

        private DateTime? _searchAppointmentDate;
        public DateTime? SearchAppointmentDate
        {
            get => _searchAppointmentDate;
            set
            {
                if (_searchAppointmentDate != value)
                {
                    _searchAppointmentDate = value;
                    OnPropertyChanged(nameof(SearchAppointmentDate));
                    AppointmentsView.Refresh();
                }
            }
        }

        private DateTime? _searchPatientDOB;
        public DateTime? SearchPatientDOB
        {
            get => _searchPatientDOB;
            set
            {
                if (_searchPatientDOB != value)
                {
                    _searchPatientDOB = value;
                    OnPropertyChanged(nameof(SearchPatientDOB));
                    AppointmentsView.Refresh();
                }
            }
        }

        // Commands for Add, Edit, and Clear Search actions
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ClearSearchCommand { get; }

        // Constructor
        public AppointmentViewModel()
        {
            Appointments = new ObservableCollection<Appointment>();
            AddCommand = new RelayCommand(ExecuteAdd);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEdit);
            ClearSearchCommand = new RelayCommand(ExecuteClearSearch);
            LoadAppointments();

            // Initialize the CollectionView for filtering
            AppointmentsView = CollectionViewSource.GetDefaultView(Appointments);
            AppointmentsView.Filter = FilterAppointments;
        }

        /// <summary>
        /// Loads appointments from the database and populates the Appointments collection.
        /// </summary>
        public void LoadAppointments()
        {
            try
            {
                var appointmentsFromDb = AppointmentDal.GetAppointments();
                Appointments.Clear();
                foreach (var appointment in appointmentsFromDb)
                {
                    Appointments.Add(appointment);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Filter logic based on search criteria, including Patient DOB.
        /// </summary>
        private bool FilterAppointments(object obj)
        {
            if (obj is Appointment appointment)
            {
                bool matchesPatientName = string.IsNullOrWhiteSpace(SearchPatientName) ||
                    appointment.PatientFullName.IndexOf(SearchPatientName, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesDoctorName = string.IsNullOrWhiteSpace(SearchDoctorName) ||
                    appointment.DoctorFullName.IndexOf(SearchDoctorName, StringComparison.OrdinalIgnoreCase) >= 0;

                bool matchesAppointmentDate = !SearchAppointmentDate.HasValue ||
                    appointment.DateTime.Date == SearchAppointmentDate.Value.Date;

                bool matchesPatientDOB = !SearchPatientDOB.HasValue ||
                    appointment.PatientDOB.Date == SearchPatientDOB.Value.Date;

                return matchesPatientName && matchesDoctorName && matchesAppointmentDate && matchesPatientDOB;
            }
            return false;
        }

        /// <summary>
        /// Executes the Add command to open the AddEditAppointmentWindow for adding a new appointment.
        /// </summary>
        private void ExecuteAdd(object parameter)
        {
            var addEditWindow = new AddEditAppointmentWindow();
            if (addEditWindow.ShowDialog() == true)
            {
                LoadAppointments();
            }
        }

        /// <summary>
        /// Determines whether the Edit command can execute based on whether an appointment is selected.
        /// </summary>
        private bool CanExecuteEdit(object parameter)
        {
            return SelectedAppointment != null;
        }

        /// <summary>
        /// Executes the Edit command to open the AddEditAppointmentWindow for editing the selected appointment.
        /// </summary>
        private void ExecuteEdit(object parameter)
        {
            if (SelectedAppointment == null)
                return;

            var addEditWindow = new AddEditAppointmentWindow(SelectedAppointment);
            if (addEditWindow.ShowDialog() == true)
            {
                LoadAppointments();
            }
        }

        /// <summary>
        /// Executes the Clear Search command to reset all search criteria, including Patient DOB.
        /// </summary>
        private void ExecuteClearSearch(object parameter)
        {
            SearchPatientName = string.Empty;
            SearchDoctorName = string.Empty;
            SearchAppointmentDate = null;
            SearchPatientDOB = null;
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
