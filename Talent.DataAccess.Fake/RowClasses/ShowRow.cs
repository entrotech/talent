using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.DataAccess.Fake
{
    /// <summary>
    /// Represents a Show record in the Fake database
    /// </summary>
    public class ShowRow
    {
        public int ShowId { get; set; }
        public string Title { get; set; }
        public int? LengthInMinutes { get; set; }
        public DateTime? TheatricalReleaseDate { get; set; }
        public DateTime? DvdReleaseDate { get; set; }
        public int? MpaaRatingId { get; set; }
    }
}
