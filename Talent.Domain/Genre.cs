﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Talent.Domain
{
    public class Genre : AggregateRootBase
    {
        #region Constructor

        #endregion

        #region Fields

        private int _genreId;
        private string _name = String.Empty;
        private string _code = String.Empty;
        private bool _isInactive;
        private int _displayOrder = 10;

        #endregion

        #region Properties

        public int GenreId
        {
            get { return _genreId; }
            set
            {
                if (_genreId == value) return;
                _genreId = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Please enter a Name.")]
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

        [Required(ErrorMessage = "Please enter a Code.")]
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

        public override string Validate(string propertyName = null)
        {
            List<string> errors = new List<string>();
            string err;
            switch (propertyName)
            {
                case "Code":
                    if (String.IsNullOrEmpty(Code))
                        errors.Add("Code is required.");
                    if (Code != null && Code.Length > 20)
                        errors.Add("Code cannot exceed 50 characters");
                    break;
                case "Name":
                    if (String.IsNullOrEmpty(Name))
                        errors.Add("Name is required.");
                    if (Name != null && Name.Length > 50)
                        errors.Add("Name cannot exceed 50 characters");
                    break;
                case null:
                    err = Validate("Code");
                    if (err != null) errors.Add(err);

                    err = Validate("Name");
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
