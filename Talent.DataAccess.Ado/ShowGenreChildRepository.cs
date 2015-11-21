using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;

namespace Talent.DataAccess.Ado
{
    internal class ShowGenreChildRepository
    {
        #region PersistChild

        public ShowGenre PersistChild(ShowGenre showGenre, SqlConnection conn)
        {
            if (showGenre.ShowGenreId == 0 && showGenre.IsMarkedForDeletion)
            {
                showGenre = null;
            }
            else if (showGenre.IsMarkedForDeletion)
            {
                DeleteEntity(showGenre, conn);
                showGenre = null;
            }
            else if (showGenre.ShowGenreId == 0)
            {
                InsertEntity(showGenre, conn);
                showGenre.IsDirty = false;
            }
            else if (showGenre.IsDirty)
            {
                UpdateEntity(showGenre, conn);
                showGenre.IsDirty = false;
            }
            return showGenre;
        }

        #endregion

        #region SQL

        internal static void InsertEntity(ShowGenre item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("insert ShowGenre (ShowId, GenreId)");
                sql.Append("values (@ShowId, @GenreId);");
                sql.Append("select cast ( scope_identity() as int);");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);
                item.ShowGenreId = (int)cmd.ExecuteScalar();
            }
        }

        internal static void UpdateEntity(ShowGenre item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("update ShowGenre set ");
                sql.Append(" ShowId = @ShowId, ");
                sql.Append(" GenreId = @GenreId ");
                sql.Append("where ShowGenreId = @ShowGenreId");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);
                cmd.Parameters.AddWithValue("@ShowGenreId", item.ShowGenreId);

                cmd.ExecuteNonQuery();
            }
        }

        internal static void DeleteEntity(ShowGenre item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "delete ShowGenre where ShowGenreId = @ShowGenreId";
                cmd.Parameters.AddWithValue("@ShowGenreId", item.ShowGenreId);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SetCommonParameters(ShowGenre item, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@ShowId", item.ShowId);
            cmd.Parameters.AddWithValue("@GenreId", item.GenreId);
        }

        #endregion
    }
}
