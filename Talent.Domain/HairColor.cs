using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;

namespace Talent.Domain
{
    public class HairColor : AggregateRootBase
    {
        #region Constructor

        #endregion

        #region Fields

        private int _hairColorId;
        private string _name = String.Empty;
        private string _code = String.Empty;
        private bool _isInactive;
        private int _displayOrder = 10;

        #endregion

        #region Properties

        public int HairColorId
        {
            get { return _hairColorId; }
            set
            {
                if (_hairColorId == value) return;
                _hairColorId = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                var val = value ?? String.Empty;
                if (_name == val) return;
                _name = val;
                OnPropertyChanged();
            }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                var val = value ?? String.Empty;
                if (_code == val) return;
                _code = val;
                OnPropertyChanged();
            }
        }

        public bool IsInactive
        {
            get { return _isInactive; }
            set
            {
                if (_isInactive == value) return;
                _isInactive = value;
                OnPropertyChanged();
            }
        }

        public int DisplayOrder
        {
            get { return _displayOrder; }
            set
            {
                if (_displayOrder == value) return;
                _displayOrder = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}

