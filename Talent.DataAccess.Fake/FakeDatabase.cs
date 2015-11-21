using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.DataAccess.Fake
{
    /// <summary>
    /// The FakeDatabase simulates a database in memory.
    /// </summary>
    /// <remarks>
    /// This uses a Singleton Pattern [Gang of Four] to assure that only one instance of 
    /// the database exists at a time. For use in testing, you can use the Reset() method
    /// to return the database to a state with the original "seed" sample data, as populated 
    /// in the constructor.
    /// 
    /// To use this database, you generally just use the Instance property, e.g.,
    /// 
    /// var genres = FakeDatabase.Instance.Genres.ToList();
    /// 
    /// will return a list of all Genres.
    /// </remarks>
    public class FakeDatabase 
    {
        #region Fields

        private static FakeDatabase _instance;

        #endregion

        #region Constructors

        private FakeDatabase()
        {
            MpaaRatings = LoadMpaaRatings();
            Genres = LoadGenres();
            CreditTypes = LoadCreditTypes();
            HairColors = LoadHairColors();
            EyeColors = LoadEyeColors();
            Shows = LoadShows();
            People = LoadPeople();
            ShowGenres = LoadShowGenres();
            Credits = LoadCredits();
        }

        #endregion

        #region Properties

        public static FakeDatabase Instance { 
            get {
                if (_instance == null)
                {
                    _instance = new FakeDatabase();
                }
                return _instance;
            }
        }

        public List<MpaaRatingRow> MpaaRatings { get; set; }
        public List<GenreRow> Genres { get; set; }
        public List<CreditTypeRow> CreditTypes { get; set; }
        public List<HairColorRow> HairColors { get; set; }
        public List<EyeColorRow> EyeColors { get; set; }
        public List<ShowRow> Shows { get; set; }
        public List<PersonRow> People { get; set; }
        public List<ShowGenreRow> ShowGenres { get; set; }
        public List<CreditRow> Credits { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets the database to it's default sample data.
        /// </summary>
        public static void Reset()
        {
            _instance = null;
        }

        #endregion

        #region Load Methods

        private static List<MpaaRatingRow> LoadMpaaRatings()
        {
            var maxMpaaRatingId = 0;
            var list = new List<MpaaRatingRow>();
            list.Add(
                new MpaaRatingRow()
                {
                    MpaaRatingId = ++maxMpaaRatingId,
                    Code = "G",
                    Name = "General Audiences",
                    Description = "All ages admitted.",
                    DisplayOrder = 10
                }
                );
            list.Add(
                new MpaaRatingRow()
                {
                    MpaaRatingId = ++maxMpaaRatingId,
                    Code = "PG",
                    Name = "Parental Guidance Suggested",
                    Description = "Some material may not be suitable for children.",
                    DisplayOrder = 20
                }
                );
            list.Add(
                new MpaaRatingRow()
                {
                    MpaaRatingId = ++maxMpaaRatingId,
                    Code = "PG-13",
                    Name = "Parents Are Strongly Cautioned",
                    Description = "Parents are urged to be cautious.",
                    DisplayOrder = 30
                }
                );
            list.Add(
                new MpaaRatingRow()
                {
                    MpaaRatingId = ++maxMpaaRatingId,
                    Code = "R",
                    Name = "Restricted",
                    Description = "People under 17 years must be accompanied.",
                    DisplayOrder = 40
                }
                );
            list.Add(
                new MpaaRatingRow()
                {
                    MpaaRatingId = ++maxMpaaRatingId,
                    Code = "NC-17",
                    Name = "Adults Only",
                    Description = "Exclusively for adults.",
                    DisplayOrder = 50
                }
                );
            list.Add(
                new MpaaRatingRow()
                {
                    MpaaRatingId = ++maxMpaaRatingId,
                    Code = "UR",
                    Name = "Unrated",
                    Description = "Not submitted for rating.",
                    DisplayOrder = 60
                }
                );
            return list;
        }

        private static List<GenreRow> LoadGenres()
        {
            var maxId = 0;
            var list = new List<GenreRow>();
            list.Add(
                new GenreRow()
                {
                    GenreId = ++maxId, // 1
                    Code = "Dra",
                    Name = "Drama"
                });
            list.Add(
                new GenreRow()
                {
                    GenreId = ++maxId, //2
                    Code = "Com",
                    Name = "Comedy"
                });
            list.Add(
                new GenreRow()
                {
                    GenreId = ++maxId, // 3
                    Code = "Sus",
                    Name = "Suspense"
                });
            list.Add(
                new GenreRow()
                {
                    GenreId = ++maxId, // 4
                    Code = "Act",
                    Name = "Action"
                });
            list.Add(
                new GenreRow()
                {
                    GenreId = ++maxId,  // 5
                    Code = "Doc",
                    Name = "Documentary"
                });
            list.Add(
                new GenreRow()
                {
                    GenreId = 6,
                    Code = "Ani",
                    Name = "Animation"
                });
            list.Add(
                new GenreRow()
                {
                    GenreId = 7,
                    Code = "Adv",
                    Name = "Adventure"
                });

            return list;
        }

        private static List<CreditTypeRow> LoadCreditTypes()
        {
            var maxId = 0;
            var list = new List<CreditTypeRow>();
            list.Add(
                new CreditTypeRow()
                {
                    CreditTypeId = ++maxId, // 1
                    Code = "Act",
                    Name = "Actor",
                    DisplayOrder = 5
                });
            list.Add(
                new CreditTypeRow()
                {
                    CreditTypeId = ++maxId, // 2
                    Code = "Dir",
                    Name = "Director",
                    DisplayOrder = 10
                });
            list.Add(
                new CreditTypeRow()
                {
                    CreditTypeId = ++maxId, // 3
                    Code = "Prod",
                    Name = "Producer",
                    DisplayOrder = 10
                });
            list.Add(
                new CreditTypeRow()
                {
                    CreditTypeId = ++maxId, // 4
                    Code = "EP",
                    Name = "Executive Producer",
                    DisplayOrder = 10
                });
            list.Add(
                new CreditTypeRow()
                {
                    CreditTypeId = ++maxId, // 5
                    Code = "Wri",
                    Name = "Writer",
                    DisplayOrder = 10
                });

            return list;
        }

        private static List<HairColorRow> LoadHairColors()
        {
            var maxId = 0;
            var list = new List<HairColorRow>();
            list.Add(
                new HairColorRow()
                {
                    HairColorId = ++maxId, // 1
                    Code = "Unk",
                    Name = "Unknown",
                    DisplayOrder = 5
                });
            list.Add(
                new HairColorRow()
                {
                    HairColorId = ++maxId, // 2
                    Code = "Bald",
                    Name = "Bald",
                    DisplayOrder = 10
                });
            list.Add(
                new HairColorRow()
                {
                    HairColorId = ++maxId, // 3
                    Code = "Blnd",
                    Name = "Blonde",
                    DisplayOrder = 10
                });
            list.Add(
                new HairColorRow()
                {
                    HairColorId = ++maxId, // 4
                    Code = "Brun",
                    Name = "Brunette",
                    DisplayOrder = 10
                });
            list.Add(
                new HairColorRow()
                {
                    HairColorId = ++maxId, // 5
                    Code = "Gray",
                    Name = "Gray",
                    DisplayOrder = 10
                });
            list.Add(
                new HairColorRow()
                {
                    HairColorId = ++maxId, // 6
                    Code = "Red",
                    Name = "Red",
                    DisplayOrder = 10
                });

            return list;
        }

        private static List<EyeColorRow> LoadEyeColors()
        {
            var maxId = 0;
            var list = new List<EyeColorRow>();
            list.Add(
                new EyeColorRow()
                {
                    EyeColorId = ++maxId, // 1
                    Code = "Unk",
                    Name = "Unknown",
                    DisplayOrder = 5
                });
            list.Add(
                new EyeColorRow()
                {
                    EyeColorId = ++maxId, // 2
                    Code = "Blue",
                    Name = "Blue",
                    DisplayOrder = 10
                });
            list.Add(
                new EyeColorRow()
                {
                    EyeColorId = ++maxId, // 3
                    Code = "Brn",
                    Name = "Brown",
                    DisplayOrder = 10
                });
            list.Add(
                new EyeColorRow()
                {
                    EyeColorId = ++maxId, // 4
                    Code = "Hzl",
                    Name = "Hazel",
                    DisplayOrder = 10
                });
            list.Add(
                new EyeColorRow()
                {
                    EyeColorId = ++maxId, // 5
                    Code = "Grn",
                    Name = "Green",
                    DisplayOrder = 10
                });
            list.Add(
                new EyeColorRow()
                {
                    EyeColorId = ++maxId, // 6
                    Code = "Blk",
                    Name = "Black",
                    DisplayOrder = 10
                });

            return list;
        }

        private static List<ShowRow> LoadShows()
        {
            var maxId = 0;
            var list = new List<ShowRow>();
            list.Add(
                new ShowRow()
                {
                    ShowId = ++maxId, // 1
                    Title = "Philomena",
                    LengthInMinutes = 98,
                    MpaaRatingId = 3,
                    TheatricalReleaseDate = new DateTime(2013, 11, 27)
                });

            list.Add(
                new ShowRow()
                {
                    ShowId = ++maxId, // 2
                    Title = "Frozen",
                    LengthInMinutes = 102,
                    MpaaRatingId = 2,
                    TheatricalReleaseDate = new DateTime(2013, 11, 27)
                });

            return list;
        }

        private static List<PersonRow> LoadPeople()
        {
            var maxId = 0;
            List<PersonRow> list = new List<PersonRow>();
            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 1
                    Salutation = "Ms.",
                    FirstName = "Judith",
                    MiddleName = "Olivia",
                    LastName = "Dench",
                    Suffix = "",
                    DateOfBirth = new DateTime(1934, 12, 9),
                    HairColorId = 5,
                    EyeColorId = 2,
                    Height = 66
                });

            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 2
                    Salutation = "Mr.",
                    FirstName = "Stephen",
                    MiddleName = "",
                    LastName = "Frears",
                    Suffix = "",
                    DateOfBirth = new DateTime(1941, 6, 20)
                });


            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 3
                    Salutation = "Mr.",
                    FirstName = "Stephen",
                    MiddleName = "John",
                    LastName = "Coogan",
                    Suffix = "",
                    DateOfBirth = new DateTime(1965, 10, 14),
                    HairColorId = 4,
                    EyeColorId = 5,
                    Weight = 185,
                    Height = 72
                });

            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 4
                    Salutation = "Ms.",
                    FirstName = "Sophie",
                    MiddleName = "Kennedy",
                    LastName = "Clark",
                    Suffix = "",
                    DateOfBirth = null,
                    HairColorId = 3,
                    EyeColorId = 3,
                    Weight = 115,
                    Height = 68
                });

            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 5
                    Salutation = "Mr.",
                    FirstName = "Chris",
                    MiddleName = "",
                    LastName = "Buck",
                    Suffix = "",
                    DateOfBirth = null,
                    HairColorId = 4,
                    EyeColorId = 5,
                    Weight = 175,
                    Height = 68
                });

            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 6
                    Salutation = "Ms.",
                    FirstName = "Jennifer",
                    MiddleName = "",
                    LastName = "Lee",
                    Suffix = "",
                    DateOfBirth = null,
                    HairColorId = 3,
                    EyeColorId = 5
                });

            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 7
                    Salutation = "Ms.",
                    FirstName = "Kristen",
                    MiddleName = "",
                    LastName = "Bell",
                    Suffix = "",
                    DateOfBirth = new DateTime(1980, 7, 18),
                    HairColorId = 4,
                    EyeColorId = 6
                });

            list.Add(
                new PersonRow
                {
                    PersonId = ++maxId, // 8
                    Salutation = "",
                    FirstName = "Josh",
                    MiddleName = "",
                    LastName = "Gad",
                    Suffix = "",
                    DateOfBirth = null,
                    HairColorId = 1,
                    EyeColorId = 1
                });

            return list;
        }

        private static List<ShowGenreRow> LoadShowGenres()
        {
            var maxShowGenreId = 0;

            var list = new List<ShowGenreRow>();
            ShowGenreRow showGenre;

            showGenre = new ShowGenreRow
            {
                ShowGenreId = ++maxShowGenreId,
                ShowId = 1,
                GenreId = 1
            };
            list.Add(showGenre);

            showGenre = new ShowGenreRow
            {
                ShowGenreId = ++maxShowGenreId,
                ShowId = 2,
                GenreId = 2
            };
            list.Add(showGenre);

            showGenre = new ShowGenreRow
            {
                ShowGenreId = ++maxShowGenreId,
                ShowId = 2,
                GenreId = 6
            };
            list.Add(showGenre);

            showGenre = new ShowGenreRow
            {
                ShowGenreId = ++maxShowGenreId,
                ShowId = 2,
                GenreId = 7
            };

            list.Add(showGenre);
            return list;
        }

        private static List<CreditRow> LoadCredits()
        {
            var maxId = 0;
            var list = new List<CreditRow>();
            CreditRow crd;
            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 1,
                PersonId = 1,
                CreditTypeId = 1,
                Character = "Philomena"
            };
            list.Add(crd);


            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 1,
                PersonId = 2,
                CreditTypeId = 2
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 1,
                PersonId = 3,
                CreditTypeId = 5
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 1,
                PersonId = 3,
                CreditTypeId = 1,
                Character = "Martin Sixsmith"
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 1,
                PersonId = 4,
                CreditTypeId = 1,
                Character = "Young Philomena"
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 1,
                PersonId = 5,
                CreditTypeId = 2
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 2,
                PersonId = 6,
                CreditTypeId = 2
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 2,
                PersonId = 6,
                CreditTypeId = 5
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 2,
                PersonId = 7,
                CreditTypeId = 1,
                Character = "Anna"
            };
            list.Add(crd);

            crd = new CreditRow
            {
                CreditId = ++maxId,
                ShowId = 2,
                PersonId = 8,
                CreditTypeId = 1,
                Character = "Olaf"
            };
            list.Add(crd);

            return list;
        }
        #endregion

        #region Helper Methods

        public static MpaaRatingRow FindMpaaRatingById(List<MpaaRatingRow> ratings, int id)
        {
            foreach (var r in ratings)
            {
                if (r.MpaaRatingId == id) return r;
            }
            return null;
        }

        public static GenreRow FindGenreById(List<GenreRow> genres, int id)
        {
            foreach (var g in genres)
            {
                if (g.GenreId == id) return g;
            }
            return null;
        }

        public static CreditTypeRow FindCreditTypeById(List<CreditTypeRow> creditTypes, int id)
        {
            foreach (var c in creditTypes)
            {
                if (c.CreditTypeId == id) return c;
            }
            return null;
        }

        public static HairColorRow FindHairColorById(List<HairColorRow> hairColors, int id)
        {
            foreach (var c in hairColors)
            {
                if (c.HairColorId == id) return c;
            }
            return null;
        }

        public static EyeColorRow FindEyeColorById(List<EyeColorRow> EyeColors, int id)
        {
            foreach (var c in EyeColors)
            {
                if (c.EyeColorId == id) return c;
            }
            return null;
        }

        public static ShowRow FindShowById(List<ShowRow> shows, int id)
        {
            foreach (var c in shows)
            {
                if (c.ShowId == id) return c;
            }
            return null;
        }

        public static PersonRow FindPersonById(List<PersonRow> people, int id)
        {
            foreach (var c in people)
            {
                if (c.PersonId == id) return c;
            }
            return null;
        }

        #endregion

    }
}
