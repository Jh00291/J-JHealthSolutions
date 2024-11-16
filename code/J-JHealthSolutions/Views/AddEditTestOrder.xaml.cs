﻿using System;
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
            LoadTests();
        }

        private void LoadTests()
        {
            this.testComboBox.ItemsSource = TestDal.GetTests();
        }

        public AddEditTestOrder(TestOrder test)
        {
            InitializeComponent();
            _selectedTestOrder = test;
            LoadTest();
        }

        private void LoadTest()
        {
            if (_selectedTestOrder != null)
            {
                testComboBox.SelectedItem = _selectedTestOrder;
            }
        }


        private void TestComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (testComboBox.SelectedItem != null)
            {
                _selectedTest = (Test)testComboBox.SelectedItem;
                UpdateLabelUOM();
            }
        }

        private void UpdateLabelUOM()
        {
            var converter = new EnumDescriptionConverter();
            this.unitTextLabel.Content = converter.Convert(_selectedTest.Unit, typeof(string), null, CultureInfo.InvariantCulture);
        }
    }
}
