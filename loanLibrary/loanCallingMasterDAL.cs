using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanCallingMaster
    /// </summary>
    public class loanCallingMasterDAL
    {
        #region Properties
        public int CallingMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public DateTime CallingDate { get; set; }
        public string CallingName { get; set; }
        public int linktoCustomerMasterId { get; set; }
        public int? linktoBankMasterId { get; set; }
        public decimal? Amount { get; set; }
        public string Notes { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string Customer { get; set; }
        public string Bank { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.CallingMasterId = Convert.ToInt32(sqlRdr["CallingMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.CallingDate = Convert.ToDateTime(sqlRdr["CallingDate"]);
                this.CallingName = Convert.ToString(sqlRdr["CallingName"]);
                this.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                if (sqlRdr["linktoBankMasterId"] != DBNull.Value)
                {
                    this.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                }
                if (sqlRdr["Amount"] != DBNull.Value)
                {
                    this.Amount = Convert.ToDecimal(sqlRdr["Amount"]);
                }
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.Customer = Convert.ToString(sqlRdr["Customer"]);
                this.Bank = Convert.ToString(sqlRdr["Bank"]);
                return true;
            }
            return false;
        }

        private List<loanCallingMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanCallingMasterDAL> lstCallingMaster = new List<loanCallingMasterDAL>();
            loanCallingMasterDAL objCallingMaster = null;
            while (sqlRdr.Read())
            {
                objCallingMaster = new loanCallingMasterDAL();
                objCallingMaster.CallingMasterId = Convert.ToInt32(sqlRdr["CallingMasterId"]);
                objCallingMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objCallingMaster.CallingDate = Convert.ToDateTime(sqlRdr["CallingDate"]);
                objCallingMaster.CallingName = Convert.ToString(sqlRdr["CallingName"]);
                objCallingMaster.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                if (sqlRdr["linktoBankMasterId"] != DBNull.Value)
                {
                    objCallingMaster.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                }
                if (sqlRdr["Amount"] != DBNull.Value)
                {
                    objCallingMaster.Amount = Convert.ToDecimal(sqlRdr["Amount"]);
                }
                objCallingMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                objCallingMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objCallingMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objCallingMaster.Customer = Convert.ToString(sqlRdr["Customer"]);
                objCallingMaster.Bank = Convert.ToString(sqlRdr["Bank"]);
                lstCallingMaster.Add(objCallingMaster);
            }
            return lstCallingMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertCallingMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCallingMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CallingMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@CallingDate", SqlDbType.Date).Value = this.CallingDate;
                SqlCmd.Parameters.Add("@CallingName", SqlDbType.NVarChar).Value = this.CallingName;
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                SqlCmd.Parameters.Add("@Amount", SqlDbType.Money).Value = this.Amount;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.CallingMasterId = Convert.ToInt32(SqlCmd.Parameters["@CallingMasterId"].Value);
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
        public loanRecordStatus UpdateCallingMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCallingMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CallingMasterId", SqlDbType.Int).Value = this.CallingMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@CallingDate", SqlDbType.Date).Value = this.CallingDate;
                SqlCmd.Parameters.Add("@CallingName", SqlDbType.NVarChar).Value = this.CallingName;
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                SqlCmd.Parameters.Add("@Amount", SqlDbType.Money).Value = this.Amount;
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

        #region Delete
        public loanRecordStatus DeleteCallingMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCallingMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CallingMasterId", SqlDbType.Int).Value = this.CallingMasterId;
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

        #region Select
        public bool SelectCallingMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCallingMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CallingMasterId", SqlDbType.Int).Value = this.CallingMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                bool IsSelected = SetClassPropertiesFromSqlDataReader(SqlRdr);
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

        public List<loanCallingMasterDAL> SelectAllCallingMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCallingMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.CallingDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@CallingDate", SqlDbType.Date).Value = this.CallingDate;
                }
                SqlCmd.Parameters.Add("@CallingName", SqlDbType.NVarChar).Value = this.CallingName;
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCallingMasterDAL> lstCallingMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstCallingMasterDAL;
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
