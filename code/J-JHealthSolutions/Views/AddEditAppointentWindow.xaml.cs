using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.DAL;
using Mysqlx.Crud;

namespace J_JHealthSolutions.Views
{
    public partial class AddEditAppointmentWindow : Window
    {
        private List<Patient> allPatients;
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
        }

        private void PopulateAppointmentData(Appointment appointment)
        {
            patientAutoCompleteBox.Text = appointment.PatientFullName;
            doctorAutoCompleteBox.Text = appointment.DoctorFullName;

            selectedPatient = allPatients.FirstOrDefault(p => p.PatientId == appointment.PatientId);
            selectedDoctor = allDoctors.FirstOrDefault(d => d.DoctorId == appointment.DoctorId);

            datePicker.IsEnabled = true;
            datePicker.SelectedDate = appointment.DateTime.Date;

            UpdateAvailableTimeSlots();
            timeComboBox.SelectedItem = appointment.DateTime.ToString("hh:mm tt");

            reasonTextBox.Text = appointment.Reason;


            UpdateStatusComboBox();
            UpdateNurseAutoCompleteBox();

        }

        private void LoadPatients()
        {
            LoadAllPatients();
            patientAutoCompleteBox.ItemsSource = allPatients;
        }

        private void LoadDoctors()
        {
            LoadAllDoctors();
            doctorAutoCompleteBox.ItemsSource = allDoctors;
        }

        private void LoadNurses()
        {
            LoadAllNurses();
            nurseAutoCompleteBox.ItemsSource = allNurses;
        }

        private void LoadAllPatients()
        {
            PatientDal patientDal = new PatientDal();
            allPatients = patientDal.GetPatients();
        }

        private void LoadAllDoctors()
        {
            DoctorDal da = new DoctorDal();
            allDoctors = da.GetDoctors();
        }

        private void LoadAllNurses()
        {
            NurseDal da = new NurseDal();
            allNurses = da.GetNurses();
        }

        private void PatientAutoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            var searchText = patientAutoCompleteBox.SearchText.ToLower();
            var filteredPatients = allPatients.Where(p => p.PatientFullName.ToLower().Contains(searchText)).ToList();
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
            if (datePicker.SelectedDate.HasValue && doctorAutoCompleteBox.SelectedItem != null)
            {
                int selectedDoctorId = ((Doctor)doctorAutoCompleteBox.SelectedItem).DoctorId;
                DateTime selectedDate = datePicker.SelectedDate.Value;

                TimeSpan startTime = TimeSpan.FromHours(8);
                TimeSpan endTime = TimeSpan.FromHours(17);
                TimeSpan interval = TimeSpan.FromMinutes(20);

                timeComboBox.Items.Clear();

                var appointmentDal = new AppointmentDal();

                for (TimeSpan time = startTime; time < endTime; time += interval)
                {
                    DateTime appointmentDateTime = selectedDate.Date + time;

                    bool isAvailable = appointmentDal.IsTimeSlotAvailable(selectedDoctorId, appointmentDateTime);
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

        private void DoctorAutoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            var searchText = doctorAutoCompleteBox.SearchText.ToLower();
            var filteredDoctors = allDoctors.Where(d => d.EmployeeFullName.ToLower().Contains(searchText)).ToList();
            doctorAutoCompleteBox.ItemsSource = filteredDoctors;
            doctorAutoCompleteBox.PopulateComplete();
        }

        private void DoctorAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoctor = doctorAutoCompleteBox.SelectedItem as Doctor;
            this.datePicker.IsEnabled = true;
        }

        private void SaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            ValidatePatientSelection();
            ValidateDoctorSelection();
            ValidateNurseSelection();

            if (selectedPatient == null)
            {
                MessageBox.Show("Please select a patient.");
                return;
            }

            if (selectedDoctor == null)
            {
                MessageBox.Show("Please select a doctor.");
                return;
            }

            if (datePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select a date.");
                return;
            }

            if (string.IsNullOrWhiteSpace(reasonTextBox.Text))
            {
                MessageBox.Show("Please enter a reason for the appointment.");
                return;
            }

            var statusString = statusComboBox.SelectedItem.ToString();
            if (!Enum.TryParse(statusString, out Status appointmentStatus))
            {
                MessageBox.Show("Invalid status selected.");
                return;
            }

            if ((appointmentStatus == Status.InProgress || appointmentStatus == Status.Completed) && selectedNurse == null)
            {
                MessageBox.Show("Please select a nurse for the appointment.");
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

            MessageBox.Show("Appointment saved successfully.");
            this.Close();
        }

        private void SaveAppointment(Appointment appointment)
        {
            try
            {
                var appointmentDal = new AppointmentDal();
                if (_appointment != null)
                {
                    appointment.AppointmentId = _appointment.AppointmentId;
                    appointmentDal.UpdateAppointment(appointment);
                }
                else
                {
                    appointmentDal.AddAppointment(appointment);
                }
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void ValidatePatientSelection()
        {
            string inputText = patientAutoCompleteBox.Text.Trim();
            if (string.IsNullOrEmpty(inputText))
            {
                selectedPatient = null;
                return;
            }

            var matchingPatient = allPatients.FirstOrDefault(p => p.PatientFullName.Equals(inputText, StringComparison.OrdinalIgnoreCase));
            if (matchingPatient != null)
            {
                selectedPatient = matchingPatient;
            }
            else
            {
                selectedPatient = null;
                MessageBox.Show("The patient you entered does not exist. Please select a valid patient from the list.", "Invalid Patient", MessageBoxButton.OK, MessageBoxImage.Warning);
                patientAutoCompleteBox.Text = string.Empty;
            }
        }

        private void ValidateDoctorSelection()
        {
            string inputText = doctorAutoCompleteBox.Text.Trim();
            if (string.IsNullOrEmpty(inputText))
            {
                selectedDoctor = null;
                return;
            }

            var matchingDoctor = allDoctors.FirstOrDefault(d => d.EmployeeFullName.Equals(inputText, StringComparison.OrdinalIgnoreCase));
            if (matchingDoctor != null)
            {
                selectedDoctor = matchingDoctor;
            }
            else
            {
                selectedDoctor = null;
                MessageBox.Show("The doctor you entered does not exist. Please select a valid doctor from the list.", "Invalid Doctor", MessageBoxButton.OK, MessageBoxImage.Warning);
                doctorAutoCompleteBox.Text = string.Empty;
            }
        }

        private void ValidateNurseSelection()
        {
            string inputText = nurseAutoCompleteBox.Text.Trim();
            if (string.IsNullOrEmpty(inputText))
            {
                selectedNurse = null;
                return;
            }

            var matchingNurse = allNurses.FirstOrDefault(n => n.EmployeeFullName.Equals(inputText, StringComparison.OrdinalIgnoreCase));
            if (matchingNurse != null)
            {
                selectedNurse = matchingNurse;
            }
            else
            {
                selectedNurse = null;
                MessageBox.Show("The nurse you entered does not exist. Please select a valid nurse from the list.", "Invalid Nurse", MessageBoxButton.OK, MessageBoxImage.Warning);
                nurseAutoCompleteBox.Text = string.Empty;
            }
        }


        private void UpdateStatusComboBox()
        {
            List<string> statuses;

            if (isEdit)
            {
                // Editing an existing appointment: only allow "Completed", "NoShow", "Cancelled"
                statuses = new List<string> { "Completed", "NoShow", "Cancelled" };
            }
            else
            {
                // Adding a new appointment: only allow "Scheduled", "InProgress"
                statuses = new List<string> { "Scheduled", "InProgress" };
            }

            string currentStatus = statusComboBox.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(currentStatus) && statuses.Contains(currentStatus))
            {
                statusComboBox.ItemsSource = statuses;
                statusComboBox.SelectedItem = currentStatus;
            }
            else
            {
                statusComboBox.ItemsSource = statuses;
                if (statuses.Count > 0)
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
                if (statusString == "InProgress" || statusString == "Completed" || statusString == "Scheduled")
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
