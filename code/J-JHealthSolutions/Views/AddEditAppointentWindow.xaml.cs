using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.Model;
using System.Linq;
using System.Numerics;
using J_JHealthSolutions.DAL;

namespace J_JHealthSolutions.Views
{
    public partial class AddEditAppointmentWindow : Window
    {
        private List<Patient> allPatients;
        private List<Employee> allDoctors;

        private Patient selectedPatient;
        private Employee selectedDoctor;

        public AddEditAppointmentWindow()
        {
            InitializeComponent();

            // Load patients and doctors
            LoadPatients();
            LoadDoctors();

            // Populate statusComboBox
            statusComboBox.Items.Clear();
            statusComboBox.ItemsSource = Enum.GetNames(typeof(Status));
            statusComboBox.SelectedIndex = 0; // Default to first status
        }

        private void LoadPatients()
        {
            // Fetch patients from your data source
            LoadAllPatients();

            // Optionally, set an initial ItemsSource
            patientAutoCompleteBox.ItemsSource = allPatients;
        }

        private void LoadDoctors()
        {
            // Fetch doctors from your data source
            LoadAllDoctors();

            // Optionally, set an initial ItemsSource
            doctorAutoCompleteBox.ItemsSource = allDoctors;
        }

        private void LoadAllPatients()
        {
            PatientDal patientDal = new PatientDal();
            allPatients = patientDal.GetPatients();
        }

        private void LoadAllDoctors()
        {
            EmployeeDal employeeDal = new EmployeeDal();
            allDoctors = employeeDal.GetAllDoctors();
        }

        // Event handler for patient search
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
                int selectedDoctorId = ((Employee)doctorAutoCompleteBox.SelectedItem).EmployeeId.Value;
                DateTime selectedDate = datePicker.SelectedDate.Value;

                // Define start and end times, and interval
                TimeSpan startTime = TimeSpan.FromHours(8); // 8 AM
                TimeSpan endTime = TimeSpan.FromHours(17);  // 5 PM
                TimeSpan interval = TimeSpan.FromMinutes(20); // 20-minute intervals

                // Clear existing items in ComboBox
                timeComboBox.Items.Clear();

                var appointmentDal = new AppointmentDal();

                // Loop through each time slot, checking availability
                for (TimeSpan time = startTime; time < endTime; time += interval)
                {
                    DateTime appointmentDateTime = selectedDate.Date + time;

                    // Check if the time slot is available
                    bool isAvailable = appointmentDal.IsTimeSlotAvailable(selectedDoctorId, appointmentDateTime);
                    if (isAvailable)
                    {
                        // Add available time slot to ComboBox in AM/PM format
                        timeComboBox.Items.Add(appointmentDateTime.ToString("hh:mm tt"));
                    }
                }

                // Set a default selection if available slots are found
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
                // Clear ComboBox if no date or doctor is selected
                timeComboBox.Items.Clear();
            }
        }



        // Event handler for doctor search
        private void DoctorAutoCompleteBox_Populating(object sender, PopulatingEventArgs e)
        {
            var searchText = doctorAutoCompleteBox.SearchText.ToLower();
            var filteredDoctors = allDoctors.Where(d => (d.FName + " " + d.LName).ToLower().Contains(searchText)).ToList();
            doctorAutoCompleteBox.ItemsSource = filteredDoctors;
            doctorAutoCompleteBox.PopulateComplete();
        }

        private void DoctorAutoCompleteBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDoctor = doctorAutoCompleteBox.SelectedItem as Employee;
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

            // Get selected status
            var statusString = statusComboBox.SelectedItem.ToString();
            if (!Enum.TryParse(statusString, out Status appointmentStatus))
            {
                MessageBox.Show("Invalid status selected.");
                return;
            }

            // Create new appointment
            var appointment = new Appointment
            {
                PatientId = selectedPatient.PatientId.Value,
                DoctorId = selectedDoctor.EmployeeId.Value,
                Reason = reasonTextBox.Text,
                Status = appointmentStatus
            };

            // Save appointment to your data source
            SaveAppointment(appointment);

            MessageBox.Show("Appointment saved successfully.");
            this.Close();
        }

        private void SaveAppointment(Appointment appointment)
        {
            // Implement logic to save the appointment to your database or service
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
