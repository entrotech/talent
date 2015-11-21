using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class GenreRepository : IRepository<Genre>
    {
        #region IRepository<Genre> Members

        public IEnumerable<Genre> Fetch(object criteria = null)
        {
            var list = new List<Genre>();
            if(criteria == null)
            {
                foreach(var row in FakeDatabase.Instance.Genres
                    .OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if(criteria is int)
            {
                var row = FakeDatabase.Instance
                    .Genres.FirstOrDefault(o => o.GenreId == (int)criteria);
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

        public Genre Persist(Genre item)
        {
            if (item.GenreId == 0 && item.IsMarkedForDeletion) return null;
            if(item.GenreId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .Genres.Select(o => o.GenreId).Max();
                item.GenreId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.Genres.Add(row);
            }
            else
            {
                // Locate existing item
                var row = FakeDatabase.Instance
                    .Genres.Where(o => o.GenreId == item.GenreId)
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
                        FakeDatabase.Instance.Genres.Remove(row);
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

        private Genre MapRowToObject(GenreRow row)
        {
            var existingItem =  new Genre
            {
                GenreId = row.GenreId,
                Name = row.Name,
                Code = row.Code,
                IsInactive = row.IsInactive,
                DisplayOrder = row.DisplayOrder
            };
            // Item retrieved from the "database" is not dirty.
            existingItem.IsDirty = false;
            return existingItem;
        }

        private GenreRow MapObjectToRow(Genre item)
        {
            return new GenreRow {
                GenreId = item.GenreId,
                Name = item.Name,
                Code = item.Code,
                IsInactive = item.IsInactive,
                DisplayOrder = item.DisplayOrder
            };
        }

        #endregion
    }
}
