using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;

namespace Talent.DataAccess.Fake
{
    internal static class ShowGenreChildRepository
    {
        #region Internal Methods

        internal static IEnumerable<ShowGenre> GetByShowId(int showId)
        {
            return FakeDatabase.Instance
                .ShowGenres.Where(o => o.ShowId == showId)
                .Select(o => MapRowToObj(o));
        }

        internal static void DeleteByShowId(int showId)
        {
            var toDelete = FakeDatabase.Instance
                .ShowGenres.Where(o => o.ShowId == showId).ToList();
            foreach(var td in toDelete)
            {
                FakeDatabase.Instance.ShowGenres.Remove(td);
            }
        }

        internal static ShowGenre Persist(ShowGenre item)
        {
            if (item.ShowGenreId == 0 && item.IsMarkedForDeletion) return null;
            if (item.ShowGenreId == 0)
            {
                // Insert
                var maxId = FakeDatabase.Instance
                    .ShowGenres.Max(o => o.ShowGenreId);
                item.ShowGenreId = ++maxId;
                var row = MapObjToRow(item);
                FakeDatabase.Instance.ShowGenres.Add(row);
            }
            else
            {
                var existingRow = FakeDatabase.Instance.ShowGenres
                    .FirstOrDefault(o => o.ShowGenreId == item.ShowGenreId);
                if (existingRow == null)
                {
                    throw new ApplicationException(
                        "Record not found, another user may have deleted it.");
                }
                else if (item.IsMarkedForDeletion)
                {
                    // Delete
                    FakeDatabase.Instance.ShowGenres.Remove(existingRow);
                }
                else
                {
                    // Update
                    existingRow.ShowId = item.ShowId;
                    existingRow.GenreId = item.GenreId;
                }
            }
            // reset dirty flag because obj is now synced with db
            if (item != null) item.IsDirty = false;
            return item;
        }

        #endregion

        #region Private Methods

        private static ShowGenre MapRowToObj(ShowGenreRow row)
        {
            var existingItem = new ShowGenre
            {
                ShowGenreId = row.ShowGenreId,
                ShowId = row.ShowId,
                GenreId = row.GenreId
            };
            existingItem.IsDirty = false;
            return existingItem;
        }

        private static ShowGenreRow MapObjToRow(ShowGenre item)
        {
            return new ShowGenreRow
            {
                ShowGenreId = item.ShowGenreId,
                ShowId = item.ShowId,
                GenreId = item.GenreId
            };
        }

        #endregion
    }
}
