using System;
using System.Windows;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.ViewModel;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for AddEditTestOrder.xaml
    /// </summary>
    public partial class AddEditTestOrder : Window
    {
        public AddEditTestOrder()
        {
            InitializeComponent();

            var dialogService = new DialogService();
            var viewModel = new AddEditTestOrderViewModel(dialogService);
            this.DataContext = viewModel;

            viewModel.OnTestOrderSaved += ViewModel_OnTestOrderSaved;
        }

        public AddEditTestOrder(Visit currentVisit)
        {
            InitializeComponent();

            var dialogService = new DialogService();
            var viewModel = new AddEditTestOrderViewModel(currentVisit, dialogService);
            this.DataContext = viewModel;

            viewModel.OnTestOrderSaved += ViewModel_OnTestOrderSaved;
            viewModel.OnCancelRequested += ViewModel_OnCancelRequested;
        }

        private void ViewModel_OnCancelRequested(object? sender, EventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public AddEditTestOrder(TestOrder testOrder, Visit currentVisit)
        {
            InitializeComponent();
            var dialogService = new DialogService();
            var viewModel = new AddEditTestOrderViewModel(testOrder, currentVisit, dialogService);
            this.DataContext = viewModel;

            viewModel.OnTestOrderSaved += ViewModel_OnTestOrderSaved;
            viewModel.OnCancelRequested += ViewModel_OnCancelRequested;
        }

        private void ViewModel_OnTestOrderSaved(object sender, EventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}