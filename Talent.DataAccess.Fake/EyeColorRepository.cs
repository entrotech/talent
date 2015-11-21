using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class EyeColorRepository : IRepository<EyeColor>
    {
        #region IRepository<EyeColor> Members

        public IEnumerable<EyeColor> Fetch(object criteria = null)
        {
            var list = new List<EyeColor>();
            if(criteria == null)
            {
                foreach(var row in FakeDatabase.Instance.EyeColors
                    .OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if(criteria is int)
            {
                var row = FakeDatabase.Instance
                    .EyeColors.FirstOrDefault(o => o.EyeColorId == (int)criteria);
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

        public EyeColor Persist(EyeColor item)
        {
            if (item.EyeColorId == 0 && item.IsMarkedForDeletion) return null;
            if(item.EyeColorId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .EyeColors.Select(o => o.EyeColorId).Max();
                item.EyeColorId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.EyeColors.Add(row);
            }
            else
            {
                // Locate existing item
                var row = FakeDatabase.Instance
                    .EyeColors.Where(o => o.EyeColorId == item.EyeColorId)
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
                        FakeDatabase.Instance.EyeColors.Remove(row);
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

        private EyeColor MapRowToObject(EyeColorRow row)
        {
            var existingItem = new EyeColor
            {
                EyeColorId = row.EyeColorId,
                Name = row.Name,
                Code = row.Code,
                IsInactive = row.IsInactive,
                DisplayOrder = row.DisplayOrder
            };
            // Item retrieved from the "database" is not dirty.
            existingItem.IsDirty = false;
            return existingItem;
        }

        private EyeColorRow MapObjectToRow(EyeColor item)
        {
            return new EyeColorRow {
                EyeColorId = item.EyeColorId,
                Name = item.Name,
                Code = item.Code,
                IsInactive = item.IsInactive,
                DisplayOrder = item.DisplayOrder
            };
        }

        #endregion
    }
}
