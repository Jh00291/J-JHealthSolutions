using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using J_JHealthSolutions.ViewModels;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// A window for adding or editing a patient's information.
    /// </summary>
    public partial class AddEditPatientWindow : Window
    {
        public AddEditPatientWindow()
        {
            InitializeComponent();
            var viewModel = new AddEditPatientViewModel();
            this.DataContext = viewModel;
            viewModel.CloseAction = new Action<bool?>(this.CloseWindow);
            this.Title = "Add New Patient";
        }

        public AddEditPatientWindow(Patient patient)
        {
            InitializeComponent();
            var viewModel = new AddEditPatientViewModel(patient);
            this.DataContext = viewModel;
            viewModel.CloseAction = new Action<bool?>(this.CloseWindow);
            this.Title = "Edit Patient: " + patient.FName + " " + patient.LName;
        }

        /// <summary>
        /// Method to close the window with an optional dialog result.
        /// </summary>
        /// <param name="dialogResult">The dialog result to set before closing.</param>
        private void CloseWindow(bool? dialogResult)
        {
            if (dialogResult.HasValue)
            {
                this.DialogResult = dialogResult;
            }
            this.Close();
        }
    }
}
