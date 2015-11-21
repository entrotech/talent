using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class PersonRepository : IRepository<Person>
    {
        #region IRepository<Person> Members

        public IEnumerable<Person> Fetch(object criteria = null)
        {
            var list = new List<Person>();
            if (criteria == null)
            {
                foreach (var row in FakeDatabase.Instance.People
                    .OrderBy(o => o.LastName)
                    .ThenBy(o => o.FirstName))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if (criteria is int)
            {
                var row = FakeDatabase.Instance
                    .People.FirstOrDefault(o => o.PersonId == (int)criteria);
                if (row != null)
                {
                    list.Add(MapRowToObject(row));
                }
                // If row == null, then record is not found and list returns empty.
            }
            else
            {
                throw new InvalidOperationException("Invalid Query criteria type.");
            }
            return list;
        }

        public Person Persist(Person item)
        {
            if (item.PersonId == 0 && item.IsMarkedForDeletion) return null;
            if (item.PersonId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .People.Select(o => o.PersonId).Max();
                item.PersonId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.People.Add(row);
                PersistChildren(item);
            }
            else
            {
                // Find existing item in database
                var row = FakeDatabase.Instance
                    .People.Where(o => o.PersonId == item.PersonId)
                    .FirstOrDefault();
                if (row == null)
                {
                    throw new ApplicationException("Record not found, another user may have deleted it.");
                }
                else
                {
                    if (item.IsMarkedForDeletion)
                    {
                        // Delete entire Object Graph (Children first to avoid RI problems)
                        CreditChildRepository.DeleteByPersonId(item.PersonId);
                        FakeDatabase.Instance.People.Remove(row);
                        item = null;
                    }
                    else
                    {
                        // Update the Person and Children
                        row.PersonId = item.PersonId;
                        row.Salutation = item.Salutation;
                        row.FirstName = item.FirstName;
                        row.MiddleName = item.MiddleName;
                        row.LastName = item.LastName;
                        row.Suffix = item.Suffix;
                        row.StageName = item.StageName;
                        row.DateOfBirth = item.DateOfBirth;
                        row.Height = item.Height;
                        row.Weight = item.Weight;
                        row.HairColorId = item.HairColorId;
                        row.EyeColorId = item.EyeColorId;
                        PersistChildren(item);
                    }
                }
            }
            return item;
        }

        #endregion // IRepository<T> interface

        #region Private Methods

        private Person MapRowToObject(PersonRow row)
        {
            var item = new Person
            {
                PersonId = row.PersonId,
                Salutation = row.Salutation,
                FirstName = row.FirstName,
                MiddleName = row.MiddleName,
                LastName = row.LastName,
                Suffix = row.Suffix,
                StageName = row.StageName,
                DateOfBirth = row.DateOfBirth,
                Height = row.Height,
                Weight = row.Weight,
                HairColorId = row.HairColorId,
                EyeColorId = row.EyeColorId,    
            };

            // Populate Credits Collection
            var credits = CreditChildRepository.GetByPersonId(item.PersonId);
            if (credits.Any())
                item.Credits.AddRange(credits);

            // Item retrieved from the "database" is not dirty.
            if (item != null) item.IsDirty = false;
            return item;
        }

        private PersonRow MapObjectToRow(Person item)
        {
            return new PersonRow
            {
                PersonId = item.PersonId,
                Salutation = item.Salutation,
                FirstName = item.FirstName,
                MiddleName = item.MiddleName,
                LastName = item.LastName,
                Suffix = item.Suffix,
                StageName = item.StageName,
                DateOfBirth = item.DateOfBirth,
                Height = item.Height,
                Weight = item.Weight,
                HairColorId = item.HairColorId,
                EyeColorId = item.EyeColorId,
            };
        }

        private void PersistChildren(Person item)
        {

            var credits = item.Credits.ToList();
            for (int index = credits.Count() - 1; index >= 0; index--)
            {
                credits[index].PersonId = item.PersonId;
                var updatedCredit = CreditChildRepository.Persist(credits[index]);
                if (updatedCredit == null)
                    credits.RemoveAt(index);
                else
                    credits[index] = updatedCredit;
            }
        }

        #endregion
    }
}
