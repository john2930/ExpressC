using Expenses.ViewModel;
namespace Expenses.Wpf {    
    public class ReportsViewModel : ViewModelBase {
        #region properties
        private ReportsSubviewType _currentViewType;
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel {
            get { return this._currentViewModel; }
            protected set {
                if(this._currentViewModel == value) { return; }

                this._currentViewModel = value;
                this.NotifyOfPropertyChange(() => this.CurrentViewModel);                
            }
        }
        public ReportsSubviewType CurrentViewType {
            get { return this._currentViewType; }
            set {
                if(this._currentViewType== value) { return; }

                this._currentViewType = value;
                this.NotifyOfPropertyChange(() => this.CurrentViewType);
                this.OnCurrentViewTypeChanged();
            }
        }
        #endregion
        public ReportsViewModel() {
            OnCurrentViewTypeChanged();
        }
        private void OnCurrentViewTypeChanged() {
            switch(CurrentViewType) {
                case ReportsSubviewType.SavedReports:
                case ReportsSubviewType.PendingReports:
                case ReportsSubviewType.PastReports:                
                    CurrentViewModel = new ExpenseReportsViewModel();
                    break;
                case ReportsSubviewType.ApprovalsReports:
                    CurrentViewModel = new ApproveExpenseReportsViewModel();
                    break;
                case ReportsSubviewType.OutgoingCharges:
                    CurrentViewModel = new ChargesViewModel();
                    break;
            }
            
        }
    }
    public enum ReportsSubviewType { OutgoingCharges, SavedReports, PendingReports, PastReports, ApprovalsReports }
}