using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.Wpf
{
    public class LogInControlViewModel : ViewModelBase
    {
        public string Alias
        {
            get { return this._alias; }
            set
            {
                if (this._alias == value)
                { return; }

                this._alias = value;
                this.NotifyOfPropertyChange(() => this.Alias);
            }
        }
        private string _alias = string.Empty;

        public string Password
        {
            get { return this._password; }
            set
            {
                if (this._password == value)
                { return; }

                this._password = value;
                this.NotifyOfPropertyChange(() => this.Password);
            }
        }
        private string _password = string.Empty;

        public ICommand LogInCommand
        {
            get { return this._logInCommand; }
            set
            {
                if (this._logInCommand == value)
                { return; }

                this._logInCommand = value;
                this.NotifyOfPropertyChange(() => this.LogInCommand);
            }
        }
        private ICommand _logInCommand;

        private readonly MainWindowViewModel _mainViewModel;

        private LogInControlViewModel() { }

        public LogInControlViewModel(MainWindowViewModel mainViewModel, ICommand logInCommand)
        {
            this.Alias = "user";

            this._mainViewModel = mainViewModel;
            this.LogInCommand = logInCommand;
        }
    }
}
