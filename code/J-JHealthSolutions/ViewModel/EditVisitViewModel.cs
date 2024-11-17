﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.Views;

namespace J_JHealthSolutions.ViewModels
{
    public class EditVisitViewModel : INotifyPropertyChanged
    {
        private Visit _visit;

        public event PropertyChangedEventHandler PropertyChanged;

        public EditVisitViewModel(Visit visit)
        {
            _visit = visit ?? throw new ArgumentNullException(nameof(visit));

            // Initialize properties from the visit
            InitialDiagnosis = _visit.InitialDiagnosis;
            FinalDiagnosis = _visit.FinalDiagnosis;
            SelectedStatus = _visit.VisitStatus;

            // Initialize statuses
            Statuses = new ObservableCollection<string> { "Completed", "InProgress", "Pending" };

            // Initialize TestOrders
            TestOrders = new ObservableCollection<TestOrder>(TestOrderDal.GetTestOrdersFromVisit((int)visit.VisitId));

            // Initialize commands
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
            EditTestOrderCommand = new RelayCommand(EditTestOrder, CanEditOrDeleteTestOrder);
            DeleteTestOrderCommand = new RelayCommand(DeleteTestOrder, CanEditOrDeleteTestOrder);
            AddTestOrderCommand = new RelayCommand(AddTestOrder);
        }

        #region Properties

        private string _initialDiagnosis;
        public string InitialDiagnosis
        {
            get => _initialDiagnosis;
            set
            {
                _initialDiagnosis = value;
                OnPropertyChanged(nameof(InitialDiagnosis));
            }
        }

        private string _finalDiagnosis;
        public string FinalDiagnosis
        {
            get => _finalDiagnosis;
            set
            {
                _finalDiagnosis = value;
                OnPropertyChanged(nameof(FinalDiagnosis));
            }
        }

        public ObservableCollection<string> Statuses { get; set; }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }

        public ObservableCollection<TestOrder> TestOrders { get; set; }

        private TestOrder _selectedTestOrder;
        public TestOrder SelectedTestOrder
        {
            get => _selectedTestOrder;
            set
            {
                _selectedTestOrder = value;
                OnPropertyChanged(nameof(SelectedTestOrder));
                // Notify that CanExecute for commands might have changed
                (EditTestOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteTestOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand EditTestOrderCommand { get; }
        public ICommand DeleteTestOrderCommand { get; }
        public ICommand AddTestOrderCommand { get; }

        #endregion

        #region Command Methods

        private void Save(object parameter)
        {
            if (!ValidateStatus())
                return;

            try
            {
                var visitDal = new VisitDal();
                _visit.InitialDiagnosis = this.InitialDiagnosis;
                _visit.FinalDiagnosis = this.FinalDiagnosis;
                _visit.VisitStatus = this.SelectedStatus;
                visitDal.UpdateVisit(_visit);

                // Close the window
                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving visit: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanSave(object parameter)
        {
            // Can execute if status is selected
            return !string.IsNullOrEmpty(SelectedStatus);
        }

        private void Cancel(object parameter)
        {
            // Close the window without saving
            CloseWindow();
        }

        private void EditTestOrder(object parameter)
        {
            // Implement your logic to edit the selected test order
            // For example, open a new window to edit the TestOrder
        }

        private void DeleteTestOrder(object parameter)
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete the selected test order?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                TestOrders.Remove(SelectedTestOrder);
                TestOrderDal.DeleteTestOrder((int)SelectedTestOrder.TestOrderID);
                SelectedTestOrder = null;
            }
        }

        private void AddTestOrder(object parameter)
        {
           bool? dialogResult = new AddEditTestOrder(_visit).ShowDialog();
        }


        private bool CanEditOrDeleteTestOrder(object parameter)
        {
            // Can execute if a test order is selected
            return SelectedTestOrder != null;
        }

        #endregion

        #region Helper Methods

        private bool ValidateStatus()
        {
            if (string.IsNullOrEmpty(SelectedStatus))
            {
                MessageBox.Show("Please select a status for the visit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void CloseWindow()
        {
            // Raise the event to close the window
            RequestClose?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Event to Request Window Close

        public event EventHandler RequestClose;

        #endregion
    }
}