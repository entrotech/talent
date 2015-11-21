using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.DataAccess.Fake
{
    /// <summary>
    /// Represents a Credit record in the Fake database.
    /// </summary>
    public class CreditRow
    {
        public int CreditId { get; set; }
        public int ShowId { get; set; }
        public int PersonId { get; set; }
        public int CreditTypeId { get; set; }
        public string Character { get; set; }
    }
}
