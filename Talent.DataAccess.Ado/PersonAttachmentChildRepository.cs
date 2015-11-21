using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talent.Domain;

namespace Talent.DataAccess.Ado
{
    internal class PersonAttachmentChildRepository
    {

        #region PersistChild

        public PersonAttachment PersistChild(PersonAttachment PersonAttachment, SqlConnection conn)
        {

            if (PersonAttachment.PersonAttachmentId == 0 && PersonAttachment.IsMarkedForDeletion)
            {
                PersonAttachment = null;
            }
            else if (PersonAttachment.IsMarkedForDeletion)
            {
                DeleteEntity(PersonAttachment, conn);
                PersonAttachment = null;
            }
            else if (PersonAttachment.PersonAttachmentId == 0)
            {
                InsertEntity(PersonAttachment, conn);
                PersonAttachment.IsDirty = false;
            }
            else if (PersonAttachment.IsDirty)
            {
                UpdateEntity(PersonAttachment, conn);
                PersonAttachment.IsDirty = false;
            }
            return PersonAttachment;

        }

        #endregion

        #region SQL

        internal static void InsertEntity(PersonAttachment item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("insert PersonAttachment (PersonId, Caption, FileBytes, FileName, FileExtension)");
                sql.Append("values (@PersonId, @Caption, @FileBytes, @FileName, @FileExtension);");
                sql.Append("select cast ( scope_identity() as int);");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);

                item.PersonAttachmentId = (int)cmd.ExecuteScalar();
            }
        }

        internal static void UpdateEntity(PersonAttachment item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                var sql = new StringBuilder();
                sql.Append("update PersonAttachment set ");
                sql.Append(" PersonId = @PersonId, ");
                sql.Append(" Caption = @Caption, ");
                sql.Append(" FileName = @FileName ");
                sql.Append(" FileBytes = @FileBytes ");
                sql.Append("where PersonAttachmentId = @PersonAttachmentId ");
                cmd.CommandText = sql.ToString();

                SetCommonParameters(item, cmd);
                cmd.Parameters.AddWithValue("@PersonAttachmentId", item.PersonAttachmentId);

                cmd.ExecuteNonQuery();
            }
        }

        internal static void DeleteEntity(PersonAttachment item, SqlConnection conn)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "delete PersonAttachment where PersonAttachmentId = @PersonAttachmentId";
                cmd.Parameters.AddWithValue("@PersonAttachmentId", item.PersonAttachmentId);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SetCommonParameters(PersonAttachment item, SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@PersonId", item.PersonId);
            cmd.Parameters.AddWithValue("@Caption", item.Caption);
            cmd.Parameters.AddWithValue("@FileName", item.FileName);
            cmd.Parameters.AddWithValue("@FileExtension", item.FileExtension);
            cmd.Parameters.AddWithValue("@FileBytes", item.FileBytes);
        }

        #endregion

    }
}
