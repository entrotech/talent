using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.ExtensionMethods;
using Ucla.Common.Interfaces;
using Ucla.Common.Utility;

namespace Talent.DataAccess.Ado
{
    public class MpaaRatingRepository : IRepository<MpaaRating>
    {
        #region IRepository interface

        public IEnumerable<MpaaRating> Fetch(object criteria = null)
        {
            var data = new List<MpaaRating>();
            var connString = ConfigurationManager
                .ConnectionStrings["AppConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    if (criteria == null)
                    {
                        cmd.CommandText = "select * from MpaaRating";
                    }
                    else if (criteria is int)
                    {
                        cmd.CommandText = "select * from MpaaRating where MpaaRatingId = @MpaaRatingId";
                        cmd.Parameters.AddWithValue("@MpaaRatingId", (int)criteria);
                    }
                    else
                    {
                        var msg = String.Format(
                            "MpaaRatingRepository: Unknown criteria type: {0}",
                            criteria);
                        throw new InvalidOperationException(msg);
                    }
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var g = new MpaaRating();
                        g.MpaaRatingId = dr.AsInt32("MpaaRatingId");
                        g.Code = dr.AsString("Code");
                        g.Name = dr.AsString("Name");
                        g.Description = dr.AsString("Description");
                        g.IsInactive = dr.AsBoolean("IsInactive");
                        g.DisplayOrder = dr.AsInt32("DisplayOrder");

                        g.IsDirty = false;
                        data.Add(g);
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// Saves entity changes to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>updated entity, or null if the entity is deleted</returns>
        public MpaaRating Persist(MpaaRating item)
        {
            try
            {
                if (item.MpaaRatingId == 0 && item.IsMarkedForDeletion)
                {
                    item = null;
                }

                var connString = ConfigurationManager.ConnectionStrings["AppConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    if (item.IsMarkedForDeletion)
                    {
                        DeleteEntity(item, conn);
                        item = null;
                    }
                    else if (item.MpaaRatingId == 0)
                    {
                        InsertEntity(item, conn);
                        item.IsDirty = false;
                    }
                    else if (item.IsDirty)
                    {
                        UpdateEntity(item, conn);
                        item.IsDirty = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                var msg = SqlExceptionDecoder.GetFriendlyMessage("Show", ex);
                throw new ApplicationException(msg, ex);
            }
            return item;
        }

        #endregion

        #region SQL methods

        internal static void InsertEntity(MpaaRating item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("insert MpaaRating (Code, Name, Description, IsInactive, DisplayOrder)");
                sql.Append("values (@Code, @Name, @Description, @IsInactive, @DisplayOrder);");
                sql.Append("select cast( scope_identity() as int);");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);

                item.MpaaRatingId = (int)cmd.ExecuteScalar();
            }
        }

        internal static void UpdateEntity(MpaaRating item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("update MpaaRating set ");
                sql.Append(" Code = @Code, ");
                sql.Append(" Name = @Name, ");
                sql.Append(" IsInactive = @IsInactive, ");
                sql.Append(" DisplayOrder = @DisplayOrder, ");
                sql.Append(" Description = @Description ");
                sql.Append("where MpaaRatingId = @MpaaRatingId");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);
                cmd.Parameters.AddWithValue("@MpaaRatingId", item.MpaaRatingId);

                cmd.ExecuteNonQuery();
            }
        }

        internal static void DeleteEntity(MpaaRating item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "delete MpaaRating where MpaaRatingId = @MpaaRatingId";
                cmd.Parameters.AddWithValue("@MpaaRatingId", item.MpaaRatingId);
                cmd.ExecuteNonQuery();
            }
        }



        private static void SetCommonParameters(MpaaRating item, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Code", item.Code);
            cmd.Parameters.AddWithValue("@Name", item.Name);
            cmd.Parameters.AddWithValue("@Description", item.Description);
            cmd.Parameters.AddWithValue("@IsInactive", item.IsInactive);
            cmd.Parameters.AddWithValue("@DisplayOrder", item.DisplayOrder);
        }

        #endregion

    }
}

