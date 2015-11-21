using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;

namespace Talent.Domain
{
    public class Show : AggregateRootBase
    {
        #region Constructor

        #endregion

        #region Fields

        private int _showId;
        private string _title = String.Empty;
        private int? _lengthInMinutes;
        private DateTime? _theatricalReleaseDate;
        private DateTime? _dvdReleaseDate;
        private int? _mpaaRatingId;
        private List<ShowGenre> _showGenres = new List<ShowGenre>();
        private List<Credit> _credits = new List<Credit>();

        #endregion

        #region Properties

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

        public string Title
        {
            get { return _title; }
            set
            {
                var val = value ?? String.Empty;
                if (_title == val) return;
                _title = val;
                OnPropertyChanged();
            }
        }

        public int? LengthInMinutes
        {
            get { return _lengthInMinutes; }
            set 
            {
                if (_lengthInMinutes == value) return;
                _lengthInMinutes = value;
                OnPropertyChanged();
            }
        }

        [DataType(DataType.Date)]
        public DateTime? TheatricalReleaseDate
        {
            get { return _theatricalReleaseDate; }
            set
            {
                if (_theatricalReleaseDate == value) return;
                _theatricalReleaseDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime? DvdReleaseDate
        {
            get { return _dvdReleaseDate; }
            set
            {
                if (_dvdReleaseDate == value) return;
                _dvdReleaseDate = value;
                OnPropertyChanged();
            }
        }

        public int? MpaaRatingId
        {
            get { return _mpaaRatingId; }
            set
            {
                if (_mpaaRatingId == value) return;
                _mpaaRatingId = value;
                OnPropertyChanged();
            }
        }

        public List<ShowGenre> ShowGenres
        {
            get { return _showGenres; }
        }

        public List<Credit> Credits
        {
            get { return _credits; }
        }
        
        #endregion

        #region Overrides

        public override string ToString()
        {
            return Title;
        }

        protected override bool GetGraphDirty()
        {
            return base.GetGraphDirty()
                || Credits.Any(o => o.IsDirty 
                    || (o.CreditId == 0 && !o.IsMarkedForDeletion)
                    || (o.CreditId > 0 && o.IsMarkedForDeletion))
                || ShowGenres.Any(o => o.IsDirty 
                    || (o.ShowGenreId == 0 && !o.IsMarkedForDeletion)
                    || (o.ShowGenreId > 0 && o.IsMarkedForDeletion));
        }


        public override string Validate(string propertyName = null)
        {
            List<string> errors = new List<string>();
            string err;
            switch (propertyName)
            {
                case "Title":
                    if (String.IsNullOrEmpty(Title))
                        errors.Add("Title is required.");
                    if (Title != null && Title.Length > 100)
                        errors.Add("Title cannot exceed 100 characters");
                    break;
                case "LengthInMinutes":
                    if (LengthInMinutes.HasValue
                        && (LengthInMinutes < 10 || LengthInMinutes > 1000))
                        errors.Add("Show length must be between 10 and 1000");
                    break;
                case "Credits":
                    foreach (var c in Credits)
                    {
                        err = c.Validate();
                        if (err != null) errors.Add(err);
                    }
                    break;
                case null:
                    err = Validate("Title");
                    if (err != null) errors.Add(err);

                    err = Validate("LengthInMinutes");
                    if (err != null) errors.Add(err);

                    err = Validate("Credits");
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
