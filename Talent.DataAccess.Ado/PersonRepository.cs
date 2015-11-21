using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;
using Ucla.Common.Interfaces;
using Ucla.Common.ExtensionMethods;
using System.Transactions;
using Ucla.Common.Utility;

namespace Talent.DataAccess.Ado
{
    public class PersonRepository : IRepository<Person>
    {
        #region IRepository<Person> Members

        public IEnumerable<Person> Fetch(object criteria = null)
        {
            var data = new List<Person>();
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
                        var sql = new StringBuilder();
                        sql.Append("select * from Person; ");
                        sql.Append("select * from Credit; ");
                        sql.Append("select * from PersonAttachment; ");
                        cmd.CommandText = sql.ToString();
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            data.Add(LoadPersonFromDataReader(dr));
                        }

                        dr.NextResult();
                        while (dr.Read())
                        {
                            var c = LoadCreditFromDataReader(dr);
                            data.Where(o => o.PersonId == c.PersonId)
                                .Single().Credits.Add(c);
                        }
                        dr.NextResult();
                        while (dr.Read())
                        {
                            var c = LoadPersonAttachmentFromDataReader(dr);
                            data.Where(o => o.PersonId == c.PersonId)
                                .Single().Attachments.Add(c);
                        }
                    }
                    else if (criteria is PersonCriteria)
                    {
                        var crit = criteria as PersonCriteria;
                        var sql = new StringBuilder();
                        sql.Append(@"select * 
                    from Person 
                    where FirstName like '%' + @Name + '%' or LastName like '%' + @Name + '%';");
                        sql.Append(@"select c.* 
                    from Credit c 
                    join Person p on c.PersonId = p.PersonId 
                    where p.FirstName like  '%' + @Name + '%' or p.LastName like '%' + @Name + '%';
                ");
                        sql.Append(@"select pa.* 
                    from PersonAttachment pa 
                    join Person p on pa.PersonId = p.PersonId 
                    where p.FirstName like  '%' + @Name + '%' or p.LastName like '%' + @Name + '%';
                ");
                        cmd.CommandText = sql.ToString();
                        cmd.Parameters.AddWithValue("@Name", crit.Name);
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            data.Add(LoadPersonFromDataReader(dr));
                        }

                        dr.NextResult();
                        while (dr.Read())
                        {
                            var c = LoadCreditFromDataReader(dr);
                            data.Where(o => o.PersonId == c.PersonId)
                                .Single().Credits.Add(c);
                        }
                        dr.NextResult();
                        while (dr.Read())
                        {
                            var c = LoadPersonAttachmentFromDataReader(dr);
                            data.Where(o => o.PersonId == c.PersonId)
                                .Single().Attachments.Add(c);
                        }
                    }
                    else if (criteria is int)
                    {
                        var sql = new StringBuilder();
                        sql.Append("select * from Person where "
                            + "PersonId = @PersonId; \r\n");
                        sql.Append("select * from Credit where "
                            + "PersonId = @PersonId; ");
                        sql.Append("select * from PersonAttachment where "
                            + "PersonId = @PersonId; ");
                        cmd.CommandText = sql.ToString();
                        cmd.Parameters.AddWithValue("@PersonId", (int)criteria);
                        var dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            data.Add(LoadPersonFromDataReader(dr));
                        }
                        dr.NextResult();
                        while (dr.Read())
                        {
                            data[0].Credits.Add(LoadCreditFromDataReader(dr));
                        }
                        dr.NextResult();
                        while (dr.Read())
                        {
                            data[0].Attachments.Add(LoadPersonAttachmentFromDataReader(dr));
                        }
                    }
                    else
                    {
                        var msg = String
                            .Format("PersonRepository: Unknown criteria type: {0}",
                                criteria);
                        throw new InvalidOperationException(msg);
                    }
                }
            }
            return data;
        }

        private static Credit LoadCreditFromDataReader(SqlDataReader dr)
        {
            var c = new Credit();
            c.CreditId = dr.AsInt32("CreditId");
            c.PersonId = dr.AsInt32("PersonId");
            c.ShowId = dr.AsInt32("ShowId");
            c.CreditTypeId = dr.AsInt32("CreditTypeId");
            c.Character = dr.AsString("Character");
            c.IsDirty = false;
            return c;
        }

        private static PersonAttachment LoadPersonAttachmentFromDataReader(SqlDataReader dr)
        {
            var c = new PersonAttachment();
            c.PersonAttachmentId = dr.AsInt32("PersonAttachmentId");
            c.PersonId = dr.AsInt32("PersonId");
            c.Caption = dr.AsString("Caption");
            c.FileName = dr.AsString("FileName");
            c.FileExtension = dr.AsString("FileExtension");
            var b = dr.GetSqlBytes(dr.GetOrdinal("FileBytes"));
            c.FileBytes = (byte[])dr["FileBytes"];

            c.IsDirty = false;
            return c;
        }

        private static Person LoadPersonFromDataReader(SqlDataReader dr)
        {
            var s = new Person();
            s.PersonId = dr.AsInt32("PersonId");
            s.Salutation = dr.AsString("Salutation");
            s.FirstName = dr.AsString("FirstName");
            s.MiddleName = dr.AsString("MiddleName");
            s.LastName = dr.AsString("LastName");
            s.Suffix = dr.AsString("Suffix");
            s.StageName = dr.AsString("StageName");
            s.DateOfBirth = dr.AsNullableDateTime("DateOfBirth");
            s.Weight = dr.AsNullableDouble("Weight");
            s.Height = dr.AsNullableDouble("Height");
            s.HairColorId = dr.AsInt32("HairColorId");
            s.EyeColorId = dr.AsInt32("EyeColorId");
            s.IsDirty = false;
            return s;
        }

        /// <summary>
        /// Saves entity changes to the database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>updated entity, or null if the entity is deleted</returns>
        public Person Persist(Person item)
        {
            try
            {
                if (item.PersonId == 0 && item.IsMarkedForDeletion)
                {
                    item = null;
                }

                var connString = ConfigurationManager
                    .ConnectionStrings["AppConnection"].ConnectionString;
                using (TransactionScope ts = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        if (item.IsMarkedForDeletion)
                        {
                            // Also Deletes Children
                            DeleteEntity(item, conn);
                            item = null;
                        }
                        else if (item.PersonId == 0)
                        {
                            InsertEntity(item, conn);
                            PersistChildren(item, conn);
                            item.IsDirty = false;
                        }
                        else if (item.IsDirty)
                        {
                            UpdateEntity(item, conn);
                            PersistChildren(item, conn);
                            item.IsDirty = false;
                        }
                        else
                        {
                            // No changes to Person, but might be changes to children
                            PersistChildren(item, conn);
                        }
                    }
                    ts.Complete();
                }
            }
            catch (SqlException ex)
            {
                var msg = SqlExceptionDecoder.GetFriendlyMessage("Show", ex);
                throw new ApplicationException(msg, ex);
            }
            return item;
        }

        private static void PersistChildren(Person person, SqlConnection conn)
        {
            if (person.Credits.Any())
            {
                var repo = new CreditChildRepository();
                for (var index = person.Credits.Count() - 1; index >= 0; index--)
                {
                    person.Credits[index].PersonId = person.PersonId;
                    var credit = repo.PersistChild(person.Credits[index], conn);
                    if (credit == null)
                    {
                        // Persist returns null, remove Credit from person
                        person.Credits.RemoveAt(index);
                    }
                    else
                    {
                        person.Credits[index] = credit;
                    }
                }
            }

            if (person.Attachments.Any())
            {
                var repo = new PersonAttachmentChildRepository();
                for (var index = person.Attachments.Count() - 1; index >= 0; index--)
                {
                    person.Attachments[index].PersonId = person.PersonId;
                    var attachment = repo.PersistChild(person.Attachments[index], conn);
                    if (attachment == null)
                    {
                        // Persist returns null, remove Credit from person
                        person.Attachments.RemoveAt(index);
                    }
                    else
                    {
                        person.Attachments[index] = attachment;
                    }
                }
            }
        }

        #endregion

        #region SQL Methods

        internal static void InsertEntity(Person item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("insert Person (Salutation, FirstName, MiddleName, "
                + "LastName, Suffix, StageName, DateOfBirth, Height, Weight, "
                + "HairColorId, EyeColorId)");
                sql.Append("values ( @Salutation, @FirstName, @MiddleName, "
                + "@LastName, @Suffix, @StageName, @DateOfBirth, @Height, @Weight, "
                + "@HairColorId, @EyeColorId); ");
                sql.Append("select cast ( scope_identity() as int);");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);

                item.PersonId = (int)cmd.ExecuteScalar();
            }
        }

        internal static void UpdateEntity(Person item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("update Person set ");
                sql.Append(" Salutation = @Salutation, ");
                sql.Append(" FirstName = @FirstName, ");
                sql.Append(" MiddleName = @MiddleName, ");
                sql.Append(" LastName = @LastName, ");
                sql.Append(" Suffix = @Suffix, ");
                sql.Append(" StageName = @StageName, ");
                sql.Append(" DateOfBirth = @DateOfBirth, ");
                sql.Append(" Height = @Height, ");
                sql.Append(" Weight = @Weight, ");
                sql.Append(" HairColorId = @HairColorId, ");
                sql.Append(" EyeColorId = @EyeColorId ");
                sql.Append("where PersonId = @PersonId");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);
                cmd.Parameters.AddWithValue("@PersonId", item.PersonId);

                cmd.ExecuteNonQuery();
            }
        }

        internal static void DeleteEntity(Person item, SqlConnection conn)
        {

            // Cascade delete Credits
            foreach (var credit in item.Credits)
            {
                CreditChildRepository.DeleteEntity(credit, conn);
            }

            // Cascade delete Attachments
            foreach (var attachment in item.Attachments)
            {
                PersonAttachmentChildRepository.DeleteEntity(attachment, conn);
            }

            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "delete Person where PersonId = @PersonId";
                cmd.Parameters.AddWithValue("@PersonId", item.PersonId);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SetCommonParameters(Person person, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@Salutation", person.Salutation);
            cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
            cmd.Parameters.AddWithValue("@MiddleName", person.MiddleName);
            cmd.Parameters.AddWithValue("@LastName", person.LastName);
            cmd.Parameters.AddWithValue("@Suffix", person.Suffix);
            cmd.Parameters.AddWithValue("@StageName", person.StageName);
            cmd.Parameters.AddWithValue("@DateOfBirth",
                person.DateOfBirth.AsSqlParameterValue());
            cmd.Parameters.AddWithValue("@Weight",
                person.Weight.AsSqlParameterValue());
            cmd.Parameters.AddWithValue("@Height",
                person.Height.AsSqlParameterValue());
            cmd.Parameters.AddWithValue("@HairColorId",
                person.HairColorId.AsSqlParameterValue());
            cmd.Parameters.AddWithValue("@EyeColorId",
                person.EyeColorId.AsSqlParameterValue());
        }

        #endregion
    }
}

