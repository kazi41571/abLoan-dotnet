using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanSalesItemTran
    /// </summary>
    [Serializable]
    public class loanSalesItemTranDAL
    {
        #region Properties
        public int SalesItemTranId { get; set; }
        public int linktoSalesMasterId { get; set; }
        public int linktoItemMasterId { get; set; }
        public int Quantity { get; set; }
        public decimal SalesRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Vat { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Fees { get; set; }
        public decimal NetAmount { get; set; }

        /// Extra
        public string Item { get; set; }
        public string ItemCode { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.SalesItemTranId = Convert.ToInt32(sqlRdr["SalesItemTranId"]);
                this.linktoSalesMasterId = Convert.ToInt32(sqlRdr["linktoSalesMasterId"]);
                this.linktoItemMasterId = Convert.ToInt32(sqlRdr["linktoItemMasterId"]);
                this.Quantity = Convert.ToInt32(sqlRdr["Quantity"]);
                this.SalesRate = Convert.ToDecimal(sqlRdr["SalesRate"]);
                this.DiscountAmount = Convert.ToDecimal(sqlRdr["DiscountAmount"]);
                this.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                this.VatAmount = Convert.ToDecimal(sqlRdr["VatAmount"]);
                this.Fees = Convert.ToDecimal(sqlRdr["Fees"]);
                this.NetAmount = Convert.ToDecimal(sqlRdr["NetAmount"]);

                /// Extra
                this.Item = Convert.ToString(sqlRdr["Item"]);
                return true;
            }
            return false;
        }

        private List<loanSalesItemTranDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanSalesItemTranDAL> lstSalesItemTran = new List<loanSalesItemTranDAL>();
            loanSalesItemTranDAL objSalesItemTran = null;
            while (sqlRdr.Read())
            {
                objSalesItemTran = new loanSalesItemTranDAL();
                objSalesItemTran.SalesItemTranId = Convert.ToInt32(sqlRdr["SalesItemTranId"]);
                objSalesItemTran.linktoSalesMasterId = Convert.ToInt32(sqlRdr["linktoSalesMasterId"]);
                objSalesItemTran.linktoItemMasterId = Convert.ToInt32(sqlRdr["linktoItemMasterId"]);
                objSalesItemTran.Quantity = Convert.ToInt32(sqlRdr["Quantity"]);
                objSalesItemTran.SalesRate = Convert.ToDecimal(sqlRdr["SalesRate"]);
                objSalesItemTran.DiscountAmount = Convert.ToDecimal(sqlRdr["DiscountAmount"]);
                objSalesItemTran.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                objSalesItemTran.VatAmount = Convert.ToDecimal(sqlRdr["VatAmount"]);
                objSalesItemTran.Fees = Convert.ToDecimal(sqlRdr["Fees"]);
                objSalesItemTran.NetAmount = Convert.ToDecimal(sqlRdr["NetAmount"]);

                /// Extra
                objSalesItemTran.Item = Convert.ToString(sqlRdr["Item"]);
                objSalesItemTran.ItemCode = Convert.ToString(sqlRdr["ItemCode"]);
                lstSalesItemTran.Add(objSalesItemTran);
            }
            return lstSalesItemTran;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertSalesItemTran(List<loanSalesItemTranDAL> lstSalesItemTranDAL, SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlCommand SqlCmd = null;
            try
            {
                SqlCmd = new SqlCommand("loanSalesItemTran_Insert", sqlCon, sqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                loanRecordStatus rs = loanRecordStatus.Success;
                foreach (loanSalesItemTranDAL obj in lstSalesItemTranDAL)
                {
                    SqlCmd.Parameters.Clear();
                    SqlCmd.Parameters.Add("@SalesItemTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    if (this.linktoSalesMasterId > 0)
                    {
                        SqlCmd.Parameters.Add("@linktoSalesMasterId", SqlDbType.Int).Value = this.linktoSalesMasterId;
                    }
                    if (obj.linktoItemMasterId > 0)
                    {
                        SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = obj.linktoItemMasterId;
                    }
                    SqlCmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = obj.Quantity;
                    SqlCmd.Parameters.Add("@SalesRate", SqlDbType.Money).Value = obj.SalesRate;
                    SqlCmd.Parameters.Add("@DiscountAmount", SqlDbType.Money).Value = obj.DiscountAmount;
                    SqlCmd.Parameters.Add("@Vat", SqlDbType.Money).Value = obj.Vat;
                    SqlCmd.Parameters.Add("@VatAmount", SqlDbType.Money).Value = obj.VatAmount;
                    SqlCmd.Parameters.Add("@Fees", SqlDbType.Money).Value = obj.Fees;
                    SqlCmd.Parameters.Add("@NetAmount", SqlDbType.Money).Value = obj.NetAmount;
                    SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                    SqlCmd.ExecuteNonQuery();

                    this.SalesItemTranId = Convert.ToInt32(SqlCmd.Parameters["@SalesItemTranId"].Value);
                    rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                    if (rs != loanRecordStatus.Success)
                    {
                        return rs;
                    }
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
            }
        }
        #endregion


        #region Delete
        public loanRecordStatus DeleteSalesItemTran(SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlCommand SqlCmd = null;
            try
            {
                SqlCmd = new SqlCommand("loanSalesItemTran_Delete", sqlCon, sqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoSalesMasterId", SqlDbType.Int).Value = this.linktoSalesMasterId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCmd.ExecuteNonQuery();

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
            }
        }
        #endregion

        #region Select
        public bool SelectSalesItemTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanSalesItemTran_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@SalesItemTranId", SqlDbType.Int).Value = this.SalesItemTranId;

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

        public List<loanSalesItemTranDAL> SelectAllSalesItemTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanSalesItemTran_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoSalesMasterId", SqlDbType.Int).Value = this.linktoSalesMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanSalesItemTranDAL> lstSalesItemTranDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstSalesItemTranDAL;
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
