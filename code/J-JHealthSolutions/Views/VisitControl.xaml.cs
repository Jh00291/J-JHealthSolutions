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
            var editVisitWindow = new EditVisit(selectedVisit);
            bool? dialogResult = editVisitWindow.ShowDialog();
            if (dialogResult == true)
            {
                LoadVisits();
            }
        }
    }
}

