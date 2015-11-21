using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class MpaaRatingRepository : IRepository<MpaaRating>
    {
        #region IRepository<MpaaRating> Members

        public IEnumerable<MpaaRating> Fetch(object criteria = null)
        {
            var list = new List<MpaaRating>();
            if(criteria == null)
            {
                foreach(var row in FakeDatabase.Instance.MpaaRatings
                    .OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if(criteria is int)
            {
                var row = FakeDatabase.Instance
                    .MpaaRatings.FirstOrDefault(o => o.MpaaRatingId == (int)criteria);
                if(row != null)
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

        public MpaaRating Persist(MpaaRating item)
        {
            if (item.MpaaRatingId == 0 && item.IsMarkedForDeletion) return null;
            if(item.MpaaRatingId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .MpaaRatings.Select(o => o.MpaaRatingId).Max();
                item.MpaaRatingId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.MpaaRatings.Add(row);
            }
            else
            {
                // Locate existing item
                var row = FakeDatabase.Instance
                    .MpaaRatings.Where(o => o.MpaaRatingId == item.MpaaRatingId)
                    .FirstOrDefault();
                if(row == null)
                {
                    throw new ApplicationException("Record not found, another user may have deleted it.");
                }
                else
                {
                    if (item.IsMarkedForDeletion)
                    {
                        // Delete
                        FakeDatabase.Instance.MpaaRatings.Remove(row);
                        item = null;
                    }
                    else
                    {
                        // Update
                        row.Name = item.Name;
                        row.Code = item.Code;
                        row.Description = item.Description;
                        row.IsInactive = item.IsInactive;
                        row.DisplayOrder = item.DisplayOrder;
                    }
                }
            }
            if(item != null) item.IsDirty = false;
            return item;
        }

        #endregion // IRepository<T> interface

        #region Private Methods

        private MpaaRating MapRowToObject(MpaaRatingRow row)
        {
            var existingItem =  new MpaaRating
            {
                MpaaRatingId = row.MpaaRatingId,
                Name = row.Name,
                Code = row.Code,
                Description = row.Description,
                IsInactive = row.IsInactive,
                DisplayOrder = row.DisplayOrder
            };
            // Item retrieved from the "database" is not dirty.
            existingItem.IsDirty = false;
            return existingItem;
        }

        private MpaaRatingRow MapObjectToRow(MpaaRating item)
        {
            return new MpaaRatingRow {
                MpaaRatingId = item.MpaaRatingId,
                Name = item.Name,
                Code = item.Code,
                Description = item.Description,
                IsInactive = item.IsInactive,
                DisplayOrder = item.DisplayOrder
            };
        }

        #endregion
    }
}
