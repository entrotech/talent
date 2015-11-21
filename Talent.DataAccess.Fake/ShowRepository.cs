using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;

namespace Talent.DataAccess.Fake
{
    public class ShowRepository : IRepository<Show>
    {
        #region IRepository<Show> Members

        public IEnumerable<Show> Fetch(object criteria = null)
        {
            var list = new List<Show>();
            if (criteria == null)
            {
                foreach (var row in FakeDatabase.Instance.Shows
                    .OrderBy(o => o.Title))
                {
                    list.Add(MapRowToObject(row));
                }
            }
            else if (criteria is int)
            {
                var row = FakeDatabase.Instance
                    .Shows.FirstOrDefault(o => o.ShowId == (int)criteria);
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

        public Show Persist(Show item)
        {
            if (item.ShowId == 0 && item.IsMarkedForDeletion) return null;
            if (item.ShowId == 0)
            {
                // Insert
                var nextId = FakeDatabase.Instance
                    .Shows.Select(o => o.ShowId).Max();
                item.ShowId = ++nextId;
                var row = MapObjectToRow(item);
                FakeDatabase.Instance.Shows.Add(row);
                PersistChildren(item);
            }
            else
            {
                // Find existing item in database
                var row = FakeDatabase.Instance
                    .Shows.Where(o => o.ShowId == item.ShowId)
                    .FirstOrDefault();
                if (row == null)
                {
                    throw new ApplicationException(
                        "Record not found, another user may have deleted it.");
                }
                else
                {
                    if (item.IsMarkedForDeletion)
                    {
                        // Delete entire Object Graph (Children first to avoid RI problems)
                        ShowGenreChildRepository.DeleteByShowId(item.ShowId);
                        CreditChildRepository.DeleteByShowId(item.ShowId);
                        FakeDatabase.Instance.Shows.Remove(row);
                        item = null;
                    }
                    else
                    {
                        // Update the Show and Children
                        row.Title = item.Title;
                        row.LengthInMinutes = item.LengthInMinutes;
                        row.TheatricalReleaseDate = item.TheatricalReleaseDate;
                        row.DvdReleaseDate = item.DvdReleaseDate;
                        row.MpaaRatingId = item.MpaaRatingId;
                        PersistChildren(item);
                    }
                }
            }
            if (item != null) item.IsDirty = false;
            return item;
        }

        #endregion // IRepository<T> interface

        #region Private Methods

        private Show MapRowToObject(ShowRow row)
        {
            var parent = new Show
            {
                ShowId = row.ShowId,
                Title = row.Title,
                LengthInMinutes = row.LengthInMinutes,
                TheatricalReleaseDate = row.TheatricalReleaseDate,
                DvdReleaseDate = row.DvdReleaseDate,
                MpaaRatingId = row.MpaaRatingId
            };

            // Populate ShowGenre Collection
            var showGenres = ShowGenreChildRepository.GetByShowId(parent.ShowId);
            if (showGenres.Any())
                parent.ShowGenres.AddRange(showGenres);

            // Populate Credits Collection
            var credits = CreditChildRepository.GetByShowId(parent.ShowId);
            if (credits.Any())
                parent.Credits.AddRange(credits);

            // Item retrieved from the "database" is not dirty.
            parent.IsDirty = false;
            return parent;
        }

        private ShowRow MapObjectToRow(Show item)
        {
            return new ShowRow
            {
                ShowId = item.ShowId,
                Title = item.Title,
                LengthInMinutes = item.LengthInMinutes,
                TheatricalReleaseDate = item.TheatricalReleaseDate,
                DvdReleaseDate = item.DvdReleaseDate,
                MpaaRatingId = item.MpaaRatingId
            };
        }

        private void PersistChildren(Show item)
        {

            var showGenres = item.ShowGenres.ToList();
            for (int index = showGenres.Count() - 1; index >= 0; index--)
            {
                showGenres[index].ShowId = item.ShowId;
                var updatedShowGenre = ShowGenreChildRepository.Persist(showGenres[index]);
                if (updatedShowGenre == null)
                    showGenres.RemoveAt(index);
                else
                    showGenres[index] = updatedShowGenre;
            }

            var credits = item.Credits.ToList();
            for (int index = credits.Count() - 1; index >= 0; index--)
            {
                credits[index].ShowId = item.ShowId;
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
