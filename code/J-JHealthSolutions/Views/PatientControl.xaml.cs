using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace J_JHealthSolutions.Views
{
    public partial class PatientControl : UserControl
    {
        private PatientDal _patientDal = new PatientDal();

        public PatientControl()
        {
            InitializeComponent();
        }

        // Add Patient
        private void AddPatient_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Edit Patient
        private void EditPatient_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Delete Patient
        private void DeletePatient_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
