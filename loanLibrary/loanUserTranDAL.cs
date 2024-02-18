using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanUserTran
    /// </summary>
    public class loanUserTranDAL
    {
        #region Properties
        public int UserTranId { get; set; }
        public int linktoUserMasterId { get; set; }
        public string SessionId { get; set; }
        public DateTime LoginDateTime { get; set; }
        public DateTime? LogoutDateTime { get; set; }
        public string OS { get; set; }
        public string IPAddress { get; set; }
        public string DeviceName { get; set; }
        public string Browser { get; set; }

        /// Extra
        public string Username { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.UserTranId = Convert.ToInt32(sqlRdr["UserTranId"]);
                this.linktoUserMasterId = Convert.ToInt32(sqlRdr["linktoUserMasterId"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                this.LoginDateTime = Convert.ToDateTime(sqlRdr["LoginDateTime"]);
                if (sqlRdr["LogoutDateTime"] != DBNull.Value)
                {
                    this.LogoutDateTime = Convert.ToDateTime(sqlRdr["LogoutDateTime"]);
                }
                this.OS = Convert.ToString(sqlRdr["OS"]);
                this.IPAddress = Convert.ToString(sqlRdr["IPAddress"]);
                this.DeviceName = Convert.ToString(sqlRdr["DeviceName"]);
                this.Browser = Convert.ToString(sqlRdr["Browser"]);

                /// Extra
                this.Username = Convert.ToString(sqlRdr["Username"]);
                return true;
            }
            return false;
        }

        private List<loanUserTranDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanUserTranDAL> lstUserTranDAL = new List<loanUserTranDAL>();
            loanUserTranDAL objUserTranDAL = null;
            while (sqlRdr.Read())
            {
                objUserTranDAL = new loanUserTranDAL();
                objUserTranDAL.UserTranId = Convert.ToInt32(sqlRdr["UserTranId"]);
                objUserTranDAL.linktoUserMasterId = Convert.ToInt32(sqlRdr["linktoUserMasterId"]);
                objUserTranDAL.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                objUserTranDAL.LoginDateTime = Convert.ToDateTime(sqlRdr["LoginDateTime"]);
                if (sqlRdr["LogoutDateTime"] != DBNull.Value)
                {
                    objUserTranDAL.LogoutDateTime = Convert.ToDateTime(sqlRdr["LogoutDateTime"]);
                }
                objUserTranDAL.OS = Convert.ToString(sqlRdr["OS"]);
                objUserTranDAL.IPAddress = Convert.ToString(sqlRdr["IPAddress"]);
                objUserTranDAL.DeviceName = Convert.ToString(sqlRdr["DeviceName"]);
                objUserTranDAL.Browser = Convert.ToString(sqlRdr["Browser"]);

                /// Extra
                objUserTranDAL.Username = Convert.ToString(sqlRdr["Username"]);
                lstUserTranDAL.Add(objUserTranDAL);
            }
            return lstUserTranDAL;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertUserTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserTran_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoUserMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                }
                SqlCmd.Parameters.Add("@LoginDateTime", SqlDbType.DateTime).Value = this.LoginDateTime;
                SqlCmd.Parameters.Add("@LogoutDateTime", SqlDbType.DateTime).Value = this.LogoutDateTime;
                SqlCmd.Parameters.Add("@OS", SqlDbType.VarChar).Value = this.OS;
                SqlCmd.Parameters.Add("@IPAddress", SqlDbType.VarChar).Value = this.IPAddress;
                SqlCmd.Parameters.Add("@DeviceName", SqlDbType.VarChar).Value = this.DeviceName;
                SqlCmd.Parameters.Add("@Browser", SqlDbType.VarChar).Value = this.Browser;

                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar, 36).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.UserTranId = Convert.ToInt32(SqlCmd.Parameters["@UserTranId"].Value);
                this.SessionId = Convert.ToString(SqlCmd.Parameters["@SessionId"].Value);
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

        #region Update
        public loanRecordStatus UpdateUserTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserTran_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@LogoutDateTime", SqlDbType.DateTime).Value = this.LogoutDateTime;

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

        #region SelectAll

        public List<loanUserTranDAL> SelectAllUserTranPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserTranPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.linktoUserMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanUserTranDAL> lstUserTranDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstUserTranDAL;
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
        #endregion
    }
}
