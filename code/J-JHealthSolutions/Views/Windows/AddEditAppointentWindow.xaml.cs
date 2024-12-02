using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model.Domain;
using J_JHealthSolutions.Model.Other;

namespace J_JHealthSolutions.Views
{
    public partial class AddEditAppointmentWindow : Window
    {
        private List<Patient> allActivePatients;
        private IEnumerable<Doctor> allDoctors;
        private IEnumerable<Nurse> allNurses;

        private Patient selectedPatient;
        private Doctor selectedDoctor;
        private Nurse selectedNurse;

        private Appointment _appointment;

        private Boolean isEdit = false;

        public AddEditAppointmentWindow()
        {
            InitializeComponent();

            LoadPatients();
            LoadDoctors();
            LoadNurses();

            statusComboBox.SelectionChanged += StatusComboBox_SelectionChanged;

            UpdateStatusComboBox();
            UpdateNurseAutoCompleteBox();
        }

        public AddEditAppointmentWindow(Appointment appointment)
        {
            isEdit = true;
            InitializeComponent();

            LoadPatients();
            LoadDoctors();
            LoadNurses();

            this.Title = "Edit Appointment";
            this.titleLabel.Content = "Edit Appointment";
            _appointment = appointment;
            PopulateAppointmentData(appointment);

            statusComboBox.SelectionChanged += StatusComboBox_SelectionChanged;
            UpdateNurseAutoCompleteBox();
            UpdateStatusComboBox();
            UpdateStatusComboBox(_appointment.Status);
        }

        private void PopulateAppointmentData(Appointment appointment)
        {
            patientAutoCompleteBox.Text = appointment.PatientFullName;
            doctorComboBox.SelectedItem = allDoctors.FirstOrDefault(d => d.DoctorId == appointment.DoctorId);

            selectedPatient = allActivePatients.FirstOrDefault(p => p.PatientId == appointment.PatientId);
            selectedDoctor = allDoctors.FirstOrDefault(d => d.DoctorId == appointment.DoctorId);

            datePicker.IsEnabled = true;
            datePicker.SelectedDate = appointment.DateTime.Date;

            UpdateAvailableTimeSlots();
            timeComboBox.SelectedItem = appointment.DateTime.ToString("hh:mm tt");

            reasonTextBox.Text = appointment.Reason;


            UpdateStatusComboBox();
            UpdateNurseAutoCompleteBox();

        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoctor = doctorComboBox.SelectedItem as Doctor;
            this.datePicker.IsEnabled = selectedDoctor != null;
            UpdateAvailableTimeSlots();
        }

        private void LoadPatients()
        {
            LoadAllPatients();
            patientAutoCompleteBox.ItemsSource = allActivePatients;
        }

        private void LoadDoctors()
        {
            LoadAllDoctors();
            doctorComboBox.ItemsSource = allDoctors;
            doctorComboBox.DisplayMemberPath = "EmployeeFullName";
        }

        private void LoadNurses()
        {
            LoadAllNurses();
            nurseAutoCompleteBox.ItemsSource = allNurses;
        }

        private void LoadAllPatients()
        {
            allActivePatients = PatientDal.GetActivePatients();
        }

        private void LoadAllDoctors()
        {
            allDoctors = DoctorDal.GetDoctors();
        }

        private void LoadAllNurses()
        {
            NurseDal da = new NurseDal();
            allNurses = da.GetNurses();
        }

        private void PatientAutoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            var searchText = patientAutoCompleteBox.SearchText.ToLower();
            var filteredPatients = allActivePatients.Where(p => p.PatientFullName.ToLower().Contains(searchText)).ToList();
            patientAutoCompleteBox.ItemsSource = filteredPatients;
            patientAutoCompleteBox.PopulateComplete();
        }

        private void PatientAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPatient = patientAutoCompleteBox.SelectedItem as Patient;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            this.timeComboBox.IsEnabled = true;
            UpdateAvailableTimeSlots();
        }

        private void TimeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void UpdateAvailableTimeSlots()
        {
            if (datePicker.SelectedDate.HasValue && doctorComboBox.SelectedItem != null)
            {
                int selectedDoctorId = selectedDoctor.DoctorId;
                DateTime selectedDate = datePicker.SelectedDate.Value;

                TimeSpan startTime = TimeSpan.FromHours(8);
                TimeSpan endTime = TimeSpan.FromHours(17);
                TimeSpan interval = TimeSpan.FromMinutes(20);

                timeComboBox.Items.Clear();


                for (TimeSpan time = startTime; time < endTime; time += interval)
                {
                    DateTime appointmentDateTime = selectedDate.Date + time;

                    bool isAvailable = AppointmentDal.IsTimeSlotAvailable(selectedDoctorId, appointmentDateTime);
                    if (isAvailable)
                    {
                        timeComboBox.Items.Add(appointmentDateTime.ToString("hh:mm tt"));
                    }
                }

                if (timeComboBox.Items.Count > 0)
                {
                    timeComboBox.SelectedItem = timeComboBox.Items[0];
                }
                else
                {
                    MessageBox.Show("No available time slots for the selected date and doctor.", "No Availability", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                timeComboBox.Items.Clear();
            }
        }

        private void SaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            patientErrorLabel.Visibility = Visibility.Collapsed;
            doctorErrorLabel.Visibility = Visibility.Collapsed;
            dateErrorLabel.Visibility = Visibility.Collapsed;
            reasonErrorLabel.Visibility = Visibility.Collapsed;
            statusErrorLabel.Visibility = Visibility.Collapsed;
            nurseErrorLabel.Visibility = Visibility.Collapsed;

            dateErrorLabel.Content = "Please select a date.";

            if (selectedPatient == null)
            {
                patientErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (selectedDoctor == null)
            {
                doctorErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (datePicker.SelectedDate == null)
            {
                dateErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(reasonTextBox.Text))
            {
                reasonErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            var statusString = statusComboBox.SelectedItem?.ToString();
            if (!Enum.TryParse(statusString, out Status appointmentStatus))
            {
                statusErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if ((appointmentStatus == Status.InProgress || appointmentStatus == Status.Completed) && selectedNurse == null)
            {
                nurseErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }

            if (hasError)
            {
                return;
            }

            DateTime selectedDate = datePicker.SelectedDate.Value;

            if (!isEdit)
            {
                if (selectedDate.Date < DateTime.Today)
                {
                    dateErrorLabel.Content = "The date cannot be in the past.";
                    dateErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            if (appointmentStatus == Status.InProgress)
            {
                if (selectedDate.Date > DateTime.Today)
                {
                    dateErrorLabel.Content = "The date cannot be in the future for an appointment in progress.";
                    dateErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }

            if (hasError)
            {
                return;
            }

            var appointment = new Appointment
            {
                PatientId = selectedPatient.PatientId.Value,
                DoctorId = selectedDoctor.DoctorId,
                DateTime = CombineDateAndTime(datePicker.SelectedDate.Value, timeComboBox.SelectedItem.ToString()),
                Reason = reasonTextBox.Text,
                Status = appointmentStatus,
            };

            SaveAppointment(appointment);
            this.Close();
        }


        private void SaveAppointment(Appointment appointment)
        {
            try
            {
                if (_appointment != null)
                {
                    appointment.AppointmentId = _appointment.AppointmentId;
                    AppointmentDal.UpdateAppointment(appointment);
                }
                else
                {
                    int newAppointmentId = AppointmentDal.AddAppointment(appointment);
                    appointment.AppointmentId = newAppointmentId;
                }

                if (selectedNurse != null)
                {
                    var existingVisit = VisitDal.GetVisitByAppointmentId(appointment.AppointmentId.Value);

                    if (existingVisit == null)
                    {
                        Visit newVisit = new Visit
                        {
                            AppointmentId = appointment.AppointmentId.Value,
                            PatientId = appointment.PatientId,
                            DoctorId = appointment.DoctorId,
                            NurseId = selectedNurse.NurseId,
                            VisitDateTime = appointment.DateTime,
                            VisitStatus = "InProgress",
                            BloodPressureDiastolic = 0,
                            BloodPressureSystolic = 0,
                            Height = 0,
                            Weight = 0,
                            Pulse = 0,
                            Temperature = 0,
                            Symptoms = string.Empty,
                            InitialDiagnosis = string.Empty,
                            FinalDiagnosis = string.Empty
                        };

                        VisitDal.AddVisit(newVisit);
                    }
                }
                MessageBox.Show("Appointment saved successfully.");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowErrorDialog("Error", $"Error saving appointment: {ex.Message}");
            }
        }

        private void ShowErrorDialog(string title, string message)
        {
            var errorDialog = new Window
            {
                Title = title,
                Content = new TextBlock { Text = message, Margin = new Thickness(10) },
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            errorDialog.ShowDialog();
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private DateTime CombineDateAndTime(DateTime date, string timeString)
        {
            if (DateTime.TryParse(timeString, out DateTime time))
            {
                return date.Date + time.TimeOfDay;
            }
            else
            {
                throw new ArgumentException("Invalid time format.");
            }
        }

        private void UpdateStatusComboBox(Status? currentStatus = null)
        {
            List<string> statuses;

            if (isEdit)
            {
              statuses = new List<string> { "Scheduled", "Completed", "InProgress", "NoShow", "Cancelled" };
            }
            else
            {
              statuses = new List<string> { "Scheduled", "InProgress" };
            }

            statusComboBox.ItemsSource = statuses;

            if (currentStatus != null && statuses.Contains(currentStatus.ToString()))
            {
                statusComboBox.SelectedItem = currentStatus.ToString();
            }
            else if (statuses.Count > 0)
            {
              statusComboBox.SelectedIndex = 0;
            }

            UpdateNurseAutoCompleteBox();
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateNurseAutoCompleteBox();
        }

        private void UpdateNurseAutoCompleteBox()
        {
            if (statusComboBox.SelectedItem != null)
            {
                var statusString = statusComboBox.SelectedItem.ToString();
                if (statusString == "InProgress" || statusString == "Completed")
                {
                    nurseAutoCompleteBox.Visibility = Visibility.Visible;
                    nurseLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    nurseAutoCompleteBox.Visibility = Visibility.Hidden;
                    nurseLabel.Visibility = Visibility.Hidden;
                    nurseAutoCompleteBox.Text = string.Empty;
                    selectedNurse = null;
                }
            }
        }

        private void NurseAutoCompleteBox_OnPopulating(object sender, PopulatingEventArgs e)
        {
            var searchText = nurseAutoCompleteBox.SearchText.ToLower();
            var filteredNurses = allNurses.Where(n => n.EmployeeFullName.ToLower().Contains(searchText)).ToList();
            nurseAutoCompleteBox.ItemsSource = filteredNurses;
            nurseAutoCompleteBox.PopulateComplete();
        }

        private void NurseAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedNurse = nurseAutoCompleteBox.SelectedItem as Nurse;
        }
    }
}
