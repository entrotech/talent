using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;

namespace Talent.Domain
{
    public class Credit : DomainBase
    {
        #region Fields

        private int _creditId;
        private int _showId;
        private int _personId;
        private int _creditTypeId;
        private string _character = String.Empty;

        #endregion

        #region Properties

        public int CreditId
        {
            get { return _creditId; }
            set
            {
                if (_creditId == value) return;
                _creditId = value;
                OnPropertyChanged();
            }
        }

        public int ShowId
        {
            get { return _showId; }
            set
            {
                if (_showId == value) return;
                _showId = value;
                OnPropertyChanged();
            }
        }

        public int PersonId
        {
            get { return _personId; }
            set
            {
                if (_personId == value) return;
                _personId = value;
                OnPropertyChanged();
            }
        }

        public int CreditTypeId
        {
            get { return _creditTypeId; }
            set
            {
                if (_creditTypeId == value) return;
                _creditTypeId = value;
                OnPropertyChanged();
            }
        }

        public string Character
        {
            get { return _character; }
            set
            {
                var val = value ?? String.Empty;
                if (_character == val) return;
                _character = val;
                OnPropertyChanged();
            }
        }
        
        #endregion

        #region Overrides

        public override string Validate(string columnName = null)
        {
            List<string> errors = new List<string>();
            string err;
            switch (columnName)
            {
                case "CreditTypeId":
                    if (CreditTypeId == 0)
                        errors.Add("You must select a Credit Type");
                    break;
                case "PersonId":
                    if (PersonId == 0 && ShowId == 0)
                        errors.Add("You must select a Person");
                    break;
                case "ShowId":
                    if (ShowId == 0 && PersonId == 0)
                        errors.Add("You must select a Show");
                    break;
                case "Character":
                    if (Character.Length > 50)
                        errors.Add("Character name cannot exceed 50 characters");
                    break;
                case null:
                    err = Validate("CreditTypeId");
                    if (err != null) errors.Add(err);

                    err = Validate("PersonId");
                    if (err != null) errors.Add(err);

                    err = Validate("ShowId");
                    if (err != null) errors.Add(err);

                    break;
            }
            return errors.Count == 0 ? null : String.Join("\r\n", errors);
        }

        #endregion
    }
}
