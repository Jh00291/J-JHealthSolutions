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

        public AdminControl()
        {
            InitializeComponent();
        }

    }
}
