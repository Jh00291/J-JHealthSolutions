using System.Windows;
using System.Windows.Controls;

namespace J_JHealthSolutions.Views
{
    public partial class VisitControl : UserControl
    {
        public VisitControl()
        {
            InitializeComponent();
            LoadVisits();
        }

        private void VisitsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditButton.IsEnabled = VisitsDataGrid.SelectedItem != null;
            DeleteButton.IsEnabled = VisitsDataGrid.SelectedItem != null;
        }

        private void LoadVisits()
        {
            // Load visit data from the DAL (assuming a VisitDal class is available)
            var visits = /* VisitDal */.GetVisits();
            VisitsDataGrid.ItemsSource = visits;
        }

        private void AddVisit_Click(object sender, RoutedEventArgs e)
        {
            // Open a new window to add a visit
            var addVisitWindow = new AddEditVisitWindow();
            bool? dialogResult = addVisitWindow.ShowDialog();

            if (dialogResult == true)
            {
                LoadVisits();
            }
        }

        private void EditVisit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedVisit != null)
            {
                var editVisitWindow = new AddEditVisitWindow(SelectedVisit);
                bool? dialogResult = editVisitWindow.ShowDialog();

                if (dialogResult == true)
                {
                    LoadVisits();
                }
            }
            else
            {
                MessageBox.Show("Please select a visit to edit.");
            }
        }

        private void DeleteVisit_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedVisit != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete visit ID {SelectedVisit.VisitId}?",
                    "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    // Delete the visit through the DAL
                    /* VisitDal */.DeleteVisit(SelectedVisit.VisitId);
                    MessageBox.Show("Visit deleted successfully.");
                    LoadVisits();
                }
            }
            else
            {
                MessageBox.Show("Please select a visit to delete.");
            }
        }

        private Visit SelectedVisit
        {
            get
            {
                return (Visit)VisitsDataGrid.SelectedItem;
            }
        }
    }
}

