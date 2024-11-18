using System;
using System.Collections.Generic;
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
using J_JHealthSolutions.ViewModels;

namespace J_JHealthSolutions.Views
{
    /// <summary>
    /// Interaction logic for EditVisit.xaml
    /// </summary>
    public partial class EditVisit : Window
    {
        private Visit _visit;

        public EditVisit()
        {
            InitializeComponent();
        }

        public EditVisit(Visit visit)
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
