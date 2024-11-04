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
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for EditVisit.xaml
    /// </summary>
    public partial class EditVisit : Window
    {
        private Visit _visit;

        public EditVisit()
        {
            InitializeComponent();
        }

        public EditVisit(Visit visit)
        {
            InitializeComponent();
            List<string> statuses = new List<string> { "Completed", "InProgress", "Pending" };
            statusComboBox.ItemsSource = statuses;
            _visit = visit;
            PopulateVisitData(visit);
        }

        private void PopulateVisitData(Visit visit)
        {
           initialDiagnosisTextbox.Text = visit.InitialDiagnosis;
           finalDiagnosisTextBox.Text = visit.FinalDiagnosis;
           statusComboBox.SelectedItem = visit.VisitStatus;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ValidateStatus();
            try
            {
                var visitDal = new VisitDal();
                _visit.InitialDiagnosis = initialDiagnosisTextbox.Text;
                _visit.FinalDiagnosis = finalDiagnosisTextBox.Text;
                _visit.VisitStatus = statusComboBox.SelectedItem.ToString();
                visitDal.UpdateVisit(_visit);
                this.DialogResult = true;
                this.Close();
            } catch (Exception ex)
            {
                MessageBox.Show($"Error saving visit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateStatus()
        {
            if (statusComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a status for the visit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
