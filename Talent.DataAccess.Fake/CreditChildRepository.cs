using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;

namespace Talent.DataAccess.Fake
{
    internal static class CreditChildRepository
    {
        internal static IEnumerable<Credit> GetByShowId(int showId)
        {
            return FakeDatabase.Instance
                .Credits.Where(o => o.ShowId == showId)
                .Select(o => MapRowToObj(o))
                .ToList();
        }

        internal static IEnumerable<Credit> GetByPersonId(int personId)
        {
            return FakeDatabase.Instance
                .Credits.Where(o => o.PersonId == personId)
                .Select(o => MapRowToObj(o))
                .ToList();
        }

        internal static void DeleteByShowId(int showId)
        {
            var toDelete = FakeDatabase.Instance.Credits.Where(o => o.ShowId == showId).ToList();
            foreach (var td in toDelete)
            {
                FakeDatabase.Instance.Credits.Remove(td);
            }
        }

        internal static void DeleteByPersonId(int showId)
        {
            var toDelete = FakeDatabase.Instance.Credits.Where(o => o.ShowId == showId).ToList();
            foreach (var td in toDelete)
            {
                FakeDatabase.Instance.Credits.Remove(td);
            }
        }

        internal static Credit Persist(Credit item)
        {
            if (item.CreditId == 0 && item.IsMarkedForDeletion) return null;
            if (item.CreditId == 0)
            {
                // Insert
                var maxId = FakeDatabase.Instance.Credits.Max(o => o.CreditId);
                item.CreditId = ++maxId;
                var row = MapObjToRow(item);
                FakeDatabase.Instance.Credits.Add(row);
            }
            else
            {
                var existingRow = FakeDatabase.Instance.Credits
                    .FirstOrDefault(o => o.CreditId == item.CreditId);
                if(existingRow == null)
                {
                    throw new ApplicationException(
                        "Record not found, another user may have deleted it.");
                }
                else if(item.IsMarkedForDeletion)
                {
                    // Delete
                    FakeDatabase.Instance.Credits.Remove(existingRow);
                }
                else
                {
                    // Update
                    existingRow.ShowId = item.ShowId;
                    existingRow.PersonId = item.PersonId;
                    existingRow.CreditTypeId = item.CreditTypeId;
                    existingRow.Character = item.Character;
                }
            }
            // reset dirty flag because obj is now synced with db
            if (item != null) item.IsDirty = false;
            return item;
        }

        #region Private Methods

        private static Credit MapRowToObj(CreditRow row)
        {
            var existingItem = new Credit
            {
                CreditId = row.CreditId,
                ShowId = row.ShowId,
                PersonId = row.PersonId,
                CreditTypeId = row.CreditTypeId,
                Character = row.Character
            };
            existingItem.IsDirty = false;
            return existingItem;
        }


        private static CreditRow MapObjToRow(Credit item)
        {
            return new CreditRow
                   {
                       CreditId = item.CreditId,
                       ShowId = item.ShowId,
                       PersonId = item.PersonId,
                       CreditTypeId = item.CreditTypeId,
                       Character = item.Character
                   };
        }

        #endregion
    }
}
