using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Model
{
    public interface IViewService
    {
        event EventHandler<EventArgs<bool>> BusyChanged;

        void ShowBusy(bool isBusy);

        void ShowError(string message);

        Task<bool> ConfirmAsync(string title, string message);
        
        Task ExecuteBusyActionAsync(Func<Task> func);        
    }
}
