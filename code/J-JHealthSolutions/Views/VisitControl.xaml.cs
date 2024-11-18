using J_JHealthSolutions.Model;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.DAL;

namespace J_JHealthSolutions.Views
{
    public partial class VisitControl : UserControl
    {
        public VisitControl()
        {
            InitializeComponent();
            LoadVisits();
            VisitsDataGrid.SelectionChanged += VisitsDataGrid_SelectionChanged;
            CheckUpButton.IsEnabled = false; // Disable Check-Up button by default
        }

        private void VisitsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = VisitsDataGrid.SelectedItem != null;
            CheckUpButton.IsEnabled = VisitsDataGrid.SelectedItem != null;
        }

        private void LoadVisits()
        {
            VisitDal da = new VisitDal();
            var visits = da.GetVisits();
            VisitsDataGrid.ItemsSource = visits;
        }

        private Visit SelectedVisit
        {
            get
            {
                return (Visit)VisitsDataGrid.SelectedItem;
            }
        }

        private void CheckUp_Click(object sender, RoutedEventArgs e)
        {
            if (VisitsDataGrid.SelectedItem is Visit selectedVisit)
            {
                // Pass the selected visit to the CheckUpWindow
                var checkUpWindow = new CheckUpWindow(selectedVisit);
                checkUpWindow.ShowDialog();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedVisit = VisitsDataGrid.SelectedItem as Visit;
            if (selectedVisit != null)
            {
                var editVisitWindow = new EditVisit(selectedVisit);
                bool? dialogResult = editVisitWindow.ShowDialog();
                if (dialogResult == true)
                {
                    LoadVisits(); // Refresh DataGrid
                }
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var patientName = SearchPatientName.Text;
            var dob = SearchDOB.SelectedDate;

            VisitDal visitDal = new VisitDal();
            var visits = visitDal.SearchVisits(patientName, dob);
            VisitsDataGrid.ItemsSource = visits;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            SearchPatientName.Clear();
            SearchDOB.SelectedDate = null;
            LoadVisits();
        }
    }
}

