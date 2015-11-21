using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.DataAccess.Fake
{
    // Represents a Person row in the Fake database
    public class PersonRow
    {
        public int PersonId { get; set; }
        public string Salutation { get; set; }
        public string FirstName  { get; set; }
        public string MiddleName  { get; set; }
        public string LastName  { get; set; }
        public string Suffix  { get; set; }
        public string StageName  { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public double? Height  { get; set; }
        public double? Weight { get; set; }
        public int? HairColorId { get; set; }
        public int? EyeColorId { get; set; }
    }
}
