using System.Windows;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.Views
{
    public partial class CheckUpWindow : Window
    {

        /// <summary>
        /// Interaction logic for CheckupWindow.xaml
        /// </summary>
        private Visit _visit;

        /// <summary>
        /// Initializes a new instance of CheckUpWindow with the selected visit details.
        /// </summary>
        /// <param name="visit">The visit to be displayed in the check-up window</param>
        public CheckUpWindow(Visit visit)
        {
            InitializeComponent();
            _visit = visit;
            PopulateCheckUpData();

            // Set read-only mode based on status or final diagnosis
            IsReadOnly = visit.VisitStatus == "Completed" || !string.IsNullOrWhiteSpace(visit.FinalDiagnosis);

            saveButton.IsEnabled = !IsReadOnly;

        }



        /// <summary>
        /// Populates the check-up fields with the existing visit data.
        /// </summary>
        private void PopulateCheckUpData()
        {
            weightTextBox.Text = _visit.Weight?.ToString();
            heightTextBox.Text = _visit.Height?.ToString();
            bpSystolicTextBox.Text = _visit.BloodPressureSystolic?.ToString();
            bpDiastolicTextBox.Text = _visit.BloodPressureDiastolic?.ToString();
            temperatureTextBox.Text = _visit.Temperature?.ToString();
            pulseTextBox.Text = _visit.Pulse?.ToString();
            symptomsTextBox.Text = _visit.Symptoms;
        }

        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                // Update UI bindings
                weightTextBox.IsReadOnly = value;
                heightTextBox.IsReadOnly = value;
                bpSystolicTextBox.IsReadOnly = value;
                bpDiastolicTextBox.IsReadOnly = value;
                temperatureTextBox.IsReadOnly = value;
                pulseTextBox.IsReadOnly = value;
                symptomsTextBox.IsReadOnly = value;
                saveButton.IsEnabled = !value;
            }
        }

        /// <summary>
        /// Saves the check-up data back to the visit.
        /// </summary>
        private void SaveCheckUp_Click(object sender, RoutedEventArgs e)
        {
            bool hasError = false;

            // Reset all error labels
            weightErrorLabel.Visibility = Visibility.Collapsed;
            heightErrorLabel.Visibility = Visibility.Collapsed;
            bpSystolicErrorLabel.Visibility = Visibility.Collapsed;
            bpDiastolicErrorLabel.Visibility = Visibility.Collapsed;
            temperatureErrorLabel.Visibility = Visibility.Collapsed;
            pulseErrorLabel.Visibility = Visibility.Collapsed;

            // Validate and assign Weight
            if (string.IsNullOrWhiteSpace(weightTextBox.Text))
            {
                weightErrorLabel.Content = "Weight is required.";
                weightErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else if (!decimal.TryParse(weightTextBox.Text, out decimal weight))
            {
                weightErrorLabel.Content = "Invalid weight. Please enter a numeric value.";
                weightErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                _visit.Weight = weight;
            }

            // Validate and assign Height
            if (string.IsNullOrWhiteSpace(heightTextBox.Text))
            {
                heightErrorLabel.Content = "Height is required.";
                heightErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else if (!decimal.TryParse(heightTextBox.Text, out decimal height))
            {
                heightErrorLabel.Content = "Invalid height. Please enter a numeric value.";
                heightErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                _visit.Height = height;
            }

            // Validate and assign BloodPressureSystolic
            if (string.IsNullOrWhiteSpace(bpSystolicTextBox.Text))
            {
                bpSystolicErrorLabel.Content = "Systolic blood pressure is required.";
                bpSystolicErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else if (!int.TryParse(bpSystolicTextBox.Text, out int bpSystolic))
            {
                bpSystolicErrorLabel.Content = "Invalid systolic pressure. Enter a numeric value.";
                bpSystolicErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                _visit.BloodPressureSystolic = bpSystolic;
            }

            // Validate and assign BloodPressureDiastolic
            if (string.IsNullOrWhiteSpace(bpDiastolicTextBox.Text))
            {
                bpDiastolicErrorLabel.Content = "Diastolic blood pressure is required.";
                bpDiastolicErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else if (!int.TryParse(bpDiastolicTextBox.Text, out int bpDiastolic))
            {
                bpDiastolicErrorLabel.Content = "Invalid diastolic pressure. Enter a numeric value.";
                bpDiastolicErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                _visit.BloodPressureDiastolic = bpDiastolic;
            }

            // Validate and assign Temperature
            if (string.IsNullOrWhiteSpace(temperatureTextBox.Text))
            {
                temperatureErrorLabel.Content = "Temperature is required.";
                temperatureErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else if (!decimal.TryParse(temperatureTextBox.Text, out decimal temperature))
            {
                temperatureErrorLabel.Content = "Invalid temperature. Enter a numeric value.";
                temperatureErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                _visit.Temperature = temperature;
            }

            // Validate and assign Pulse
            if (string.IsNullOrWhiteSpace(pulseTextBox.Text))
            {
                pulseErrorLabel.Content = "Pulse is required.";
                pulseErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else if (!int.TryParse(pulseTextBox.Text, out int pulse))
            {
                pulseErrorLabel.Content = "Invalid pulse. Please enter a numeric value.";
                pulseErrorLabel.Visibility = Visibility.Visible;
                hasError = true;
            }
            else
            {
                _visit.Pulse = pulse;
            }

            // Stop processing if any errors are present
            if (hasError)
            {
                return;
            }

            // Assign Symptoms (no validation required for symptoms as it is optional)
            _visit.Symptoms = symptomsTextBox.Text;

            // Save the updated visit to the database
            try
            {
                VisitDal.UpdateVisit(_visit);

                IsReadOnly = _visit.VisitStatus == "Completed" || !string.IsNullOrWhiteSpace(_visit.FinalDiagnosis);

                MessageBox.Show("Check-up data saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // Show an error message if there is a problem during saving
                MessageBox.Show($"An error occurred while saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        /// <summary>
        /// Closes the window when the cancel button is clicked.
        /// </summary>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
