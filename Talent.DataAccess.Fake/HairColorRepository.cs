using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class HairColorRepository : IRepository<HairColor>
    {
        #region IRepository<HairColor> Members

        public IEnumerable<HairColor> Fetch(object criteria = null)
        {
            var list = new List<HairColor>();
            if(criteria == null)
            {
                foreach(var row in FakeDatabase.Instance.HairColors
                    .OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if(criteria is int)
            {
                var row = FakeDatabase.Instance
                    .HairColors.FirstOrDefault(o => o.HairColorId == (int)criteria);
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

        public HairColor Persist(HairColor item)
        {
            if (item.HairColorId == 0 && item.IsMarkedForDeletion) return null;
            if(item.HairColorId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .HairColors.Select(o => o.HairColorId).Max();
                item.HairColorId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.HairColors.Add(row);
            }
            else
            {
                // Locate existing item
                var row = FakeDatabase.Instance
                    .HairColors.Where(o => o.HairColorId == item.HairColorId)
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
                        FakeDatabase.Instance.HairColors.Remove(row);
                        item = null;
                    }
                    else
                    {
                        // Update
                        row.Name = item.Name;
                        row.Code = item.Code;
                        row.IsInactive = item.IsInactive;
                        row.DisplayOrder = item.DisplayOrder;
                    }
                }
            }
            if (item != null) item.IsDirty = false;
            return item;
        }

        #endregion // IRepository<T> interface

        #region Private Methods

        private HairColor MapRowToObject(HairColorRow row)
        {
            var existingItem = new HairColor
            {
                HairColorId = row.HairColorId,
                Name = row.Name,
                Code = row.Code,
                IsInactive = row.IsInactive,
                DisplayOrder = row.DisplayOrder
            };
            // Item retrieved from the "database" is not dirty.
            existingItem.IsDirty = false;
            return existingItem;
        }

        private HairColorRow MapObjectToRow(HairColor item)
        {
            return new HairColorRow {
                HairColorId = item.HairColorId,
                Name = item.Name,
                Code = item.Code,
                IsInactive = item.IsInactive,
                DisplayOrder = item.DisplayOrder
            };
        }

        #endregion
    }
}
