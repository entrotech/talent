using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Talent.DataAccess.Fake;
using Talent.DataAccess.Ado;
using Talent.Domain;

namespace Talent.WpfClient
{
    public static class LookupCache
    {
        private static MpaaRatingRepository _mpaaRatingRepository;
        private static GenreRepository _genreRepository;
        private static CreditTypeRepository _creditTypeRepository;
        private static PersonRepository _personRepository;
        private static ShowRepository _showRepository;
        private static HairColorRepository _hairColorRepository;
        private static EyeColorRepository _eyeColorRepository;

        public static IEnumerable<MpaaRating> MpaaRatings
        {
            get
            {
                if (_mpaaRatings == null)
                {
                    if (_mpaaRatingRepository == null)
                    {
                        _mpaaRatingRepository = new MpaaRatingRepository();
                    }
                    _mpaaRatings = _mpaaRatingRepository.Fetch()
                        .OrderBy(o => o.DisplayOrder)
                        .ThenBy(o => o.Name).ToList();
                }
                return _mpaaRatings;
            }
            set { _mpaaRatings = value; }
        }
        private static IEnumerable<MpaaRating> _mpaaRatings;

        public static IEnumerable<Genre> Genres
        {
            get
            {
                if (_genres == null)
                {
                    if (_genreRepository == null)
                    {
                        _genreRepository = new GenreRepository();
                    }
                    _genres = _genreRepository.Fetch()
                        .OrderBy(o => o.DisplayOrder)
                        .ThenBy(o => o.Name).ToList();
                }
                return _genres;
            }
            set { _genres = value; }
        }
        private static IEnumerable<Genre> _genres;

        public static IEnumerable<CreditType> CreditTypes
        {
            get
            {
                if (_creditTypes == null)
                {
                    if (_creditTypeRepository == null)
                    {
                        _creditTypeRepository = new CreditTypeRepository();
                    }
                    _creditTypes = _creditTypeRepository.Fetch()
                        .OrderBy(o => o.DisplayOrder)
                        .ThenBy(o => o.Name).ToList();
                }
                return _creditTypes;
            }
            set { _creditTypes = value; }
        }
        private static IEnumerable<CreditType> _creditTypes;

        public static IEnumerable<HairColor> HairColors
        {
            get
            {
                if (_hairColors == null)
                {
                    if (_hairColorRepository == null)
                    {
                        _hairColorRepository = new HairColorRepository();
                    }
                    _hairColors = _hairColorRepository.Fetch()
                        .OrderBy(o => o.DisplayOrder)
                        .ThenBy(o => o.Name).ToList();
                }
                return _hairColors;
            }
            set { _hairColors = value; }
        }
        private static IEnumerable<HairColor> _hairColors;

        public static IEnumerable<EyeColor> EyeColors
        {
            get
            {
                if (_eyeColors == null)
                {
                    if (_eyeColorRepository == null)
                    {
                        _eyeColorRepository = new EyeColorRepository();
                    }
                    _eyeColors = _eyeColorRepository.Fetch()
                        .OrderBy(o => o.DisplayOrder)
                        .ThenBy(o => o.Name).ToList();
                }
                return _eyeColors;
            }
            set { _eyeColors = value; }
        }
        private static IEnumerable<EyeColor> _eyeColors;


        public static IEnumerable<Person> People
        {
            get
            {
                if (_people == null)
                {
                    if (_personRepository == null)
                    {
                        _personRepository = new PersonRepository();
                    }
                    _people = _personRepository.Fetch()
                        .OrderBy(o => o.LastName)
                        .ThenBy(o => o.FirstName).ToList();
                }
                return _people;
            }
            set { _people = value; }
        }
        private static IEnumerable<Person> _people;


        public static IEnumerable<Show> Shows
        {
            get
            {
                if (_shows == null)
                {
                    if (_showRepository == null)
                    {
                        _showRepository = new ShowRepository();
                    }
                    _shows = _showRepository.Fetch()
                        .OrderBy(o => o.Title).ToList();
                }
                return _shows;
            }
            set { _shows = value; }
        }
        private static IEnumerable<Show> _shows;
    }
}

