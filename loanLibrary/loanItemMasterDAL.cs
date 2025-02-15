using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanItemMaster
    /// </summary>
    public class loanItemMasterDAL
    {
        #region Properties
        public int ItemMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public int? linktoCategoryMasterId { get; set; }
        public int? linktoBrandMasterId { get; set; }
        public int? linktoColorMasterId { get; set; }
        public decimal Price { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal Vat { get; set; }
        public int CurrentQuantity { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string Category { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }

        public int OpeningQuantity { get; set; }
        public int PurchaseQuantity { get; set; }
        public int SalesQuantity { get; set; }
        /// 
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.ItemMasterId = Convert.ToInt32(sqlRdr["ItemMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.ItemName = Convert.ToString(sqlRdr["ItemName"]);
                this.ItemCode = Convert.ToString(sqlRdr["ItemCode"]);
                this.ItemDescription = Convert.ToString(sqlRdr["ItemDescription"]);
                if (sqlRdr["linktoCategoryMasterId"] != DBNull.Value)
                {
                    this.linktoCategoryMasterId = Convert.ToInt32(sqlRdr["linktoCategoryMasterId"]);
                }
                if (sqlRdr["linktoBrandMasterId"] != DBNull.Value)
                {
                    this.linktoBrandMasterId = Convert.ToInt32(sqlRdr["linktoBrandMasterId"]);
                }
                if (sqlRdr["linktoColorMasterId"] != DBNull.Value)
                {
                    this.linktoColorMasterId = Convert.ToInt32(sqlRdr["linktoColorMasterId"]);
                }
                this.CurrentQuantity = Convert.ToInt32(sqlRdr["CurrentQuantity"]);
                this.Price = Convert.ToDecimal(sqlRdr["Price"]);
                this.SalesPrice = Convert.ToDecimal(sqlRdr["SalesPrice"]);
                this.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                this.IsEnabled = Convert.ToBoolean(sqlRdr["IsEnabled"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                return true;
            }
            return false;
        }

        private List<loanItemMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanItemMasterDAL> lstItemMaster = new List<loanItemMasterDAL>();
            loanItemMasterDAL objItemMaster = null;
            while (sqlRdr.Read())
            {
                objItemMaster = new loanItemMasterDAL();
                objItemMaster.ItemMasterId = Convert.ToInt32(sqlRdr["ItemMasterId"]);
                objItemMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objItemMaster.ItemName = Convert.ToString(sqlRdr["ItemName"]);
                objItemMaster.ItemCode = Convert.ToString(sqlRdr["ItemCode"]);
                objItemMaster.ItemDescription = Convert.ToString(sqlRdr["ItemDescription"]);
                if (sqlRdr["linktoCategoryMasterId"] != DBNull.Value)
                {
                    objItemMaster.linktoCategoryMasterId = Convert.ToInt32(sqlRdr["linktoCategoryMasterId"]);
                }
                if (sqlRdr["linktoBrandMasterId"] != DBNull.Value)
                {
                    objItemMaster.linktoBrandMasterId = Convert.ToInt32(sqlRdr["linktoBrandMasterId"]);
                }
                if (sqlRdr["linktoColorMasterId"] != DBNull.Value)
                {
                    objItemMaster.linktoColorMasterId = Convert.ToInt32(sqlRdr["linktoColorMasterId"]);
                }
                objItemMaster.CurrentQuantity = Convert.ToInt32(sqlRdr["CurrentQuantity"]);
                objItemMaster.Price = Convert.ToDecimal(sqlRdr["Price"]);
                objItemMaster.SalesPrice = Convert.ToDecimal(sqlRdr["SalesPrice"]);
                objItemMaster.Vat = Convert.ToDecimal(sqlRdr["Vat"]);
                objItemMaster.IsEnabled = Convert.ToBoolean(sqlRdr["IsEnabled"]);
                objItemMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objItemMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objItemMaster.Category = Convert.ToString(sqlRdr["Category"]);
                objItemMaster.Brand = Convert.ToString(sqlRdr["Brand"]);
                objItemMaster.Color = Convert.ToString(sqlRdr["Color"]);
                /// 
                lstItemMaster.Add(objItemMaster);
            }
            return lstItemMaster;
        }

        private List<loanItemMasterDAL> SetListPropertiesFromSqlDataReaderItemStockReport(SqlDataReader sqlRdr)
        {
            List<loanItemMasterDAL> lstItemMaster = new List<loanItemMasterDAL>();
            loanItemMasterDAL objItemMaster = null;
            while (sqlRdr.Read())
            {
                objItemMaster = new loanItemMasterDAL();
                objItemMaster.ItemMasterId = Convert.ToInt32(sqlRdr["ItemMasterId"]);
                objItemMaster.ItemName = Convert.ToString(sqlRdr["ItemName"]);
                objItemMaster.OpeningQuantity = Convert.ToInt32(sqlRdr["OpeningQuantity"]);
                objItemMaster.PurchaseQuantity = Convert.ToInt32(sqlRdr["PurchaseQuantity"]);
                objItemMaster.SalesQuantity = Convert.ToInt32(sqlRdr["SalesQuantity"]);
                objItemMaster.CurrentQuantity = Convert.ToInt32(sqlRdr["CurrentQuantity"]);
                lstItemMaster.Add(objItemMaster);
            }
            return lstItemMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertItemMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ItemMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = this.ItemName;
                SqlCmd.Parameters.Add("@ItemCode", SqlDbType.NVarChar).Value = this.ItemCode;
                SqlCmd.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = this.ItemDescription;
                if (this.linktoCategoryMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCategoryMasterId", SqlDbType.Int).Value = this.linktoCategoryMasterId;
                }
                if (this.linktoBrandMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBrandMasterId", SqlDbType.Int).Value = this.linktoBrandMasterId;
                }
                if (this.linktoColorMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoColorMasterId", SqlDbType.Int).Value = this.linktoColorMasterId;
                }
                SqlCmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = this.Price;
                SqlCmd.Parameters.Add("@SalesPrice", SqlDbType.Decimal).Value = this.SalesPrice;
                SqlCmd.Parameters.Add("@Vat", SqlDbType.Decimal).Value = this.Vat;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.ItemMasterId = Convert.ToInt32(SqlCmd.Parameters["@ItemMasterId"].Value);
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
        public loanRecordStatus UpdateItemMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ItemMasterId", SqlDbType.Int).Value = this.ItemMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = this.ItemName;
                SqlCmd.Parameters.Add("@ItemCode", SqlDbType.NVarChar).Value = this.ItemCode;
                SqlCmd.Parameters.Add("@ItemDescription", SqlDbType.NVarChar).Value = this.ItemDescription;
                if (this.linktoCategoryMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCategoryMasterId", SqlDbType.Int).Value = this.linktoCategoryMasterId;
                }
                if (this.linktoBrandMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoBrandMasterId", SqlDbType.Int).Value = this.linktoBrandMasterId;
                }
                if (this.linktoColorMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoColorMasterId", SqlDbType.Int).Value = this.linktoColorMasterId;
                }
                SqlCmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = this.Price;
                SqlCmd.Parameters.Add("@SalesPrice", SqlDbType.Decimal).Value = this.SalesPrice;
                SqlCmd.Parameters.Add("@Vat", SqlDbType.Decimal).Value = this.Vat;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
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
        public loanRecordStatus DeleteItemMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ItemMasterId", SqlDbType.Int).Value = this.ItemMasterId;
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
        public bool SelectItemMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ItemMasterId", SqlDbType.Int).Value = this.ItemMasterId;

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

        public List<loanItemMasterDAL> SelectAllItemMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = this.ItemName;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanItemMasterDAL> lstItemMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstItemMasterDAL;
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

        public static List<loanItemMasterDAL> SelectAllItemMasterItemName()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMasterItemName_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanItemMasterDAL> lstItemMasterDAL = new List<loanItemMasterDAL>();
                loanItemMasterDAL objItemMasterDAL = null;
                while (SqlRdr.Read())
                {
                    objItemMasterDAL = new loanItemMasterDAL();
                    objItemMasterDAL.ItemMasterId = Convert.ToInt32(SqlRdr["ItemMasterId"]);
                    objItemMasterDAL.ItemName = Convert.ToString(SqlRdr["ItemName"]);
                    lstItemMasterDAL.Add(objItemMasterDAL);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return lstItemMasterDAL;
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

        public static List<loanItemMasterDAL> SelectAllItemMasterItemNameBrand(int linktoCategoryMasterid = 0)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemMasterItemName_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;
                if (linktoCategoryMasterid > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCategoryMasterId", SqlDbType.Int).Value = linktoCategoryMasterid;
                }
                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanItemMasterDAL> lstItemMasterDAL = new List<loanItemMasterDAL>();
                loanItemMasterDAL objItemMasterDAL = null;
                while (SqlRdr.Read())
                {
                    objItemMasterDAL = new loanItemMasterDAL();
                    objItemMasterDAL.ItemMasterId = Convert.ToInt32(SqlRdr["ItemMasterId"]);
                    objItemMasterDAL.ItemName = Convert.ToString(SqlRdr["ItemName"]) + " " + Convert.ToString(SqlRdr["Brand"]) + " " +
                        Convert.ToString(SqlRdr["Color"]);
                    lstItemMasterDAL.Add(objItemMasterDAL);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return lstItemMasterDAL;
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

        public List<loanItemMasterDAL> SelectAllItemStockReportPageWise(DateTime fromDate, DateTime toDate, int itemMasterId)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanItemStockReportPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (fromDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate;
                }
                if (toDate != new DateTime())
                {
                    SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate;
                }
                if (itemMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = itemMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanItemMasterDAL> lstItemMasterDAL = SetListPropertiesFromSqlDataReaderItemStockReport(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstItemMasterDAL;
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
