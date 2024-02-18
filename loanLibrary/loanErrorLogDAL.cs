using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loanLibrary
{
    public class loanErrorLogDAL
    {
        #region Properties
        public int ErrorLogId { get; set; }
        public DateTime ErrorDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStackTrace { get; set; }
        #endregion

        #region Class Methods
        private List<loanErrorLogDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanErrorLogDAL> lstErrorLog = new List<loanErrorLogDAL>();
            loanErrorLogDAL objErrorLog = null;
            while (sqlRdr.Read())
            {
                objErrorLog = new loanErrorLogDAL();
                objErrorLog.ErrorLogId = Convert.ToInt32(sqlRdr["ErrorLogId"]);
                objErrorLog.ErrorDateTime = Convert.ToDateTime(sqlRdr["ErrorDateTime"]);
                objErrorLog.ErrorMessage = Convert.ToString(sqlRdr["ErrorMessage"]);
                objErrorLog.ErrorStackTrace = Convert.ToString(sqlRdr["ErrorStackTrace"]);
                lstErrorLog.Add(objErrorLog);
            }
            return lstErrorLog;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertErrorLog()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanErrorLog_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ErrorLogId", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@ErrorDateTime", SqlDbType.DateTime).Value = this.ErrorDateTime;
                SqlCmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar).Value = this.ErrorMessage;
                SqlCmd.Parameters.Add("@ErrorStackTrace", SqlDbType.VarChar).Value = this.ErrorStackTrace;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.ErrorLogId = Convert.ToInt32(SqlCmd.Parameters["@ErrorLogId"].Value);
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                return rs;
            }
            catch
            {
                return loanRecordStatus.Error;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion

        #region Delete
        public loanRecordStatus DeleteErrorLog()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanErrorLog_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ErrorLogId", SqlDbType.Int).Value = this.ErrorLogId;
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

        #region DeleteAll
        public static loanRecordStatus DeleteAllErrorLog(string ids)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanErrorLog_DeleteAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ids", SqlDbType.VarChar).Value = ids;
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
        public List<loanErrorLogDAL> SelectAllErrorLogPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanErrorLogPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.ErrorLogId > 0)
                {
                    SqlCmd.Parameters.Add("@ErrorLogId", SqlDbType.Int).Value = this.ErrorLogId;
                }
                if (this.ErrorDateTime != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ErrorDateTime", SqlDbType.DateTime).Value = this.ErrorDateTime;
                }
                SqlCmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar).Value = this.ErrorMessage;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanErrorLogDAL> lstErrorLogDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstErrorLogDAL;
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
