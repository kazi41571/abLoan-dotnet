using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanContractMaster
    /// </summary>
    public class loanFollowupNoteDAL
    {
        #region Properties
        public int FollowupNoteId { get; set; }
        public int linktoCustomerFollowupId { get; set; }
        public string Notes { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        public string Username { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public int? linktoUserMasterId { get; set; }


        public int totalRowCount { get; set; }


        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.FollowupNoteId = Convert.ToInt32(sqlRdr["FollowupNoteId"]);
                this.linktoCustomerFollowupId = Convert.ToInt32(sqlRdr["linktoCustomerFollowupId"]);
                this.linktoUserMasterId = Convert.ToInt32(sqlRdr["linktoUserMasterId"]);
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                this.Username = Convert.ToString(sqlRdr["Username"]);

                /// Extra

                return true;
            }
            return false;
        }

        private List<loanFollowupNoteDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanFollowupNoteDAL> lstFollowupNote = new List<loanFollowupNoteDAL>();
            loanFollowupNoteDAL objFollowupNote = null;
            while (sqlRdr.Read())
            {
                objFollowupNote = new loanFollowupNoteDAL();
                objFollowupNote.FollowupNoteId = Convert.ToInt32(sqlRdr["FollowupNoteId"]);
                objFollowupNote.linktoCustomerFollowupId = Convert.ToInt32(sqlRdr["linktoCustomerFollowupId"]);

                objFollowupNote.Notes = Convert.ToString(sqlRdr["Notes"]);

                objFollowupNote.Username = Convert.ToString(sqlRdr["Username"]);


                if (sqlRdr["linktoUserMasterId"] != DBNull.Value)
                {
                    objFollowupNote.linktoUserMasterId = Convert.ToInt32(sqlRdr["linktoUserMasterId"]);
                }
                objFollowupNote.CreateDateTime = Convert.ToDateTime(sqlRdr["CreateDateTime"]);
                objFollowupNote.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objFollowupNote.SessionId = Convert.ToString(sqlRdr["SessionId"]);


                lstFollowupNote.Add(objFollowupNote);
            }
            return lstFollowupNote;
        }
        #endregion



        #region  Notes
        public loanRecordStatus InsertCustomerfollowupNotes()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowupNote_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@FollowupNoteId", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@linktoCustomerFollowupId", SqlDbType.Int).Value = this.linktoCustomerFollowupId;
                SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();


                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                return rs;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return loanRecordStatus.Error;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }



        public loanRecordStatus UpdateCustomerfollowupNotes()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowupNote_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;



                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();


                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                return rs;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return loanRecordStatus.Error;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion


        #region  Notes
        public loanRecordStatus DeleteCustomerfollowupNote()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowupNote_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@FollowupNoteId", SqlDbType.Int).Value = this.FollowupNoteId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;


                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();


                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                return rs;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return loanRecordStatus.Error;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion

        public List<loanFollowupNoteDAL> SelectFollowupNote(out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowupNote_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.linktoCustomerFollowupId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerFollowupId", SqlDbType.Int).Value = this.linktoCustomerFollowupId;
                }
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanFollowupNoteDAL> lstFollowupNoteDAL =
                    SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                totalRowCount = totalRecords;


                return lstFollowupNoteDAL;
            }
            catch (Exception ex)
            {
                totalRecords = 0;
                loanGlobalsDAL.SaveError(ex);
                return null;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeDataReader(SqlRdr);
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }


    }
}

