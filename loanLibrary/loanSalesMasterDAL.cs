using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanSalesMaster
    /// </summary>
    public class loanSalesMasterDAL
    {
        #region Properties
        public int SalesMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerIdNo { get; set; }
        public string CustomerAddress { get; set; }
        public DateTime SalesDate { get; set; }
        public string Notes { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public decimal? ContractAmount { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public string BillNo { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TranType { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int linktoCategoryMasterId { get; set; }

        public List<loanSalesItemTranDAL> lstSalesItemTranDAL { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.SalesMasterId = Convert.ToInt32(sqlRdr["SalesMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                this.CustomerIdNo = Convert.ToString(sqlRdr["CustomerIdNo"]);
                this.CustomerAddress = Convert.ToString(sqlRdr["CustomerAddress"]);
                this.SalesDate = Convert.ToDateTime(sqlRdr["SalesDate"]);
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                if (sqlRdr["ContractStartDate"] != DBNull.Value)
                {
                    this.ContractStartDate = Convert.ToDateTime(sqlRdr["ContractStartDate"]);
                }
                if (sqlRdr["ContractAmount"] != DBNull.Value)
                {
                    this.ContractAmount = Convert.ToDecimal(sqlRdr["ContractAmount"]);
                }
                if (sqlRdr["InstallmentAmount"] != DBNull.Value)
                {
                    this.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                }
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                this.BillNo = Convert.ToString(sqlRdr["BillNo"]);
                this.ReceiptNo = Convert.ToString(sqlRdr["ReceiptNo"]);

                /// Extra
                this.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);
                return true;
            }
            return false;
        }

        private List<loanSalesMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanSalesMasterDAL> lstSalesMaster = new List<loanSalesMasterDAL>();
            loanSalesMasterDAL objSalesMaster = null;
            while (sqlRdr.Read())
            {
                objSalesMaster = new loanSalesMasterDAL();
                objSalesMaster.SalesMasterId = Convert.ToInt32(sqlRdr["SalesMasterId"]);
                objSalesMaster.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                objSalesMaster.CustomerIdNo = Convert.ToString(sqlRdr["CustomerIdNo"]);
                objSalesMaster.CustomerAddress = Convert.ToString(sqlRdr["CustomerAddress"]);
                objSalesMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objSalesMaster.SalesDate = Convert.ToDateTime(sqlRdr["SalesDate"]);
                objSalesMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                objSalesMaster.BillNo = Convert.ToString(sqlRdr["BillNo"]);
                objSalesMaster.ReceiptNo = Convert.ToString(sqlRdr["ReceiptNo"]);
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objSalesMaster.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objSalesMaster.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                if (sqlRdr["ContractStartDate"] != DBNull.Value)
                {
                    objSalesMaster.ContractStartDate = Convert.ToDateTime(sqlRdr["ContractStartDate"]);
                }
                if (sqlRdr["ContractAmount"] != DBNull.Value)
                {
                    objSalesMaster.ContractAmount = Convert.ToDecimal(sqlRdr["ContractAmount"]);
                }
                if (sqlRdr["InstallmentAmount"] != DBNull.Value)
                {
                    objSalesMaster.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                }
                objSalesMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objSalesMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objSalesMaster.VerifiedBy = Convert.ToString(sqlRdr["VerifiedBy"]);
                objSalesMaster.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);

                lstSalesMaster.Add(objSalesMaster);
            }
            return lstSalesMaster;
        }

        private List<loanSalesMasterDAL> SetListPropertiesFromSqlDataReaderReport(SqlDataReader sqlRdr)
        {
            List<loanSalesMasterDAL> lstSalesMaster = new List<loanSalesMasterDAL>();
            loanSalesMasterDAL objSalesMaster = null;
            while (sqlRdr.Read())
            {
                objSalesMaster = new loanSalesMasterDAL();
                objSalesMaster.SalesMasterId = Convert.ToInt32(sqlRdr["SalesMasterId"]);
                objSalesMaster.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                objSalesMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objSalesMaster.SalesDate = Convert.ToDateTime(sqlRdr["SalesDate"]);
                objSalesMaster.TranType = Convert.ToString(sqlRdr["TranType"]);
                if (sqlRdr["ContractStartDate"] != DBNull.Value)
                {
                    objSalesMaster.ContractStartDate = Convert.ToDateTime(sqlRdr["ContractStartDate"]);
                }
                if (sqlRdr["ContractAmount"] != DBNull.Value)
                {
                    objSalesMaster.ContractAmount = Convert.ToDecimal(sqlRdr["ContractAmount"]);
                }
                if (sqlRdr["InstallmentAmount"] != DBNull.Value)
                {
                    objSalesMaster.InstallmentAmount = Convert.ToDecimal(sqlRdr["InstallmentAmount"]);
                }
                /// Extra
                lstSalesMaster.Add(objSalesMaster);
            }
            return lstSalesMaster;
        }

        private List<loanSalesMasterDAL> SetListPropertiesFromSqlDataReaderPurchaseSalesReport(SqlDataReader sqlRdr)
        {
            List<loanSalesMasterDAL> lstSalesMaster = new List<loanSalesMasterDAL>();
            loanSalesMasterDAL objSalesMaster = null;
            while (sqlRdr.Read())
            {
                objSalesMaster = new loanSalesMasterDAL();
                objSalesMaster.SalesMasterId = Convert.ToInt32(sqlRdr["SalesMasterId"]);
                objSalesMaster.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                objSalesMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objSalesMaster.SalesDate = Convert.ToDateTime(sqlRdr["SalesDate"]);
                objSalesMaster.TranType = Convert.ToString(sqlRdr["TranType"]);

                /// Extra
                lstSalesMaster.Add(objSalesMaster);
            }
            return lstSalesMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertSalesMaster(List<loanSalesItemTranDAL> lstSalesItemTranDAL)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                SqlCmd = new SqlCommand("loanSalesMaster_Insert", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@SalesMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = this.CustomerName;
                SqlCmd.Parameters.Add("@CustomerIdNo", SqlDbType.NVarChar).Value = this.CustomerIdNo;
                SqlCmd.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar).Value = this.CustomerAddress;
                SqlCmd.Parameters.Add("@SalesDate", SqlDbType.Date).Value = this.SalesDate;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@ContractStartDate", SqlDbType.Date).Value = this.ContractStartDate;
                SqlCmd.Parameters.Add("@ContractAmount", SqlDbType.Money).Value = this.ContractAmount;
                SqlCmd.Parameters.Add("@InstallmentAmount", SqlDbType.Money).Value = this.InstallmentAmount;
                SqlCmd.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = this.BillNo;
                SqlCmd.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar).Value = this.ReceiptNo;

                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                this.SalesMasterId = Convert.ToInt32(SqlCmd.Parameters["@SalesMasterId"].Value);
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                loanSalesItemTranDAL objContractItemTranDAL = new loanSalesItemTranDAL();
                objContractItemTranDAL.linktoSalesMasterId = this.SalesMasterId;
                rs = objContractItemTranDAL.InsertSalesItemTran(lstSalesItemTranDAL, SqlCon, SqlTran);
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
        public loanRecordStatus UpdateSalesMaster(List<loanSalesItemTranDAL> lstSalesItemTranDAL)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                SqlCmd = new SqlCommand("loanSalesMaster_Update", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@SalesMasterId", SqlDbType.Int).Value = this.SalesMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = this.CustomerName;
                SqlCmd.Parameters.Add("@CustomerIdNo", SqlDbType.NVarChar).Value = this.CustomerIdNo;
                SqlCmd.Parameters.Add("@CustomerAddress", SqlDbType.NVarChar).Value = this.CustomerAddress;
                SqlCmd.Parameters.Add("@SalesDate", SqlDbType.Date).Value = this.SalesDate;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@ContractStartDate", SqlDbType.Date).Value = this.ContractStartDate;
                SqlCmd.Parameters.Add("@ContractAmount", SqlDbType.Money).Value = this.ContractAmount;
                SqlCmd.Parameters.Add("@InstallmentAmount", SqlDbType.Money).Value = this.InstallmentAmount;
                SqlCmd.Parameters.Add("@BillNo", SqlDbType.NVarChar).Value = this.BillNo;
                SqlCmd.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar).Value = this.ReceiptNo;

                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

                this.SalesMasterId = Convert.ToInt32(SqlCmd.Parameters["@SalesMasterId"].Value);
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                loanSalesItemTranDAL objContractItemTranDAL = new loanSalesItemTranDAL();
                objContractItemTranDAL.linktoSalesMasterId = this.SalesMasterId;
                rs = objContractItemTranDAL.DeleteSalesItemTran(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                rs = objContractItemTranDAL.InsertSalesItemTran(lstSalesItemTranDAL, SqlCon, SqlTran);
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
        public loanRecordStatus DeleteSalesMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                loanSalesItemTranDAL objContractItemTranDAL = new loanSalesItemTranDAL();
                objContractItemTranDAL.linktoSalesMasterId = this.SalesMasterId;
                loanRecordStatus rs = objContractItemTranDAL.DeleteSalesItemTran(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                SqlCmd = new SqlCommand("loanSalesMaster_Delete", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@SalesMasterId", SqlDbType.Int).Value = this.SalesMasterId;
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
        public bool SelectSalesMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanSalesMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@SalesMasterId", SqlDbType.Int).Value = this.SalesMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                bool IsSelected = SetClassPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                loanSalesItemTranDAL objSalesItemTranDAL = new loanSalesItemTranDAL();
                objSalesItemTranDAL.linktoSalesMasterId = this.SalesMasterId;
                this.lstSalesItemTranDAL = objSalesItemTranDAL.SelectAllSalesItemTran();

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

        public bool SelectMaxBillNo()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanSalesMasterMaxBillNo_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@MaxBillNo", SqlDbType.VarChar, 9).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();

                this.BillNo = Convert.ToString(SqlCmd.Parameters["@MaxBillNo"].Value);

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

        public List<loanSalesMasterDAL> SelectAllSalesMasterPageWise(int startRowIndex, int pageSize, out int totalRecords, int itemMasterId)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanSalesMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.FromDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                }
                if (this.ToDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                }
                if (itemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = itemMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;
                if (!string.IsNullOrWhiteSpace(this.BillNo))
                {
                    SqlCmd.Parameters.Add("@BillNo", SqlDbType.NVarChar, 20).Value = this.BillNo;
                }
                if (!string.IsNullOrWhiteSpace(this.ReceiptNo))
                {
                    SqlCmd.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar, 20).Value = this.ReceiptNo;
                }
                if (!string.IsNullOrWhiteSpace(this.CustomerName))
                {
                    SqlCmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar, 20).Value = this.CustomerName;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanSalesMasterDAL> lstSalesMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                loanSalesItemTranDAL objSalesItemTranDAL;
                foreach (loanSalesMasterDAL obj in lstSalesMasterDAL)
                {
                    objSalesItemTranDAL = new loanSalesItemTranDAL();
                    objSalesItemTranDAL.linktoSalesMasterId = obj.SalesMasterId;
                    obj.lstSalesItemTranDAL = objSalesItemTranDAL.SelectAllSalesItemTran();
                }

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstSalesMasterDAL;
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

        public List<loanSalesMasterDAL> SelectAllPurchaseSalesTransactionReportPageWise(int startRowIndex, int pageSize, out int totalRecords, out int openingBalance, int ItemMasterId)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseSalesTransactionReportPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.CustomerName;
                if (this.FromDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                }
                if (this.ToDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                }
                if (ItemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = ItemMasterId;
                }
                if (this.linktoCategoryMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCategoryMasterId", SqlDbType.Int).Value = this.linktoCategoryMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@OpeningBalance", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanSalesMasterDAL> lstSalesMasterDAL = SetListPropertiesFromSqlDataReaderPurchaseSalesReport(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                loanSalesItemTranDAL objSalesItemTranDAL;
                foreach (loanSalesMasterDAL obj in lstSalesMasterDAL)
                {
                    objSalesItemTranDAL = new loanSalesItemTranDAL();
                    objSalesItemTranDAL.linktoSalesMasterId = obj.SalesMasterId;
                    obj.lstSalesItemTranDAL = objSalesItemTranDAL.SelectAllSalesItemTran();
                }

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                openingBalance = (int)SqlCmd.Parameters["@OpeningBalance"].Value;
                return lstSalesMasterDAL;
            }
            catch (Exception ex)
            {
                totalRecords = 0;
                openingBalance = 0;
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

        public List<loanSalesMasterDAL> SelectAllSalesReportPageWise(int startRowIndex, int pageSize, out int totalRecords, int itemMasterId)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanSalesReportPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.CustomerName;
                if (this.FromDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                }
                if (this.ToDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                }
                if (itemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = itemMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanSalesMasterDAL> lstSalesMasterDAL = SetListPropertiesFromSqlDataReaderReport(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                loanSalesItemTranDAL objSalesItemTranDAL;
                foreach (loanSalesMasterDAL obj in lstSalesMasterDAL)
                {
                    objSalesItemTranDAL = new loanSalesItemTranDAL();
                    objSalesItemTranDAL.linktoSalesMasterId = obj.SalesMasterId;
                    obj.lstSalesItemTranDAL = objSalesItemTranDAL.SelectAllSalesItemTran();
                }

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstSalesMasterDAL;
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
