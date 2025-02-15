using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanChequeMaster
    /// </summary>
    public class loanChequeMasterDAL
    {
        #region Properties
        public int ChequeMasterId { get; set; }
        public string ChequeName { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public int linktoBankMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public int linktoCustomerMasterId { get; set; }
        public decimal ChequeAmount { get; set; }
        public string GivenTo { get; set; }
        public string Notes { get; set; }
        public string Others { get; set; }

        public int linktoChequeStatusMasterId { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string Bank { get; set; }
        public string Company { get; set; }
        public string Customer { get; set; }
        public bool? CustomerIsRedFlag { get; set; }
        public string ChequeStatus { get; set; }
        public int NoOfContracts { get; set; }
        public decimal TotalInstallmentAmount { get; set; }
        public bool IsSelected { get; set; }
        public string BankIds { get; set; }
        public string CustomerLinks { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.ChequeMasterId = Convert.ToInt32(sqlRdr["ChequeMasterId"]);
                this.ChequeName = Convert.ToString(sqlRdr["ChequeName"]);
                this.ChequeNo = Convert.ToString(sqlRdr["ChequeNo"]);
                this.ChequeDate = Convert.ToDateTime(sqlRdr["ChequeDate"]);
                this.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);

                this.ChequeAmount = Convert.ToDecimal(sqlRdr["ChequeAmount"]);
                this.GivenTo = Convert.ToString(sqlRdr["GivenTo"]);
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.Others = Convert.ToString(sqlRdr["Others"]);

                this.linktoChequeStatusMasterId = Convert.ToInt32(sqlRdr["linktoChequeStatusMasterId"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.Bank = Convert.ToString(sqlRdr["Bank"]);
                this.Company = Convert.ToString(sqlRdr["Company"]);
                this.Customer = Convert.ToString(sqlRdr["Customer"]);
                this.ChequeStatus = Convert.ToString(sqlRdr["ChequeStatus"]);

                return true;
            }
            return false;
        }

        private List<loanChequeMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanChequeMasterDAL> lstChequeMaster = new List<loanChequeMasterDAL>();
            loanChequeMasterDAL objChequeMaster = null;
            while (sqlRdr.Read())
            {
                objChequeMaster = new loanChequeMasterDAL();
                objChequeMaster.ChequeMasterId = Convert.ToInt32(sqlRdr["ChequeMasterId"]);
                objChequeMaster.ChequeName = Convert.ToString(sqlRdr["ChequeName"]);
                objChequeMaster.ChequeNo = Convert.ToString(sqlRdr["ChequeNo"]);
                objChequeMaster.ChequeDate = Convert.ToDateTime(sqlRdr["ChequeDate"]);
                objChequeMaster.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                objChequeMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objChequeMaster.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);

                objChequeMaster.ChequeAmount = Convert.ToDecimal(sqlRdr["ChequeAmount"]);
                objChequeMaster.GivenTo = Convert.ToString(sqlRdr["GivenTo"]);
                objChequeMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                objChequeMaster.Others = Convert.ToString(sqlRdr["Others"]);


                objChequeMaster.linktoChequeStatusMasterId = Convert.ToInt32(sqlRdr["linktoChequeStatusMasterId"]);
                objChequeMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objChequeMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objChequeMaster.Bank = Convert.ToString(sqlRdr["Bank"]);
                objChequeMaster.Company = Convert.ToString(sqlRdr["Company"]);
                objChequeMaster.Customer = Convert.ToString(sqlRdr["Customer"]);
                if (sqlRdr["CustomerIsRedFlag"] != DBNull.Value)
                {
                    objChequeMaster.CustomerIsRedFlag = Convert.ToBoolean(sqlRdr["CustomerIsRedFlag"]);
                }
                objChequeMaster.ChequeStatus = Convert.ToString(sqlRdr["ChequeStatus"]);
                lstChequeMaster.Add(objChequeMaster);
            }
            return lstChequeMaster;
        }

        private List<loanChequeMasterDAL> SetListPropertiesFromSqlDataReaderCustomer(SqlDataReader sqlRdr)
        {
            List<loanChequeMasterDAL> lstChequeMaster = new List<loanChequeMasterDAL>();
            loanChequeMasterDAL objChequeMaster = null;
            while (sqlRdr.Read())
            {
                objChequeMaster = new loanChequeMasterDAL();

                objChequeMaster.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                objChequeMaster.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                objChequeMaster.NoOfContracts = Convert.ToInt32(sqlRdr["NoOfContracts"]);
                objChequeMaster.TotalInstallmentAmount = Convert.ToDecimal(sqlRdr["TotalInstallmentAmount"]);

                /// Extra
                objChequeMaster.Bank = Convert.ToString(sqlRdr["Bank"]);
                objChequeMaster.Customer = Convert.ToString(sqlRdr["Customer"]);
                if (sqlRdr["CustomerIsRedFlag"] != DBNull.Value)
                {
                    objChequeMaster.CustomerIsRedFlag = Convert.ToBoolean(sqlRdr["CustomerIsRedFlag"]);
                }
                if (sqlRdr["ChequeMasterId"] != DBNull.Value)
                {
                    objChequeMaster.ChequeMasterId = Convert.ToInt32(sqlRdr["ChequeMasterId"]);
                }
                if (sqlRdr["ChequeNo"] != DBNull.Value)
                {
                    objChequeMaster.ChequeNo = Convert.ToString(sqlRdr["ChequeNo"]);
                }
                if (sqlRdr["ChequeAmount"] != DBNull.Value)
                {
                    objChequeMaster.ChequeAmount = Convert.ToDecimal(sqlRdr["ChequeAmount"]);
                }
                if (sqlRdr["ChequeName"] != DBNull.Value)
                {
                    objChequeMaster.ChequeName = Convert.ToString(sqlRdr["ChequeName"]);
                }
                if (sqlRdr["ChequeDate"] != DBNull.Value)
                {
                    objChequeMaster.ChequeDate = Convert.ToDateTime(sqlRdr["ChequeDate"]);
                }
                if (sqlRdr["GivenTo"] != DBNull.Value)
                {
                    objChequeMaster.GivenTo = Convert.ToString(sqlRdr["GivenTo"]);
                }
                if (sqlRdr["Notes"] != DBNull.Value)
                {
                    objChequeMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                }
                if (sqlRdr["Others"] != DBNull.Value)
                {
                    objChequeMaster.Others = Convert.ToString(sqlRdr["Others"]);
                }
                if (sqlRdr["linktoChequeStatusMasterId"] != DBNull.Value)
                {
                    objChequeMaster.linktoChequeStatusMasterId = Convert.ToInt32(sqlRdr["linktoChequeStatusMasterId"]);
                }
                objChequeMaster.CustomerLinks = Convert.ToString(sqlRdr["CustomerLinks"]);
                lstChequeMaster.Add(objChequeMaster);
            }
            return lstChequeMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertChequeMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanChequeMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ChequeMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@ChequeName", SqlDbType.NVarChar).Value = this.ChequeName;
                SqlCmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = this.ChequeNo;
                SqlCmd.Parameters.Add("@ChequeDate", SqlDbType.Date).Value = this.ChequeDate;
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }

                SqlCmd.Parameters.Add("@ChequeAmount", SqlDbType.Money).Value = this.ChequeAmount;
                SqlCmd.Parameters.Add("@GivenTo", SqlDbType.NVarChar).Value = this.GivenTo;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@Others", SqlDbType.NVarChar).Value = this.Others;

                if (this.linktoChequeStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoChequeStatusMasterId", SqlDbType.Int).Value = this.linktoChequeStatusMasterId;
                }
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.ChequeMasterId = Convert.ToInt32(SqlCmd.Parameters["@ChequeMasterId"].Value);
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
        public loanRecordStatus UpdateChequeMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanChequeMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ChequeMasterId", SqlDbType.Int).Value = this.ChequeMasterId;
                SqlCmd.Parameters.Add("@ChequeName", SqlDbType.NVarChar).Value = this.ChequeName;
                SqlCmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = this.ChequeNo;
                SqlCmd.Parameters.Add("@ChequeDate", SqlDbType.Date).Value = this.ChequeDate;
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }

                SqlCmd.Parameters.Add("@ChequeAmount", SqlDbType.Money).Value = this.ChequeAmount;
                SqlCmd.Parameters.Add("@GivenTo", SqlDbType.NVarChar).Value = this.GivenTo;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@Others", SqlDbType.NVarChar).Value = this.Others;


                if (this.linktoChequeStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoChequeStatusMasterId", SqlDbType.Int).Value = this.linktoChequeStatusMasterId;
                }
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
        public loanRecordStatus DeleteChequeMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanChequeMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ChequeMasterId", SqlDbType.Int).Value = this.ChequeMasterId;
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
        public bool SelectChequeMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanChequeMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ChequeMasterId", SqlDbType.Int).Value = this.ChequeMasterId;

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

        public List<loanChequeMasterDAL> SelectAllChequeMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanChequeMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = this.ChequeNo;
                //if (this.linktoBankMasterId > 0)
                //{
                //	SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                //}
                if (this.BankIds != null)
                {
                    SqlCmd.Parameters.Add("@BankIds", SqlDbType.VarChar).Value = this.BankIds;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                if (this.linktoChequeStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoChequeStatusMasterId", SqlDbType.Int).Value = this.linktoChequeStatusMasterId;
                }
                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanChequeMasterDAL> lstChequeMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstChequeMasterDAL;
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

        public List<loanChequeMasterDAL> SelectAllChequeMasterCustomerContractPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanChequeMasterCustomerContractPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = this.ChequeNo;
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;
                SqlCmd.Parameters.Add("@BankIds", SqlDbType.NVarChar).Value = this.BankIds;


                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanChequeMasterDAL> lstChequeMasterDAL = SetListPropertiesFromSqlDataReaderCustomer(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstChequeMasterDAL;
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
