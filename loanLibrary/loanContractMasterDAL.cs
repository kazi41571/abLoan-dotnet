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
    public class loanContractMasterDAL
    {
        #region Properties
        public int ContractMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public int linktoCustomerMasterId { get; set; }
        public string ContractTitle { get; set; }
        public int linktoBankMasterId { get; set; }
        public int FileNo { get; set; }
        public DateTime ContractDate { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime InstallmentDate { get; set; }
        public int linktoContractStatusMasterId { get; set; }
        public decimal ContractAmount { get; set; }
        public bool IsBasedOnMonth { get; set; }
        public int NoOfInstallments { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal InterestRate { get; set; }
        public decimal DownPayment { get; set; }
        public decimal SettlementAmount { get; set; }
        public string SettlementReason { get; set; }
        public string Notes { get; set; }
        public string Links { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public int? linktoUserMasterIdCreatedBy { get; set; }
        public bool? HasURL { get; set; }

        /// Extra
        public string Customer { get; set; }
        public string CustomerIdNo { get; set; }
        public string ContractStatus { get; set; }
        public bool? CustomerIsRedFlag { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime? LastPaidDate { get; set; }
        public decimal? LastPaidAmount { get; set; }
        public string Bank { get; set; }
        public decimal DueAmount { get; set; }
        public string Last3Installments { get; set; }
        public string CustomerLinks { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string GuarantorName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountNumber2 { get; set; }
        public string BankAccountNumber3 { get; set; }
        public string BankAccountNumber4 { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int? DueInstallments { get; set; }
        public string CustomerAddress { get; set; }


        public int? linktoUserMasterId { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.ContractMasterId = Convert.ToInt32(sqlRdr["ContractMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                this.ContractTitle = Convert.ToString(sqlRdr["ContractTitle"]);
                this.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                this.FileNo = Convert.ToInt32(sqlRdr["FileNo"]);
                this.ContractDate = Convert.ToDateTime(sqlRdr["ContractDate"]);
                this.ContractStartDate = Convert.ToDateTime(sqlRdr["ContractStartDate"]);
                this.InstallmentDate = Convert.ToDateTime(sqlRdr["InstallmentDate"]);
                this.linktoContractStatusMasterId = Convert.ToInt32(sqlRdr["linktoContractStatusMasterId"]);
                this.ContractAmount = Convert.ToDecimal(sqlRdr["ContractAmount"]);
                this.IsBasedOnMonth = Convert.ToBoolean(sqlRdr["IsBasedOnMonth"]);
                this.NoOfInstallments = Convert.ToInt32(sqlRdr["NoOfInstallments"]);
                this.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                this.InterestRate = Convert.ToDecimal(sqlRdr["InterestRate"]);
                this.DownPayment = Convert.ToDecimal(sqlRdr["DownPayment"]);
                this.SettlementAmount = Convert.ToDecimal(sqlRdr["SettlementAmount"]);
                this.SettlementReason = Convert.ToString(sqlRdr["SettlementReason"]);
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.Links = Convert.ToString(sqlRdr["Links"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.Customer = Convert.ToString(sqlRdr["Customer"]);
                this.CustomerIdNo = Convert.ToString(sqlRdr["CustomerIdNo"]);
                this.ContractStatus = Convert.ToString(sqlRdr["ContractStatus"]);
                this.TotalPaid = Convert.ToDecimal(sqlRdr["TotalPaid"]);
                this.Bank = Convert.ToString(sqlRdr["Bank"]);
                this.DueAmount = CalculateDueAmount(this);
                return true;
            }
            return false;
        }

        private List<loanContractMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanContractMasterDAL> lstContractMaster = new List<loanContractMasterDAL>();
            loanContractMasterDAL objContractMaster = null;
            while (sqlRdr.Read())
            {
                objContractMaster = new loanContractMasterDAL();
                objContractMaster.ContractMasterId = Convert.ToInt32(sqlRdr["ContractMasterId"]);
                objContractMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objContractMaster.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
                objContractMaster.ContractTitle = Convert.ToString(sqlRdr["ContractTitle"]);
                objContractMaster.linktoBankMasterId = Convert.ToInt32(sqlRdr["linktoBankMasterId"]);
                objContractMaster.FileNo = Convert.ToInt32(sqlRdr["FileNo"]);
                objContractMaster.ContractDate = Convert.ToDateTime(sqlRdr["ContractDate"]);
                objContractMaster.ContractStartDate = Convert.ToDateTime(sqlRdr["ContractStartDate"]);
                objContractMaster.InstallmentDate = Convert.ToDateTime(sqlRdr["InstallmentDate"]);
                objContractMaster.linktoContractStatusMasterId = Convert.ToInt32(sqlRdr["linktoContractStatusMasterId"]);
                objContractMaster.ContractAmount = Convert.ToDecimal(sqlRdr["ContractAmount"]);
                objContractMaster.IsBasedOnMonth = Convert.ToBoolean(sqlRdr["IsBasedOnMonth"]);
                objContractMaster.NoOfInstallments = Convert.ToInt32(sqlRdr["NoOfInstallments"]);
                objContractMaster.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                objContractMaster.InterestRate = Convert.ToDecimal(sqlRdr["InterestRate"]);
                objContractMaster.DownPayment = Convert.ToDecimal(sqlRdr["DownPayment"]);
                objContractMaster.SettlementAmount = Convert.ToDecimal(sqlRdr["SettlementAmount"]);
                objContractMaster.SettlementReason = Convert.ToString(sqlRdr["SettlementReason"]);
                objContractMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                objContractMaster.Links = Convert.ToString(sqlRdr["Links"]);
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objContractMaster.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objContractMaster.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                if (sqlRdr["CreateDateTime"] != DBNull.Value)
                {
                    objContractMaster.CreateDateTime = Convert.ToDateTime(sqlRdr["CreateDateTime"]);
                }
                if (sqlRdr["linktoUserMasterIdCreatedBy"] != DBNull.Value)
                {
                    objContractMaster.linktoUserMasterIdCreatedBy = Convert.ToInt32(sqlRdr["linktoUserMasterIdCreatedBy"]);
                }
                objContractMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objContractMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objContractMaster.Customer = Convert.ToString(sqlRdr["Customer"]);
                objContractMaster.CustomerIdNo = Convert.ToString(sqlRdr["CustomerIdNo"]);
                objContractMaster.CustomerAddress = Convert.ToString(sqlRdr["CustomerAddress"]);
                objContractMaster.ContractStatus = Convert.ToString(sqlRdr["ContractStatus"]);
                if (sqlRdr["CustomerIsRedFlag"] != DBNull.Value)
                {
                    objContractMaster.CustomerIsRedFlag = Convert.ToBoolean(sqlRdr["CustomerIsRedFlag"]);
                }
                objContractMaster.TotalPaid = Convert.ToDecimal(sqlRdr["TotalPaid"]);
                objContractMaster.PendingAmount = Convert.ToDecimal(sqlRdr["PendingAmount"]);
                objContractMaster.Bank = Convert.ToString(sqlRdr["Bank"]);
                if (sqlRdr["LastPaidDate"] != DBNull.Value)
                {
                    objContractMaster.LastPaidDate = Convert.ToDateTime(sqlRdr["LastPaidDate"]);
                }
                if (sqlRdr["LastPaidAmount"] != DBNull.Value)
                {
                    objContractMaster.LastPaidAmount = Convert.ToDecimal(sqlRdr["LastPaidAmount"]);
                }
                objContractMaster.DueAmount = CalculateDueAmount(objContractMaster);
                objContractMaster.Last3Installments = Convert.ToString(sqlRdr["Last3Installments"]);
                objContractMaster.CustomerLinks = Convert.ToString(sqlRdr["CustomerLinks"]);
                objContractMaster.VerifiedBy = Convert.ToString(sqlRdr["VerifiedBy"]);
                objContractMaster.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);
                objContractMaster.CreatedBy = Convert.ToString(sqlRdr["CreatedBy"]);
                objContractMaster.GuarantorName = Convert.ToString(sqlRdr["GuarantorName"]);
                objContractMaster.BankAccountNumber = Convert.ToString(sqlRdr["BankAccountNumber"]);
                objContractMaster.BankAccountNumber2 = Convert.ToString(sqlRdr["BankAccountNumber2"]);
                objContractMaster.BankAccountNumber3 = Convert.ToString(sqlRdr["BankAccountNumber3"]);
                objContractMaster.BankAccountNumber4 = Convert.ToString(sqlRdr["BankAccountNumber4"]);

                lstContractMaster.Add(objContractMaster);
            }
            return lstContractMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertContractMaster(List<loanContractItemTranDAL> lstContractItemTranDAL)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                SqlCmd = new SqlCommand("loanContractMaster_Insert", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                SqlCmd.Parameters.Add("@ContractTitle", SqlDbType.NVarChar).Value = this.ContractTitle;
                SqlCmd.Parameters.Add("@FileNo", SqlDbType.Int).Value = this.FileNo;
                SqlCmd.Parameters.Add("@ContractDate", SqlDbType.Date).Value = this.ContractDate;
                SqlCmd.Parameters.Add("@ContractStartDate", SqlDbType.Date).Value = this.ContractStartDate;
                SqlCmd.Parameters.Add("@InstallmentDate", SqlDbType.Date).Value = this.InstallmentDate;
                if (this.linktoContractStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractStatusMasterId", SqlDbType.Int).Value = this.linktoContractStatusMasterId;
                }
                SqlCmd.Parameters.Add("@ContractAmount", SqlDbType.Money).Value = this.ContractAmount;
                SqlCmd.Parameters.Add("@IsBasedOnMonth", SqlDbType.Bit).Value = this.IsBasedOnMonth;
                SqlCmd.Parameters.Add("@NoOfInstallments", SqlDbType.Int).Value = this.NoOfInstallments;
                SqlCmd.Parameters.Add("@InstallmentAmount", SqlDbType.Money).Value = this.InstallmentAmount;
                SqlCmd.Parameters.Add("@InterestRate", SqlDbType.Money).Value = this.InterestRate;
                SqlCmd.Parameters.Add("@DownPayment", SqlDbType.Money).Value = this.DownPayment;
                SqlCmd.Parameters.Add("@SettlementAmount", SqlDbType.Money).Value = this.SettlementAmount;
                SqlCmd.Parameters.Add("@SettlementReason", SqlDbType.NVarChar).Value = this.SettlementReason;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@Links", SqlDbType.NVarChar).Value = this.Links;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@CreateDateTime", SqlDbType.DateTime).Value = this.CreateDateTime;
                SqlCmd.Parameters.Add("@linktoUserMasterIdCreatedBy", SqlDbType.Int).Value = this.linktoUserMasterIdCreatedBy;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                this.ContractMasterId = Convert.ToInt32(SqlCmd.Parameters["@ContractMasterId"].Value);
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                loanContractItemTranDAL objContractItemTranDAL = new loanContractItemTranDAL();
                objContractItemTranDAL.linktoContractMasterId = this.ContractMasterId;
                rs = objContractItemTranDAL.DeleteContractItemTran(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                rs = objContractItemTranDAL.InsertContractItemTran(lstContractItemTranDAL, SqlCon, SqlTran);
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
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
                loanObjectFactoryDAL.DisposeTransaction(SqlTran);
            }
        }
        #endregion

        #region Update
        public loanRecordStatus UpdateContractMaster(List<loanContractItemTranDAL> lstContractItemTranDAL)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                SqlCmd = new SqlCommand("loanContractMaster_Update", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Value = this.ContractMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                SqlCmd.Parameters.Add("@ContractTitle", SqlDbType.NVarChar).Value = this.ContractTitle;
                SqlCmd.Parameters.Add("@FileNo", SqlDbType.Int).Value = this.FileNo;
                SqlCmd.Parameters.Add("@ContractDate", SqlDbType.Date).Value = this.ContractDate;
                SqlCmd.Parameters.Add("@ContractStartDate", SqlDbType.Date).Value = this.ContractStartDate;
                SqlCmd.Parameters.Add("@InstallmentDate", SqlDbType.Date).Value = this.InstallmentDate;
                if (this.linktoContractStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractStatusMasterId", SqlDbType.Int).Value = this.linktoContractStatusMasterId;
                }
                SqlCmd.Parameters.Add("@ContractAmount", SqlDbType.Money).Value = this.ContractAmount;
                SqlCmd.Parameters.Add("@IsBasedOnMonth", SqlDbType.Bit).Value = this.IsBasedOnMonth;
                SqlCmd.Parameters.Add("@NoOfInstallments", SqlDbType.Int).Value = this.NoOfInstallments;
                SqlCmd.Parameters.Add("@InstallmentAmount", SqlDbType.Money).Value = this.InstallmentAmount;
                SqlCmd.Parameters.Add("@InterestRate", SqlDbType.Money).Value = this.InterestRate;
                SqlCmd.Parameters.Add("@DownPayment", SqlDbType.Money).Value = this.DownPayment;
                SqlCmd.Parameters.Add("@SettlementAmount", SqlDbType.Money).Value = this.SettlementAmount;
                SqlCmd.Parameters.Add("@SettlementReason", SqlDbType.NVarChar).Value = this.SettlementReason;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@Links", SqlDbType.NVarChar).Value = this.Links;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                loanContractItemTranDAL objContractItemTranDAL = new loanContractItemTranDAL();
                objContractItemTranDAL.linktoContractMasterId = this.ContractMasterId;
                rs = objContractItemTranDAL.DeleteContractItemTran(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                rs = objContractItemTranDAL.InsertContractItemTran(lstContractItemTranDAL, SqlCon, SqlTran);
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
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
                loanObjectFactoryDAL.DisposeTransaction(SqlTran);
            }
        }
        #endregion

        #region Delete
        public loanRecordStatus DeleteContractMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                loanContractItemTranDAL objContractItemTranDAL = new loanContractItemTranDAL();
                objContractItemTranDAL.linktoContractMasterId = this.ContractMasterId;
                loanRecordStatus rs = objContractItemTranDAL.DeleteContractItemTran(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                SqlCmd = new SqlCommand("loanContractMaster_Delete", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Value = this.ContractMasterId;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
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
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
                loanObjectFactoryDAL.DisposeTransaction(SqlTran);
            }
        }
        #endregion

        #region Select
        public bool SelectContractMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Value = this.ContractMasterId;

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

        public bool SelectContractMasterAmount()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractMasterAmount_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                while (SqlRdr.Read())
                {
                    this.ContractAmount = Convert.ToDecimal(SqlRdr["ContractAmount"]);
                    this.PendingAmount = Convert.ToDecimal(SqlRdr["PendingAmount"]);
                    this.IncomeAmount = Convert.ToDecimal(SqlRdr["IncomeAmount"]);
                    this.InstallmentAmount = Convert.ToDecimal(SqlRdr["InstallmentAmount"]);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return true;
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
        public List<loanContractMasterDAL> SelectAllContractMasterPageWise(int startRowIndex, int pageSize, out int totalRecords, DateTime? contractDateFrom = null, DateTime? contractDateTo = null, bool withDueAmount = false, int CustomerPaymentContractMasterId = 0, DateTime? installmentDateFrom = null, DateTime? installmentDateTo = null, string orderBy = null, string orderDir = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (CustomerPaymentContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@CustomerPaymentContractMasterId", SqlDbType.Int).Value = CustomerPaymentContractMasterId;
                }
                if (this.ContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Value = this.ContractMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }
                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;
                SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;
                SqlCmd.Parameters.Add("@ContractTitle", SqlDbType.NVarChar).Value = this.ContractTitle;
                if (this.FileNo > 0)
                {
                    SqlCmd.Parameters.Add("@FileNo", SqlDbType.Int).Value = this.FileNo;
                }
                if (contractDateFrom != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ContractDate", SqlDbType.Date).Value = contractDateFrom;
                }
                if (contractDateTo != null)
                {
                    SqlCmd.Parameters.Add("@ContractDateTo", SqlDbType.Date).Value = contractDateTo;
                }
                if (this.linktoContractStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractStatusMasterId", SqlDbType.Int).Value = this.linktoContractStatusMasterId;
                }
                if ((this.linktoCompanyMasterId > 0))
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.LastPaidDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@LastPaidDate", SqlDbType.Date).Value = this.LastPaidDate;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                if (withDueAmount)
                {
                    SqlCmd.Parameters.Add("@WithDueAmount", SqlDbType.Bit).Value = withDueAmount;
                }
                if (installmentDateFrom != new DateTime())
                {
                    SqlCmd.Parameters.Add("@InstallmentDateFrom", SqlDbType.Date).Value = installmentDateFrom;
                }
                if (installmentDateTo != new DateTime())
                {
                    SqlCmd.Parameters.Add("@InstallmentDateTo", SqlDbType.Date).Value = installmentDateTo;
                }
                if (this.DueDateFrom != null)
                {
                    SqlCmd.Parameters.Add("@DueDateFrom", SqlDbType.Date).Value = this.DueDateFrom;
                }
                if (this.DueDateTo != null)
                {
                    SqlCmd.Parameters.Add("@DueDateTo", SqlDbType.Date).Value = this.DueDateTo;
                }
                if (this.DueInstallments != null)
                {
                    SqlCmd.Parameters.Add("@DueInstallments", SqlDbType.Int).Value = this.DueInstallments;
                }
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;
                SqlCmd.Parameters.Add("@HasURL", SqlDbType.Bit).Value = this.HasURL;
                if (orderBy != null)
                {
                    SqlCmd.Parameters.Add("@OrderBy", SqlDbType.VarChar).Value = orderBy;
                }
                if (orderDir != null)
                {
                    SqlCmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = orderDir;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanContractMasterDAL> lstContractMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstContractMasterDAL;
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

        #region SelectAllFor Followup
        public List<loanContractMasterDAL> SelectAllContractMasterByFollowupPageWise(int startRowIndex, int pageSize, out int totalRecords, DateTime? contractDateFrom = null, DateTime? contractDateTo = null, bool withDueAmount = false, int CustomerPaymentContractMasterId = 0, DateTime? installmentDateFrom = null, DateTime? installmentDateTo = null, string orderBy = null, string orderDir = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowupPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (CustomerPaymentContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@CustomerPaymentContractMasterId", SqlDbType.Int).Value = CustomerPaymentContractMasterId;
                }
                if (this.ContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Value = this.ContractMasterId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }

                if (this.linktoUserMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                }

                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;
                SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;
                SqlCmd.Parameters.Add("@ContractTitle", SqlDbType.NVarChar).Value = this.ContractTitle;
                if (this.FileNo > 0)
                {
                    SqlCmd.Parameters.Add("@FileNo", SqlDbType.Int).Value = this.FileNo;
                }
                if (contractDateFrom != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ContractDate", SqlDbType.Date).Value = contractDateFrom;
                }
                if (contractDateTo != null)
                {
                    SqlCmd.Parameters.Add("@ContractDateTo", SqlDbType.Date).Value = contractDateTo;
                }
                if (this.linktoContractStatusMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractStatusMasterId", SqlDbType.Int).Value = this.linktoContractStatusMasterId;
                }
                if ((this.linktoCompanyMasterId > 0))
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                if (this.LastPaidDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@LastPaidDate", SqlDbType.Date).Value = this.LastPaidDate;
                }
                if (this.linktoBankMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBankMasterId", SqlDbType.Int).Value = this.linktoBankMasterId;
                }
                if (withDueAmount)
                {
                    SqlCmd.Parameters.Add("@WithDueAmount", SqlDbType.Bit).Value = withDueAmount;
                }
                if (installmentDateFrom != new DateTime())
                {
                    SqlCmd.Parameters.Add("@InstallmentDateFrom", SqlDbType.Date).Value = installmentDateFrom;
                }
                if (installmentDateTo != new DateTime())
                {
                    SqlCmd.Parameters.Add("@InstallmentDateTo", SqlDbType.Date).Value = installmentDateTo;
                }
                if (this.DueDateFrom != null)
                {
                    SqlCmd.Parameters.Add("@DueDateFrom", SqlDbType.Date).Value = this.DueDateFrom;
                }
                if (this.DueDateTo != null)
                {
                    SqlCmd.Parameters.Add("@DueDateTo", SqlDbType.Date).Value = this.DueDateTo;
                }
                if (this.DueInstallments != null)
                {
                    SqlCmd.Parameters.Add("@DueInstallments", SqlDbType.Int).Value = this.DueInstallments;
                }
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;
                SqlCmd.Parameters.Add("@HasURL", SqlDbType.Bit).Value = this.HasURL;
                if (orderBy != null)
                {
                    SqlCmd.Parameters.Add("@OrderBy", SqlDbType.VarChar).Value = orderBy;
                }
                if (orderDir != null)
                {
                    SqlCmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = orderDir;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanContractMasterDAL> lstContractMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstContractMasterDAL;
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


        #region Private Methods
        private decimal CalculateDueAmount(loanContractMasterDAL objContractMasterDAL)
        {
            DateTime CurrentDate;
            if (Thread.CurrentThread.CurrentCulture.ToString().ToLower().Contains("ar-sa"))
            {
                loanGlobalsDAL.SwitchCulture("en-us");
                CurrentDate = loanGlobalsDAL.GetCurrentDateTime();
                loanGlobalsDAL.SwitchCulture("ar-sa");
            }
            else
            {
                CurrentDate = loanGlobalsDAL.GetCurrentDateTime();
            }
            int TotalInstallments;
            TotalInstallments = Convert.ToInt32(Math.Floor(CurrentDate.Subtract(objContractMasterDAL.ContractStartDate.AddMonths(-1)).Days / (365.25 / 12)));
            if (TotalInstallments > objContractMasterDAL.NoOfInstallments)
            {
                TotalInstallments = objContractMasterDAL.NoOfInstallments;
            }
            decimal AmountToPay = objContractMasterDAL.InstallmentAmount * TotalInstallments;
            if (CurrentDate > objContractMasterDAL.ContractStartDate)
            {
                decimal DueAmount = AmountToPay - objContractMasterDAL.TotalPaid;
                decimal RemainingAmount = objContractMasterDAL.ContractAmount - objContractMasterDAL.TotalPaid;
                if (DueAmount > RemainingAmount)
                {
                    return RemainingAmount;
                }
                else
                {
                    return DueAmount;
                }
            }
            else
            {
                return 0;
            }
        }
        #endregion
    }
}
