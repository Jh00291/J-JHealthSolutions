using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.DAL;

namespace J_JHealthSolutions.Views
{
    public partial class AddEditAppointmentWindow : Window
    {
        private List<Patient> allPatients;
        private IEnumerable<Doctor> allDoctors;

        private Patient selectedPatient;
        private Doctor selectedDoctor;

        private Appointment _appointment;

        public AddEditAppointmentWindow()
        {
            InitializeComponent();

            LoadPatients();
            LoadDoctors();

            statusComboBox.Items.Clear();
            statusComboBox.ItemsSource = Enum.GetNames(typeof(Status));
            statusComboBox.SelectedIndex = 0;
        }

        public AddEditAppointmentWindow(Appointment appointment)
        {
            InitializeComponent();
            this.Title = "Edit Appointment";
            this.titleLabel.Content = "Edit Appointment";
            _appointment = appointment;
            PopulateAppointmentData(appointment);
        }

        private void PopulateAppointmentData(Appointment appointment)
        {
            patientAutoCompleteBox.Text = appointment.PatientFullName;
            doctorAutoCompleteBox.Text = appointment.DoctorFullName;

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

        private void PatientAutoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            var searchText = patientAutoCompleteBox.SearchText.ToLower();
            var filteredPatients = allPatients.Where(p => p.FName.ToLower().Contains(searchText)).ToList();
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
            var filteredDoctors = allDoctors.Where(d => (d.FName + " " + d.LName).ToLower().Contains(searchText)).ToList();
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
            // Validate inputs
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

            var appointment = new Appointment
            {
                PatientId = selectedPatient.PatientId.Value,
                DoctorId = selectedDoctor.DoctorId,
                Reason = reasonTextBox.Text,
                Status = appointmentStatus
            };

            SaveAppointment(appointment);

            MessageBox.Show("Appointment saved successfully.");
            this.Close();
        }

        private void SaveAppointment(Appointment appointment)
        {
            AppointmentDal appointmentDal = new AppointmentDal();
            appointmentDal.AddAppointment(appointment);
            this.DialogResult = true;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
