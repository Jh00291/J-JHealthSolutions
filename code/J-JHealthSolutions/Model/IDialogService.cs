using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.Model
{
    public interface IDialogService
    {
        bool ShowConfirmation(string message, string caption);
        bool ShowConfirmationDialog(string title, string message);
        void ShowMessage(string message, string caption);
    }

}
