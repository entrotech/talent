using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class CreditTypeRepository : IRepository<CreditType>
    {
        #region IRepository<CreditType> Members

        public IEnumerable<CreditType> Fetch(object criteria = null)
        {
            var list = new List<CreditType>();
            if(criteria == null)
            {
                foreach(var row in FakeDatabase.Instance.CreditTypes
                    .OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if(criteria is int)
            {
                var row = FakeDatabase.Instance
                    .CreditTypes.FirstOrDefault(o => o.CreditTypeId == (int)criteria);
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

        public CreditType Persist(CreditType item)
        {
            if (item.CreditTypeId == 0 && item.IsMarkedForDeletion) return null;
            if(item.CreditTypeId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .CreditTypes.Select(o => o.CreditTypeId).Max();
                item.CreditTypeId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.CreditTypes.Add(row);
            }
            else
            {
                // Locate existing item
                var row = FakeDatabase.Instance
                    .CreditTypes.Where(o => o.CreditTypeId == item.CreditTypeId)
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
                        FakeDatabase.Instance.CreditTypes.Remove(row);
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

        private CreditType MapRowToObject(CreditTypeRow row)
        {
            var existingItem = new CreditType
            {
                CreditTypeId = row.CreditTypeId,
                Name = row.Name,
                Code = row.Code,
                IsInactive = row.IsInactive,
                DisplayOrder = row.DisplayOrder
            };
            // Item retrieved from the "database" is not dirty.
            existingItem.IsDirty = false;
            return existingItem;
        }

        private CreditTypeRow MapObjectToRow(CreditType item)
        {
            return new CreditTypeRow {
                CreditTypeId = item.CreditTypeId,
                Name = item.Name,
                Code = item.Code,
                IsInactive = item.IsInactive,
                DisplayOrder = item.DisplayOrder
            };
        }

        #endregion
    }
}
