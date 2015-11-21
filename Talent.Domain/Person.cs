using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;

namespace Talent.Domain
{
    public class Person : AggregateRootBase
    {
        #region Constructor

        #endregion

        #region Fields

        private int _personId;
        private string _salutation = String.Empty;
        private string _firstName = String.Empty;
        private string _middleName = String.Empty;
        private string _lastName = String.Empty;
        private string _suffix = String.Empty;
        private string _stageName = String.Empty;
        private DateTime? _dateOfBirth;
        private double? _weight;
        private double? _height;
        private int? _hairColorId;
        private int? _eyeColorId;
        private List<Credit> _credits = new List<Credit>();
        private ObservableCollection<PersonAttachment> _attachments = new ObservableCollection<PersonAttachment>();

        #endregion

        #region Properties

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

        public string Salutation
        {
            get { return _salutation; }
            set
            {
                var val = value ?? String.Empty;
                if (_salutation == val) return;
                _salutation = val;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                var val = value ?? String.Empty;
                if (_firstName == val) return;
                _firstName = val;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
                OnPropertyChanged("FirstLastName");
                OnPropertyChanged("LastFirstName");
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                var val = value ?? String.Empty;
                if (_middleName == val) return;
                _middleName = val;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                var val = value ?? String.Empty;
                if (_lastName == val) return;
                _lastName = val;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
                OnPropertyChanged("FirstLastName");
                OnPropertyChanged("LastFirstName");
            }
        }

        public string Suffix
        {
            get { return _suffix; }
            set
            {
                var val = value ?? String.Empty;
                if (_suffix == val) return;
                _suffix = val;
                OnPropertyChanged();
                OnPropertyChanged("FullName");
            }
        }

        public string StageName
        {
            get { return _stageName; }
            set
            {
                var val = value ?? String.Empty;
                if (_stageName == val) return;
                _stageName = val;
                OnPropertyChanged();
            }
        }

        public DateTime? DateOfBirth
        {
            get { return _dateOfBirth; }
            set
            {
                if (_dateOfBirth == value) return;
                _dateOfBirth = value;
                OnPropertyChanged();
                OnPropertyChanged("Age");
            }
        }

        public double? Weight
        {
            get { return _weight; }
            set
            {
                if (_weight == value) return;
                _weight = value;
                OnPropertyChanged();
            }
        }

        public double? Height
        {
            get { return _height; }
            set
            {
                if (_height == value) return;
                _height = value;
                OnPropertyChanged();
            }
        }

        public int? HairColorId
        {
            get { return _hairColorId; }
            set
            {
                if (_hairColorId == value) return;
                _hairColorId = value;
                OnPropertyChanged();
            }
        }

        public int? EyeColorId
        {
            get { return _eyeColorId; }
            set
            {
                if (_eyeColorId == value) return;
                _eyeColorId = value;
                OnPropertyChanged();
            }
        }

        public List<Credit> Credits
        {
            get { return _credits; }
        }

        public ObservableCollection<PersonAttachment> Attachments
        {
            get { return _attachments; }
        }

        #endregion

        #region Computed Properties

        public string FirstLastName
        {
            get
            {
                return (FirstName + " " + LastName).Trim();
            }
        }

        public string LastFirstName
        {
            get
            {
                var sb = new StringBuilder();
                var joinCharacter = "";

                if (!String.IsNullOrWhiteSpace(LastName))
                {
                    sb.Append(joinCharacter + LastName);
                    joinCharacter = ", ";
                }
                if (!String.IsNullOrWhiteSpace(FirstName))
                {
                    sb.Append(joinCharacter + FirstName);
                }
                return sb.ToString();
            }
        }

        public string FullName
        {
            get
            {
                var sb = new StringBuilder();
                var joinCharacter = "";

                if (!String.IsNullOrWhiteSpace(Salutation))
                {
                    sb.Append(joinCharacter + Salutation);
                    joinCharacter = " ";
                }
                if (!String.IsNullOrWhiteSpace(FirstName))
                {
                    sb.Append(joinCharacter + FirstName);
                    joinCharacter = " ";
                }
                if (!String.IsNullOrWhiteSpace(MiddleName))
                {
                    sb.Append(joinCharacter + MiddleName);
                    joinCharacter = " ";
                }
                if (!String.IsNullOrWhiteSpace(LastName))
                {
                    sb.Append(joinCharacter + LastName);
                    joinCharacter = " ";
                }
                if (!String.IsNullOrWhiteSpace(Suffix))
                {
                    if (sb.Length > 0) joinCharacter = ", ";
                    sb.Append(joinCharacter + Suffix);
                    joinCharacter = " ";
                }
                return sb.ToString();
            }
        }

        public int? Age
        {
            get
            {
                if (DateOfBirth.HasValue == false) return null;
                var today = DateTime.Today;
                int years = today.Year - DateOfBirth.Value.Year;
                if (
                    (DateOfBirth.Value.Date.Month > today.Month)
                    || (DateOfBirth.Value.Date.Month == today.Month
                        && DateOfBirth.Value.Date.Day > today.Day))
                {
                    years--;
                }
                return years >= 0 ? years : 0;
            }
        }
        #endregion

        #region Overrides

        public override string ToString()
        {
            return FirstLastName;
        }

        protected override bool GetGraphDirty()
        {
            return base.GetGraphDirty() ||
                this.Credits.Any(o => o.IsDirty
                    || (o.CreditId > 0 && o.IsMarkedForDeletion)
                    || (o.CreditId == 0 && !o.IsMarkedForDeletion))
                    || this.Attachments.Any(o => o.IsDirty
                    || (o.PersonAttachmentId > 0 && o.IsMarkedForDeletion)
                    || (o.PersonAttachmentId == 0 && !o.IsMarkedForDeletion));
        }

        public override string Validate(string propertyName = null)
        {
            List<string> errors = new List<string>();
            string err;
            switch (propertyName)
            {
                case "FirstName":
                    if (String.IsNullOrEmpty(FirstName))
                        errors.Add("First name is required.");
                    if (FirstName != null && FirstName.Length > 50)
                        errors.Add("First name cannot exceed 50 characters");
                    break;
                case "LastName":
                    if (String.IsNullOrEmpty(LastName))
                        errors.Add("Last name is required.");
                    if (LastName != null && LastName.Length > 50)
                        errors.Add("Last name cannot exceed 50 characters");
                    break;
                case "DateOfBirth":
                    if (DateOfBirth.HasValue
                        && DateOfBirth > DateTime.Today)
                        errors.Add("Date of Birth cannot be in the future");
                    break;
                case "Weight":
                    if (Weight.HasValue
                        && Weight < 0.0)
                        errors.Add("Weight cannot be negative");
                    break;
                case "Height":
                    if (Height.HasValue
                        && Height < 0)
                        errors.Add("Height Cannot be negative");
                    break;
                case "Credits":
                    foreach (var c in Credits)
                    {
                        err = c.Validate();
                        if (err != null) errors.Add(err);
                    }
                    break;
                case null:
                    err = Validate("FirstName");
                    if (err != null) errors.Add(err);

                    err = Validate("LastName");
                    if (err != null) errors.Add(err);

                    err = Validate("DateOfBirth");
                    if (err != null) errors.Add(err);

                    err = Validate("Weight");
                    if (err != null) errors.Add(err);

                    err = Validate("Height");
                    if (err != null) errors.Add(err);

                    err = Validate("Cast");
                    if (err != null) errors.Add(err);

                    break;
                default:
                    return null;
            }
            return errors.Count == 0 ? null : String.Join("\r\n", errors);
        }

        #endregion
    }
}
