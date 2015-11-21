using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ucla.Common.BaseClasses
{
    /// <summary>
    /// Domain Object Classes should inherit from this class to inherit the IsDirty
    /// and IsMarkedForDeletion change-tracking properties, as well as the 
    /// INotifyPropertyChanged interface for databinding and updating the
    /// IsDirty property.
    /// </summary>
    public abstract class DomainBase : INotifyPropertyChanged, IDataErrorInfo
    {
        private bool _isDirty = true;
        private bool _isMarkedForDeletion = false;

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty == value) return;
                _isDirty = value;
                OnPropertyChanged();
            }
        }

        public bool IsMarkedForDeletion
        {
            get { return _isMarkedForDeletion; }
            set
            {
                if (_isMarkedForDeletion == value) return;
                _isMarkedForDeletion = value;
                OnPropertyChanged("IsMarkedForDeletion");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(
            [CallerMemberNameAttribute] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            if (propertyName != "IsDirty"
                && propertyName != "IsMarkedForDeletion")
            {
                IsDirty = true;
            }
        }

        #endregion

        #region IDataErrorInfo Members

        public string Error
        {
            get { return Validate(); }
        }

        public string this[string columnName]
        {
            get { return Validate(columnName); }
        }

        /// <summary>
        /// Override in derived class to return validation error message
        /// </summary>
        /// <param name="columnName">property name, or null to validate entire object.</param>
        /// <returns>null if property or object is valid.</returns>
        public virtual string Validate(string columnName = null)
        {
            return null;
        }

        #endregion
    }
}
