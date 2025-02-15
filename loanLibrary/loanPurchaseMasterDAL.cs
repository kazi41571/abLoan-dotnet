using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanPurchaseMaster
    /// </summary>
    public class loanPurchaseMasterDAL
    {
        #region Properties
        public int PurchaseMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int linktoItemMasterId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchaseRate { get; set; }
        public decimal Vat { get; set; }
        public decimal NetAmount { get; set; }
        public string Notes { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string Item { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int TranId { get; set; }
        public DateTime TranDate { get; set; }
        public decimal Rate { get; set; }
        public string TranType { get; set; }
        public int TotalQuantity { get; set; }
        public string CustomerName { get; set; }
        public decimal Fees { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }

        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.PurchaseMasterId = Convert.ToInt32(sqlRdr["PurchaseMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.PurchaseDate = Convert.ToDateTime(sqlRdr["PurchaseDate"]);
                this.linktoItemMasterId = Convert.ToInt32(sqlRdr["linktoItemMasterId"]);
                this.Quantity = Convert.ToInt32(sqlRdr["Quantity"]);
                this.PurchaseRate = Convert.ToDecimal(sqlRdr["PurchaseRate"]);
                this.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                this.NetAmount = Convert.ToDecimal(sqlRdr["NetAmount"]);
                this.Notes = Convert.ToString(sqlRdr["Notes"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.Item = Convert.ToString(sqlRdr["Item"]);
                return true;
            }
            return false;
        }

        private List<loanPurchaseMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanPurchaseMasterDAL> lstPurchaseMaster = new List<loanPurchaseMasterDAL>();
            loanPurchaseMasterDAL objPurchaseMaster = null;
            while (sqlRdr.Read())
            {
                objPurchaseMaster = new loanPurchaseMasterDAL();
                objPurchaseMaster.PurchaseMasterId = Convert.ToInt32(sqlRdr["PurchaseMasterId"]);
                objPurchaseMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objPurchaseMaster.PurchaseDate = Convert.ToDateTime(sqlRdr["PurchaseDate"]);
                objPurchaseMaster.linktoItemMasterId = Convert.ToInt32(sqlRdr["linktoItemMasterId"]);
                objPurchaseMaster.Quantity = Convert.ToInt32(sqlRdr["Quantity"]);
                objPurchaseMaster.PurchaseRate = Convert.ToDecimal(sqlRdr["PurchaseRate"]);
                objPurchaseMaster.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                objPurchaseMaster.NetAmount = Convert.ToDecimal(sqlRdr["NetAmount"]);
                objPurchaseMaster.Notes = Convert.ToString(sqlRdr["Notes"]);
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objPurchaseMaster.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objPurchaseMaster.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                objPurchaseMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objPurchaseMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objPurchaseMaster.Item = Convert.ToString(sqlRdr["Item"]);
                objPurchaseMaster.VerifiedBy = Convert.ToString(sqlRdr["VerifiedBy"]);
                objPurchaseMaster.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);

                lstPurchaseMaster.Add(objPurchaseMaster);
            }
            return lstPurchaseMaster;
        }

        private List<loanPurchaseMasterDAL> SetListPropertiesFromSqlDataReaderReport(SqlDataReader sqlRdr)
        {
            List<loanPurchaseMasterDAL> lstSalesMaster = new List<loanPurchaseMasterDAL>();
            loanPurchaseMasterDAL objPurchaseMaster = null;
            while (sqlRdr.Read())
            {
                objPurchaseMaster = new loanPurchaseMasterDAL();
                objPurchaseMaster.TranId = Convert.ToInt32(sqlRdr["TranId"]);
                objPurchaseMaster.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                objPurchaseMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objPurchaseMaster.TranDate = Convert.ToDateTime(sqlRdr["TranDate"]);
                objPurchaseMaster.linktoItemMasterId = Convert.ToInt32(sqlRdr["linktoItemMasterId"]);
                objPurchaseMaster.Quantity = Convert.ToInt32(sqlRdr["Quantity"]);
                objPurchaseMaster.Rate = Convert.ToDecimal(sqlRdr["Rate"]);
                objPurchaseMaster.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                objPurchaseMaster.Fees = Convert.ToDecimal(sqlRdr["Fees"]);
                objPurchaseMaster.NetAmount = Convert.ToDecimal(sqlRdr["NetAmount"]);
                objPurchaseMaster.TranType = Convert.ToString(sqlRdr["TranType"]);

                /// Extra
                objPurchaseMaster.Item = Convert.ToString(sqlRdr["Item"]);
                objPurchaseMaster.TotalQuantity = Convert.ToInt32(sqlRdr["TotalQuantity"]);
                lstSalesMaster.Add(objPurchaseMaster);
            }
            return lstSalesMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertPurchaseMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@PurchaseMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@PurchaseDate", SqlDbType.Date).Value = this.PurchaseDate;
                if (this.linktoItemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = this.linktoItemMasterId;
                }
                SqlCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = this.Quantity;
                SqlCmd.Parameters.Add("@PurchaseRate", SqlDbType.Money).Value = this.PurchaseRate;
                SqlCmd.Parameters.Add("@Vat", SqlDbType.Money).Value = this.Vat;
                SqlCmd.Parameters.Add("@NetAmount", SqlDbType.Money).Value = this.NetAmount;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.PurchaseMasterId = Convert.ToInt32(SqlCmd.Parameters["@PurchaseMasterId"].Value);
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
        public loanRecordStatus UpdatePurchaseMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@PurchaseMasterId", SqlDbType.Int).Value = this.PurchaseMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@PurchaseDate", SqlDbType.Date).Value = this.PurchaseDate;
                if (this.linktoItemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = this.linktoItemMasterId;
                }
                SqlCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = this.Quantity;
                SqlCmd.Parameters.Add("@PurchaseRate", SqlDbType.Money).Value = this.PurchaseRate;
                SqlCmd.Parameters.Add("@Vat", SqlDbType.Money).Value = this.Vat;
                SqlCmd.Parameters.Add("@NetAmount", SqlDbType.Money).Value = this.NetAmount;
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
        public loanRecordStatus DeletePurchaseMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@PurchaseMasterId", SqlDbType.Int).Value = this.PurchaseMasterId;
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

        #region Select
        public bool SelectPurchaseMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@PurchaseMasterId", SqlDbType.Int).Value = this.PurchaseMasterId;

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

        public List<loanPurchaseMasterDAL> SelectAllPurchaseMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.FromDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                }
                if (this.ToDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                }
                if (this.linktoItemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = this.linktoItemMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanPurchaseMasterDAL> lstPurchaseMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstPurchaseMasterDAL;
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

        public List<loanPurchaseMasterDAL> SelectAllPurchaseReportPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanPurchaseReportPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.FromDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                }
                if (this.ToDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                }
                if (this.linktoItemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = this.linktoItemMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanPurchaseMasterDAL> lstPurchaseMasterDAL = SetListPropertiesFromSqlDataReaderReport(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstPurchaseMasterDAL;
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
