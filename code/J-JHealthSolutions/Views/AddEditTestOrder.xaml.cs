using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using J_JHealthSolutions.DAL;
using J_JHealthSolutions.Model;
using J_JHealthSolutions.ViewModel;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for AddEditTest.xaml
    /// </summary>
    public partial class AddEditTestOrder : Window
    {
        private TestOrder _selectedTestOrder;
        private Test _selectedTest;
        private UnitOfMeasure _selectedTestUnitOfMeasure;

        public AddEditTestOrder()
        {
            InitializeComponent();
            this.DataContext = new AddEditTestOrderViewModel();
        }

        public AddEditTestOrder(Visit currentVisit)
        {
            InitializeComponent();
            testOrderedByTextBox.Text = DoctorDal.GetDoctor(currentVisit.DoctorId).ToString();
            this.DataContext = new AddEditTestOrderViewModel(currentVisit);
        }

        public AddEditTestOrder(TestOrder test, Doctor visitDoctor)
        {
            InitializeComponent();
            _selectedTestOrder = test;
            LoadTest();
            testOrderedByTextBox.Text = visitDoctor.ToString();
            this.DataContext = new AddEditTestOrderViewModel();
        }

        private void LoadTest()
        {
            if (_selectedTestOrder != null)
            {
                testComboBox.SelectedItem = _selectedTestOrder;
            }
        }


        private void UpdateLabelUOM()
        {
            var converter = new EnumDescriptionConverter();
            this.unitTextLabel.Content = converter.Convert(_selectedTest.Unit, typeof(string), null, CultureInfo.InvariantCulture);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AddEditTestOrderViewModel;
            if (viewModel != null)
            {
                if (viewModel.Save())
                {
                    // Close the window or navigate away
                    this.Close();
                }
                else
                {
                    // Errors will be displayed; no need to do anything else
                }
            }
        }

    }
}
