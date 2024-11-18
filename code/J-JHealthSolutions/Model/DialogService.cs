using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace J_JHealthSolutions.Model
{
    public class DialogService : IDialogService
    {
        public bool ShowConfirmation(string message, string caption)
        {
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            return result == MessageBoxResult.Yes;
        }

        public bool ShowConfirmationDialog(string title, string message)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
