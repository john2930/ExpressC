//*********************************************************
// Copyright (c) Microsoft. All rights reserved.
//
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

//*********************************************************
// Copyright (c) DevExpress, Inc. All rights reserved.
//
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Expenses.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged {
        #region commands
        public ICommand BackCommand { get; private set; }
        public ICommand ShowChargesCommand { get; private set; }
        public ICommand ShowSavedReportsCommand { get; private set; }
        public ICommand ShowPendingReportsCommand { get; private set; }
        public ICommand ShowPastReportsCommand { get; private set; }
        public ICommand ShowReportsForApprovalCommand { get; private set; }
        public ICommand ChangeUserCommand { get; private set; }
        #endregion
        INavigationService NavigationService { get { return ServiceLocator.Current.GetService<INavigationService>() as INavigationService; } }
        public ViewModelBase() {
            this.BackCommand = new RelayCommand(x => NavigationService.ShowSummary());
            this.ShowChargesCommand = new RelayCommand(x => NavigationService.ShowCharges());
            this.ShowSavedReportsCommand = new RelayCommand(x => NavigationService.ShowSavedExpenseReports());
            this.ShowPendingReportsCommand = new RelayCommand(x => NavigationService.ShowPendingExpenseReports());
            this.ShowPastReportsCommand = new RelayCommand(x => NavigationService.ShowPastExpenseReports());
            this.ShowReportsForApprovalCommand = new RelayCommand(x => NavigationService.ShowReportsForApproval());
            this.ChangeUserCommand = new RelayCommand(x => NavigationService.ChangeUser());
        }
        void OnBackCommandExecute(object param) {
            INavigationService navigationService = ServiceLocator.Current.GetService<INavigationService>() as INavigationService;
            navigationService.ShowSummary();            
        }
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this._propertyChanged += value; }
            remove { this._propertyChanged -= value; }
        }


        /// <summary>
        /// The private event.
        /// </summary>
        private event PropertyChangedEventHandler _propertyChanged = delegate { };


        protected void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else memberExpression = (MemberExpression)lambda.Body;

            this.NotifyOfPropertyChange(memberExpression.Member.Name);
        }

        /// <summary>
        /// Raises the property changed event for the given property.
        /// </summary>
        /// <param name="property">The property that is raising the event.</param>
        public void NotifyOfPropertyChange(string property)
        {
            this.RaisePropertyChanged(property, true);
        }

        /// <summary>
        /// Raises the property changed event for the given property.
        /// </summary>
        /// <param name="property">The property that is raising the event.</param>
        /// <param name="verifyProperty">if set to <c>true</c> the property should be verified.</param>
        private void RaisePropertyChanged(string property, bool verifyProperty)
        {
            var handler = this._propertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
