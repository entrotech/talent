using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ucla.Common.BaseClasses;

namespace Talent.Domain
{
    public class ShowGenre : DomainBase
    {
        #region Fields

        private int _showGenreId;
        private int _showId;
        private int _genreId;

        #endregion

        #region Properties

        public int ShowGenreId
        {
            get { return _showGenreId; }
            set
            {
                if (_showGenreId == value) return;
                _showGenreId = value;
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

        #endregion
    }
}