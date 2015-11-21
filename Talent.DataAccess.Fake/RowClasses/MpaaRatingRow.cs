using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.DataAccess.Fake
{
    /// <summary>
    /// Represents a database record in the MpaaRating table.
    /// </summary>
    public class MpaaRatingRow
    {
        public int MpaaRatingId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsInactive { get; set;}
        public int DisplayOrder { get; set; }
    }
}
