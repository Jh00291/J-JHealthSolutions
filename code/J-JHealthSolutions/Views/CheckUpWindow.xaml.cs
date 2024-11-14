using System.Windows;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.Views
{
    public partial class CheckUpWindow : Window {

    /// <summary>
    /// Interaction logic for CheckupWindow.xaml
    /// </summary>
    private Visit _visit;
    private VisitDal _visitDal = new VisitDal();

    /// <summary>
    /// Initializes a new instance of CheckUpWindow with the selected visit details.
    /// </summary>
    /// <param name="visit">The visit to be displayed in the check-up window</param>
    public CheckUpWindow(Visit visit)
    {
        InitializeComponent();
        _visit = visit;
        PopulateCheckUpData();
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
            if (!string.IsNullOrWhiteSpace(weightTextBox.Text))
            {
                if (decimal.TryParse(weightTextBox.Text, out decimal weight))
                {
                    _visit.Weight = weight;
                }
                else
                {
                    weightErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }
            else
            {
                _visit.Weight = null;
            }

            // Validate and assign Height
            if (!string.IsNullOrWhiteSpace(heightTextBox.Text))
            {
                if (decimal.TryParse(heightTextBox.Text, out decimal height))
                {
                    _visit.Height = height;
                }
                else
                {
                    heightErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }
            else
            {
                _visit.Height = null;
            }

            // Validate and assign BloodPressureSystolic
            if (!string.IsNullOrWhiteSpace(bpSystolicTextBox.Text))
            {
                if (int.TryParse(bpSystolicTextBox.Text, out int bpSystolic))
                {
                    _visit.BloodPressureSystolic = bpSystolic;
                }
                else
                {
                    bpSystolicErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }
            else
            {
                _visit.BloodPressureSystolic = null;
            }

            // Validate and assign BloodPressureDiastolic
            if (!string.IsNullOrWhiteSpace(bpDiastolicTextBox.Text))
            {
                if (int.TryParse(bpDiastolicTextBox.Text, out int bpDiastolic))
                {
                    _visit.BloodPressureDiastolic = bpDiastolic;
                }
                else
                {
                    bpDiastolicErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }
            else
            {
                _visit.BloodPressureDiastolic = null;
            }

            // Validate and assign Temperature
            if (!string.IsNullOrWhiteSpace(temperatureTextBox.Text))
            {
                if (decimal.TryParse(temperatureTextBox.Text, out decimal temperature))
                {
                    _visit.Temperature = temperature;
                }
                else
                {
                    temperatureErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }
            else
            {
                _visit.Temperature = null;
            }

            // Validate and assign Pulse
            if (!string.IsNullOrWhiteSpace(pulseTextBox.Text))
            {
                if (int.TryParse(pulseTextBox.Text, out int pulse))
                {
                    _visit.Pulse = pulse;
                }
                else
                {
                    pulseErrorLabel.Visibility = Visibility.Visible;
                    hasError = true;
                }
            }
            else
            {
                _visit.Pulse = null;
            }

            // Stop processing if any errors are present
            if (hasError)
            {
                return;
            }

            // Assign Symptoms
            _visit.Symptoms = symptomsTextBox.Text;

            // Save the updated visit to the database
            try
            {
                _visitDal.UpdateVisit(_visit);
                MessageBox.Show("Check-up data saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
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
