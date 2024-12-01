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

            // Instantiate the ViewModel for adding a new patient
            var viewModel = new AddEditPatientViewModel();

            // Set the DataContext to the ViewModel
            this.DataContext = viewModel;

            // Assign the CloseAction to handle window closing from the ViewModel
            viewModel.CloseAction = new Action<bool?>(this.CloseWindow);
            this.Title = "Add New Patient";
        }

        public AddEditPatientWindow(Patient patient)
        {
            InitializeComponent();

            // Instantiate the ViewModel for editing an existing patient
            var viewModel = new AddEditPatientViewModel(patient);

            // Set the DataContext to the ViewModel
            this.DataContext = viewModel;

            // Assign the CloseAction to handle window closing from the ViewModel
            viewModel.CloseAction = new Action<bool?>(this.CloseWindow);
            this.Title = "Edit Patient: " + patient.FName + " " + patient.LName;
        }

        /// <summary>
        /// Method to close the window with an optional dialog result.
        /// </summary>
        /// <param name="dialogResult">The dialog result to set before closing.</param>
        private void CloseWindow(bool? dialogResult)
        {
            // Set the DialogResult if it's not null
            if (dialogResult.HasValue)
            {
                this.DialogResult = dialogResult;
            }

            // Close the window
            this.Close();
        }
    }
}
