using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanCustomerPaymentMaster
    /// </summary>
    public class loanCustomerPaymentMasterDAL
    {
        #region Properties
        public int CustomerPaymentMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public int linktoCustomerMasterId { get; set; }
        public DateTime PaymentDate { get; set; }
        public int linktoPaymentTypeMasterId { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceNo { get; set; }
        public string ChequeNo { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? linktoBankMasterId { get; set; }
        public string Notes { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }
        public int linktoUserMasterId { get; set; }
        public string BankAccountNumber { get; set; }

        /// Extra
        public string Customer { get; set; }
        public string IdNo { get; set; }
        public bool? CustomerIsRedFlag { get; set; }
        public string PaymentType { get; set; }
        public string Bank { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ContractTitle { get; set; }
        public string Links { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Contracts { get; set; }
        public int linktoUserMasterIdVerifier { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.CustomerPaymentMasterId = Convert.ToInt32(sqlRdr["CustomerPaymentMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                this.PaymentDate = Convert.ToDateTime(sqlRdr["PaymentDate"]);
                this.linktoPaymentTypeMasterId = Convert.ToInt32(sqlRdr["linktoPaymentTypeMasterId"]);
                this.Amount = Convert.ToDecimal(sqlRdr["Amount"]);
                this.ReferenceNo = Convert.ToString(sqlRdr["ReferenceNo"]);
                this.ChequeNo = Convert.ToString(sqlRdr["ChequeNo"]);
                if (sqlRdr["ChequeDate"] != DBNull.Value)
                {
                    this.ChequeDate = Convert.ToDateTime(sqlRdr["ChequeDate"]);
                }
                if (sqlRdr["linktoBankMasterId"] != DBNull.Value)
                {
                    this.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                }
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                this.BankAccountNumber = Convert.ToString(sqlRdr["BankAccountNumber"]);

                /// Extra
                this.Customer = Convert.ToString(sqlRdr["Customer"]);
                this.IdNo = Convert.ToString(sqlRdr["IdNo"]);
                this.PaymentType = Convert.ToString(sqlRdr["PaymentType"]);
                this.Bank = Convert.ToString(sqlRdr["Bank"]);
                return true;
            }
            return false;
        }

        private List<loanCustomerPaymentMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanCustomerPaymentMasterDAL> lstCustomerPaymentMaster = new List<loanCustomerPaymentMasterDAL>();
            loanCustomerPaymentMasterDAL objCustomerPaymentMaster = null;
            while (sqlRdr.Read())
            {
                objCustomerPaymentMaster = new loanCustomerPaymentMasterDAL();
                objCustomerPaymentMaster.CustomerPaymentMasterId = Convert.ToInt32(sqlRdr["CustomerPaymentMasterId"]);
                objCustomerPaymentMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objCustomerPaymentMaster.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                objCustomerPaymentMaster.PaymentDate = Convert.ToDateTime(sqlRdr["PaymentDate"]);
                objCustomerPaymentMaster.linktoPaymentTypeMasterId = Convert.ToInt32(sqlRdr["linktoPaymentTypeMasterId"]);
                objCustomerPaymentMaster.Amount = Convert.ToDecimal(sqlRdr["Amount"]);
                objCustomerPaymentMaster.ReferenceNo = Convert.ToString(sqlRdr["ReferenceNo"]);
                objCustomerPaymentMaster.Contracts = Convert.ToString(sqlRdr["Contracts"]);
                objCustomerPaymentMaster.ChequeNo = Convert.ToString(sqlRdr["ChequeNo"]);
                if (sqlRdr["ChequeDate"] != DBNull.Value)
                {
                    objCustomerPaymentMaster.ChequeDate = Convert.ToDateTime(sqlRdr["ChequeDate"]);
                }
                if (sqlRdr["linktoBankMasterId"] != DBNull.Value)
                {
                    objCustomerPaymentMaster.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                }
                objCustomerPaymentMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objCustomerPaymentMaster.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objCustomerPaymentMaster.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                if (sqlRdr["CustomerIsRedFlag"] != DBNull.Value)
                {
                    objCustomerPaymentMaster.CustomerIsRedFlag = Convert.ToBoolean(sqlRdr["CustomerIsRedFlag"]);
                }
                objCustomerPaymentMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objCustomerPaymentMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objCustomerPaymentMaster.Customer = Convert.ToString(sqlRdr["Customer"]);
                objCustomerPaymentMaster.BankAccountNumber = Convert.ToString(sqlRdr["BankAccountNumber"]);
                objCustomerPaymentMaster.IdNo = Convert.ToString(sqlRdr["IdNo"]);
                objCustomerPaymentMaster.PaymentType = Convert.ToString(sqlRdr["PaymentType"]);
                objCustomerPaymentMaster.Bank = Convert.ToString(sqlRdr["Bank"]);
                objCustomerPaymentMaster.Links = Convert.ToString(sqlRdr["Links"]);
                objCustomerPaymentMaster.VerifiedBy = Convert.ToString(sqlRdr["VerifiedBy"]);
                objCustomerPaymentMaster.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);
                lstCustomerPaymentMaster.Add(objCustomerPaymentMaster);
            }
            return lstCustomerPaymentMaster;
        }

        private List<loanCustomerPaymentMasterDAL> SetListPropertiesFromSqlDataReaderPaymentReport(SqlDataReader sqlRdr)
        {
            List<loanCustomerPaymentMasterDAL> lstCustomerPaymentMaster = new List<loanCustomerPaymentMasterDAL>();
            loanCustomerPaymentMasterDAL objCustomerPaymentMaster = null;
            while (sqlRdr.Read())
            {
                objCustomerPaymentMaster = new loanCustomerPaymentMasterDAL();
                objCustomerPaymentMaster.Amount = Convert.ToDecimal(sqlRdr["TotalAmount"]);
                objCustomerPaymentMaster.Month = Convert.ToString(sqlRdr["PaymentMonth"]);
                objCustomerPaymentMaster.Year = Convert.ToString(sqlRdr["PaymentYear"]);

                lstCustomerPaymentMaster.Add(objCustomerPaymentMaster);
            }
            return lstCustomerPaymentMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertCustomerPaymentMaster(List<loanContractInstallmentTranDAL> lstContractInstallmentTranDAL, SqlConnection sqlCon = null, SqlTransaction sqlTran = null)
        {
            SqlConnection SqlCon = null;
            SqlTransaction SqlTran = null;
            SqlCommand SqlCmd = null;
            try
            {
                if (sqlCon == null)
                {
                    SqlCon = loanObjectFactoryDAL.CreateConnection();
                    SqlCon.Open();
                    SqlTran = SqlCon.BeginTransaction();
                    SqlCmd = new SqlCommand("loanCustomerPaymentMaster_Insert", SqlCon, SqlTran);
                }
                else
                {
                    SqlCmd = new SqlCommand("loanCustomerPaymentMaster_Insert", sqlCon, sqlTran);
                }
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerPaymentMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                SqlCmd.Parameters.Add("@PaymentDate", SqlDbType.Date).Value = this.PaymentDate;
                if (this.linktoPaymentTypeMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoPaymentTypeMasterId", SqlDbType.Int).Value = this.linktoPaymentTypeMasterId;
                }
                SqlCmd.Parameters.Add("@Amount", SqlDbType.Money).Value = this.Amount;
                SqlCmd.Parameters.Add("@ReferenceNo", SqlDbType.NVarChar).Value = this.ReferenceNo;
                SqlCmd.Parameters.Add("@ChequeNo", SqlDbType.NVarChar).Value = this.ChequeNo;
                SqlCmd.Parameters.Add("@ChequeDate", SqlDbType.Date).Value = this.ChequeDate;
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                this.CustomerPaymentMasterId = Convert.ToInt32(SqlCmd.Parameters["@CustomerPaymentMasterId"].Value);
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                if (rs != loanRecordStatus.Success)
                {
                    if (sqlCon == null)
                    {
                        SqlTran.Rollback();
                        SqlCon.Close();
                    }
                    return rs;
                }
                foreach (loanContractInstallmentTranDAL objContractInstallmentTranDAL in lstContractInstallmentTranDAL)
                {
                    objContractInstallmentTranDAL.linktoCustomerPaymentMasterId = this.CustomerPaymentMasterId;
                    if (sqlCon == null)
                    {
                        rs = objContractInstallmentTranDAL.InsertContractInstallmentTran(SqlCon, SqlTran);
                    }
                    else
                    {
                        rs = objContractInstallmentTranDAL.InsertContractInstallmentTran(sqlCon, sqlTran);
                    }
                    if (rs != loanRecordStatus.Success)
                    {
                        if (sqlCon == null)
                        {
                            SqlTran.Rollback();
                            SqlCon.Close();
                        }
                        return rs;
                    }
                }
                if (sqlCon == null)
                {

                    SqlTran.Commit();
                    SqlCon.Close();
                }
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
                    loanObjectFactoryDAL.DisposeTransaction(SqlTran);
                }
            }
        }
        #endregion

        #region Update
        public loanRecordStatus UpdateCustomerPaymentMaster(List<loanContractInstallmentTranDAL> lstContractInstallmentTranDAL)
        {
            SqlConnection SqlCon = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();
                loanRecordStatus rs = DeleteCustomerPaymentMaster(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }
                rs = InsertCustomerPaymentMaster(lstContractInstallmentTranDAL, SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }
                SqlTran.Commit();
                SqlCon.Close();
                return rs;

            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return loanRecordStatus.Error;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeTransaction(SqlTran);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion

        #region Delete
        public loanRecordStatus DeleteCustomerPaymentMaster(SqlConnection sqlCon = null, SqlTransaction sqlTran = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                if (sqlCon == null)
                {
                    SqlCon = loanObjectFactoryDAL.CreateConnection();
                    SqlCmd = new SqlCommand("loanCustomerPaymentMaster_Delete", SqlCon);
                }
                else
                {
                    SqlCmd = new SqlCommand("loanCustomerPaymentMaster_Delete", sqlCon, sqlTran);
                }
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerPaymentMasterId", SqlDbType.Int).Value = this.CustomerPaymentMasterId;
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

        #region Select
        public bool SelectCustomerPaymentMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerPaymentMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerPaymentMasterId", SqlDbType.Int).Value = this.CustomerPaymentMasterId;

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

        public List<loanCustomerPaymentMasterDAL> SelectAllCustomerPaymentMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerPaymentMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;
                SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;
                SqlCmd.Parameters.Add("@ContractTitle", SqlDbType.NVarChar).Value = this.ContractTitle;
                if (this.PaymentDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@PaymentDate", SqlDbType.Date).Value = this.PaymentDate;
                }
                if (this.linktoPaymentTypeMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoPaymentTypeMasterId", SqlDbType.Int).Value = this.linktoPaymentTypeMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                if (this.linktoUserMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                }
                if (this.linktoUserMasterIdVerifier > 0)
                {
                    SqlCmd.Parameters.Add("@linktoUserMasterIdVerifier", SqlDbType.Int).Value = this.linktoUserMasterIdVerifier;
                }
                if (this.Amount > 0)
                {
                    SqlCmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = this.Amount;
                }
                if (!string.IsNullOrWhiteSpace(this.ChequeNo))
                {
                    SqlCmd.Parameters.Add("@ChequeNo", SqlDbType.VarChar).Value = this.ChequeNo;
                }
                if (!string.IsNullOrWhiteSpace(this.Notes))
                {
                    SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                }

                //Extra
                SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;


                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCustomerPaymentMasterDAL> lstCustomerPaymentMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstCustomerPaymentMasterDAL;
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

        public List<loanCustomerPaymentMasterDAL> SelectMonthWisePaymentReceivedReport()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPaymentReceivedReportPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                //Extra
                SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCustomerPaymentMasterDAL> lstCustomerPaymentMasterDAL = SetListPropertiesFromSqlDataReaderPaymentReport(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstCustomerPaymentMasterDAL;
            }
            catch (Exception ex)
            {
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
