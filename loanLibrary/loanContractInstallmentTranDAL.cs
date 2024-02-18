using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanContractInstallmentTran
    /// </summary>
    public class loanContractInstallmentTranDAL
    {
        #region Properties
        public int ContractInstallmentTranId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public int linktoContractMasterId { get; set; }
        public DateTime InstallmentDate { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int linktoCustomerPaymentMasterId { get; set; }
        public string Notes { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string Contract { get; set; }
        public string Customer { get; set; }
        public bool? CustomerIsRedFlag { get; set; }
        public string Bank { get; set; }
        public string ContractTitle { get; set; }
        public int linktoBankMasterId { get; set; }
        public int linktoCustomerMasterId { get; set; }
        public string PaymentType { get; set; }
        public int ContractMasterId { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string CustomerLinks { get; set; }
        public string ContractLinks { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.ContractInstallmentTranId = Convert.ToInt32(sqlRdr["ContractInstallmentTranId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.linktoContractMasterId = Convert.ToInt32(sqlRdr["linktoContractMasterId"]);
                this.InstallmentDate = Convert.ToDateTime(sqlRdr["InstallmentDate"]);
                this.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                this.linktoCustomerPaymentMasterId = Convert.ToInt32(sqlRdr["linktoCustomerPaymentMasterId"]);
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.Contract = Convert.ToString(sqlRdr["Contract"]);

                return true;
            }
            return false;
        }

        private List<loanContractInstallmentTranDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanContractInstallmentTranDAL> lstContractInstallmentTran = new List<loanContractInstallmentTranDAL>();
            loanContractInstallmentTranDAL objContractInstallmentTran = null;
            while (sqlRdr.Read())
            {
                objContractInstallmentTran = new loanContractInstallmentTranDAL();
                objContractInstallmentTran.ContractInstallmentTranId = Convert.ToInt32(sqlRdr["ContractInstallmentTranId"]);
                objContractInstallmentTran.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objContractInstallmentTran.linktoContractMasterId = Convert.ToInt32(sqlRdr["linktoContractMasterId"]);
                objContractInstallmentTran.InstallmentDate = Convert.ToDateTime(sqlRdr["InstallmentDate"]);
                objContractInstallmentTran.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                objContractInstallmentTran.linktoCustomerPaymentMasterId = Convert.ToInt32(sqlRdr["linktoCustomerPaymentMasterId"]);
                objContractInstallmentTran.Notes = Convert.ToString(sqlRdr["Notes"]);
                objContractInstallmentTran.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objContractInstallmentTran.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objContractInstallmentTran.Contract = Convert.ToString(sqlRdr["Contract"]);
                objContractInstallmentTran.Customer = Convert.ToString(sqlRdr["Customer"]);
                if (sqlRdr["CustomerIsRedFlag"] != DBNull.Value)
                {
                    objContractInstallmentTran.CustomerIsRedFlag = Convert.ToBoolean(sqlRdr["CustomerIsRedFlag"]);
                }
                objContractInstallmentTran.Bank = Convert.ToString(sqlRdr["Bank"]);
                objContractInstallmentTran.PaymentType = Convert.ToString(sqlRdr["PaymentType"]);
                objContractInstallmentTran.ChequeNo = Convert.ToString(sqlRdr["ChequeNo"]);
                if (sqlRdr["ChequeDate"] != DBNull.Value)
                {
                    objContractInstallmentTran.ChequeDate = Convert.ToDateTime(sqlRdr["ChequeDate"]);
                }
                objContractInstallmentTran.ContractMasterId = Convert.ToInt32(sqlRdr["linktoContractMasterId"]);
                objContractInstallmentTran.CustomerLinks = Convert.ToString(sqlRdr["CustomerLinks"]);
                objContractInstallmentTran.ContractLinks = Convert.ToString(sqlRdr["ContractLinks"]);
                lstContractInstallmentTran.Add(objContractInstallmentTran);
            }
            return lstContractInstallmentTran;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertContractInstallmentTran(SqlConnection sqlCon = null, SqlTransaction sqlTran = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                if (sqlCon == null)
                {
                    SqlCon = loanObjectFactoryDAL.CreateConnection();
                    SqlCmd = new SqlCommand("loanContractInstallmentTran_Insert", SqlCon);
                }
                else
                {
                    SqlCmd = new SqlCommand("loanContractInstallmentTran_Insert", sqlCon, sqlTran);
                }
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractInstallmentTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                }
                SqlCmd.Parameters.Add("@InstallmentDate", SqlDbType.Date).Value = this.InstallmentDate;
                SqlCmd.Parameters.Add("@InstallmentAmount", SqlDbType.Decimal).Value = this.InstallmentAmount;
                if (this.linktoCustomerPaymentMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerPaymentMasterId", SqlDbType.Int).Value = this.linktoCustomerPaymentMasterId;
                }
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                if (sqlCon == null)
                {
                    SqlCon.Open();
                }
                SqlCmd.ExecuteNonQuery();
                if (sqlCon == null)
                {
                    SqlCon.Close();
                }

                this.ContractInstallmentTranId = Convert.ToInt32(SqlCmd.Parameters["@ContractInstallmentTranId"].Value);
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
                if (sqlCon == null)
                {
                    loanObjectFactoryDAL.DisposeConnection(SqlCon);
                }
            }
        }
        #endregion

        #region Update
        public loanRecordStatus UpdateContractInstallmentTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractInstallmentTran_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractInstallmentTranId", SqlDbType.Int).Value = this.ContractInstallmentTranId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                }
                SqlCmd.Parameters.Add("@InstallmentDate", SqlDbType.Date).Value = this.InstallmentDate;
                SqlCmd.Parameters.Add("@InstallmentAmount", SqlDbType.Decimal).Value = this.InstallmentAmount;
                if (this.linktoCustomerPaymentMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerPaymentMasterId", SqlDbType.Int).Value = this.linktoCustomerPaymentMasterId;
                }
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
        public loanRecordStatus DeleteContractInstallmentTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractInstallmentTran_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractInstallmentTranId", SqlDbType.Int).Value = this.ContractInstallmentTranId;
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
        public bool SelectContractInstallmentTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractInstallmentTran_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractInstallmentTranId", SqlDbType.Int).Value = this.ContractInstallmentTranId;

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

        public List<loanContractInstallmentTranDAL> SelectAllContractInstallmentTranPageWise(int startRowIndex, int pageSize, out int totalRecords, DateTime? installmentDateFrom = null, DateTime? installmentDateTo = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractInstallmentTranPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                if (this.linktoCustomerPaymentMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerPaymentMasterId", SqlDbType.Int).Value = this.linktoCustomerPaymentMasterId;
                }
                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;
                SqlCmd.Parameters.Add("@ContractTitle", SqlDbType.NVarChar).Value = this.ContractTitle;

                if (this.InstallmentDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@InstallmentDate", SqlDbType.Date).Value = installmentDateFrom;
                }
                if (installmentDateTo != null)
                {
                    SqlCmd.Parameters.Add("@InstallmentDateTo", SqlDbType.Date).Value = installmentDateTo;
                }
                if ((this.linktoCompanyMasterId > 0))
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }


                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanContractInstallmentTranDAL> lstContractInstallmentTranDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstContractInstallmentTranDAL;
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
