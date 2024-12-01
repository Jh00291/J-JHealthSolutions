using System.Windows;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.ViewModels;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for EditVisit.xaml
    /// </summary>
    public partial class EditVisitWindow : Window
    {
        private Visit _visit;

        public EditVisitWindow()
        {
            InitializeComponent();
        }

        public EditVisitWindow(Visit visit)
        {
            InitializeComponent();
            var dialogService = new DialogService();
            var viewModel = new EditVisitViewModel(visit, dialogService);
            viewModel.RequestClose += (s, e) => this.Close();
            viewModel.VisitUpdated += (s, e) =>
            {
                this.DialogResult = true;
            };

            this.DataContext = viewModel;
        }
    }
}
