using DevExpress.Xpf.Core;
using Expenses.Model;
using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Expenses.Wpf
{
    public class ViewService : ViewModelBase, IViewService
    {
        private bool _isBusy;

        public event EventHandler<EventArgs<bool>> BusyChanged;

        public void ShowBusy(bool isBusy)
        {
            if (this._isBusy == isBusy) { return; }

            this._isBusy = isBusy;

            EventHandler<EventArgs<bool>> handler = this.BusyChanged;
            if (handler != null)
            {
                handler(this, new EventArgs<bool>(this._isBusy));
            }
        }
        
        public void ShowError(string message)
        {
            MessageBox.Show(message);
        }

        public Task<bool> ConfirmAsync(string title, string message)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            tcs.SetResult(DXMessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK);

            return tcs.Task;
        }

        async Task IViewService.ExecuteBusyActionAsync(Func<Task> func)
        {
            this.ShowBusy(true);
            try
            {
                await func();
            }
            catch (Exception e)
            {
                this.ShowError(e.ToString());
            }
            finally
            {
                this.ShowBusy(false);
            }
        }
    }
}
