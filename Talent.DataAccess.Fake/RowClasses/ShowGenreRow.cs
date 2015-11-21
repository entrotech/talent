using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.DataAccess.Fake
{
    /// <summary>
    /// Represents a ShowGenre records in the Fake database
    /// </summary>
    public class ShowGenreRow
    {
        public int ShowGenreId { get; set; }
        public int ShowId { get; set; }
        public int GenreId { get; set; }
    }
}