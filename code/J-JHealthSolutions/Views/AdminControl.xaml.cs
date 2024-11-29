using ICSharpCode.AvalonEdit.CodeCompletion;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for AdminControl.xaml
    /// </summary>
    public partial class AdminControl : UserControl
    {
        private CompletionWindow _completionWindow;

        // ViewModel reference
        private readonly AdminViewModel _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminControl"/> class.
        /// </summary>
        public AdminControl()
        {
            InitializeComponent();

            // Initialize the ViewModel
            _viewModel = new AdminViewModel();
            DataContext = _viewModel;
        }

        // Optionally handle UI interactions directly (if required)
        private void ExecuteQueryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Trigger query execution via ViewModel command
                if (_viewModel.ExecuteQueryCommand.CanExecute(null))
                {
                    _viewModel.ExecuteQueryCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                // Show error message
                MessageBox.Show($"Query execution failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
