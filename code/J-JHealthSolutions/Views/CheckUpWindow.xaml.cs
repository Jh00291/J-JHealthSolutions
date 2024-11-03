using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace J_JHealthSolutions.Views
{
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
        try
        {
            // Save updated check-up values to the visit instance
            _visit.Weight = string.IsNullOrWhiteSpace(weightTextBox.Text) ? (decimal?)null : Convert.ToDecimal(weightTextBox.Text);
            _visit.Height = string.IsNullOrWhiteSpace(heightTextBox.Text) ? (decimal?)null : Convert.ToDecimal(heightTextBox.Text);
            _visit.BloodPressureSystolic = string.IsNullOrWhiteSpace(bpSystolicTextBox.Text) ? (int?)null : Convert.ToInt32(bpSystolicTextBox.Text);
            _visit.BloodPressureDiastolic = string.IsNullOrWhiteSpace(bpDiastolicTextBox.Text) ? (int?)null : Convert.ToInt32(bpDiastolicTextBox.Text);
            _visit.Temperature = string.IsNullOrWhiteSpace(temperatureTextBox.Text) ? (decimal?)null : Convert.ToDecimal(temperatureTextBox.Text);
            _visit.Pulse = string.IsNullOrWhiteSpace(pulseTextBox.Text) ? (int?)null : Convert.ToInt32(pulseTextBox.Text);
            _visit.Symptoms = symptomsTextBox.Text;

            // Save the updated visit to the database
            _visitDal.UpdateVisit(_visit);

            MessageBox.Show("Check-up data saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }
        catch (FormatException ex)
        {
            MessageBox.Show($"Invalid data format: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
