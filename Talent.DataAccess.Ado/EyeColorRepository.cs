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

namespace Talent.DataAccess.Ado
{
    public class EyeColorRepository : IRepository<EyeColor>
    {
        #region IRepository interface

        public IEnumerable<EyeColor> Fetch(object criteria = null)
        {
            var data = new List<EyeColor>();
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
                        cmd.CommandText = "select * from EyeColor";
                    }
                    else if (criteria is int)
                    {
                        cmd.CommandText = "select * from EyeColor where EyeColorId = @EyeColorId";
                        cmd.Parameters.AddWithValue("@EyeColorId", (int)criteria);
                    }
                    else
                    {
                        var msg = String.Format(
                            "EyeColorRepository: Unknown criteria type: {0}",
                            criteria);
                        throw new InvalidOperationException(msg);
                    }
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        var g = new EyeColor();
                        g.EyeColorId = dr.AsInt32("EyeColorId");
                        g.Code = dr.AsString("Code");
                        g.Name = dr.AsString("Name");
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
        public EyeColor Persist(EyeColor item)
        {
            throw new InvalidOperationException("Cannot persist EyeColor - it is not editable.");
        }

        #endregion

    }
}


