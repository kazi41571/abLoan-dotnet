using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanTraceMaster
    /// </summary>
    public class loanTraceMasterDAL
    {
        #region Properties
        public long TraceMasterId { get; set; }
        public string TableName { get; set; }
        public string OperationType { get; set; }
        public long RowId { get; set; }
        public string Value { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string ModifiedBy { get; set; }
        /// 

        #endregion

        #region Class Methods
        private List<loanTraceMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanTraceMasterDAL> lstTraceMaster = new List<loanTraceMasterDAL>();
            loanTraceMasterDAL objTraceMaster = null;
            while (sqlRdr.Read())
            {
                objTraceMaster = new loanTraceMasterDAL();
                objTraceMaster.TraceMasterId = Convert.ToInt64(sqlRdr["TraceMasterId"]);
                objTraceMaster.TableName = Convert.ToString(sqlRdr["TableName"]);
                objTraceMaster.OperationType = Convert.ToString(sqlRdr["OperationType"]);
                objTraceMaster.RowId = Convert.ToInt64(sqlRdr["RowId"]);
                objTraceMaster.Value = Convert.ToString(sqlRdr["Value"]);
                objTraceMaster.CreateDateTime = Convert.ToDateTime(sqlRdr["CreateDateTime"]);
                objTraceMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objTraceMaster.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);
                /// 
				lstTraceMaster.Add(objTraceMaster);
            }
            return lstTraceMaster;
        }
        #endregion

        #region Select
        public bool SelectTraceMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanTraceMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = this.TableName;
                SqlCmd.Parameters.Add("@RowId", SqlDbType.Int).Value = this.RowId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                bool IsSelected = false;
                if (SqlRdr.Read())
                {
                    this.TraceMasterId = Convert.ToInt32(SqlRdr["TraceMasterId"]);
                    this.TableName = Convert.ToString(SqlRdr["TableName"]);
                    this.OperationType = Convert.ToString(SqlRdr["OperationType"]);
                    this.RowId = Convert.ToInt32(SqlRdr["RowId"]);
                    this.Value = Convert.ToString(SqlRdr["Value"]);

                    IsSelected = true;
                }
                SqlRdr.Close();
                SqlCon.Close();

                return IsSelected;
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return false;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeDataReader(SqlRdr);
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion
        #region SelectAll
        public List<loanTraceMasterDAL> SelectAllTraceMasterPageWise(int startRowIndex, int pageSize, out int totalRecords, DateTime? operationDateFrom = null, DateTime? operationDateTo = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanTraceMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = this.TableName;
                SqlCmd.Parameters.Add("@OperationType", SqlDbType.VarChar).Value = this.OperationType;
                if (operationDateFrom != null)
                {
                    SqlCmd.Parameters.Add("@OperationDateFrom", SqlDbType.Date).Value = operationDateFrom;
                }
                if (operationDateTo != null)
                {
                    SqlCmd.Parameters.Add("@OperationDateTo", SqlDbType.Date).Value = operationDateTo;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanTraceMasterDAL> lstTraceMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstTraceMasterDAL;
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
