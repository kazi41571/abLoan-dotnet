using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanZakaahPaymentMaster
    /// </summary>
    public class loanZakaahPaymentMasterDAL
    {
        #region Properties
        public int ZakaahPaymentMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ActiveDate { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal ZakaahAmount { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.ZakaahPaymentMasterId = Convert.ToInt32(sqlRdr["ZakaahPaymentMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.PaymentDate = Convert.ToDateTime(sqlRdr["PaymentDate"]);
                this.FromDate = Convert.ToDateTime(sqlRdr["FromDate"]);
                this.ToDate = Convert.ToDateTime(sqlRdr["ToDate"]);
                this.ActiveDate = Convert.ToDateTime(sqlRdr["ActiveDate"]);
                this.PendingAmount = Convert.ToDecimal(sqlRdr["PendingAmount"]);
                this.ZakaahAmount = Convert.ToDecimal(sqlRdr["ZakaahAmount"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                return true;
            }
            return false;
        }

        private List<loanZakaahPaymentMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanZakaahPaymentMasterDAL> lstZakaahPaymentMaster = new List<loanZakaahPaymentMasterDAL>();
            loanZakaahPaymentMasterDAL objZakaahPaymentMaster = null;
            while (sqlRdr.Read())
            {
                objZakaahPaymentMaster = new loanZakaahPaymentMasterDAL();
                objZakaahPaymentMaster.ZakaahPaymentMasterId = Convert.ToInt32(sqlRdr["ZakaahPaymentMasterId"]);
                objZakaahPaymentMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objZakaahPaymentMaster.PaymentDate = Convert.ToDateTime(sqlRdr["PaymentDate"]);
                objZakaahPaymentMaster.FromDate = Convert.ToDateTime(sqlRdr["FromDate"]);
                objZakaahPaymentMaster.ToDate = Convert.ToDateTime(sqlRdr["ToDate"]);
                objZakaahPaymentMaster.ActiveDate = Convert.ToDateTime(sqlRdr["ActiveDate"]);
                objZakaahPaymentMaster.PendingAmount = Convert.ToDecimal(sqlRdr["PendingAmount"]);
                objZakaahPaymentMaster.ZakaahAmount = Convert.ToDecimal(sqlRdr["ZakaahAmount"]);
                objZakaahPaymentMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objZakaahPaymentMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                lstZakaahPaymentMaster.Add(objZakaahPaymentMaster);
            }
            return lstZakaahPaymentMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertZakaahPaymentMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanZakaahPaymentMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ZakaahPaymentMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@PaymentDate", SqlDbType.Date).Value = this.PaymentDate;
                SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                SqlCmd.Parameters.Add("@ActiveDate", SqlDbType.Date).Value = this.ActiveDate;
                SqlCmd.Parameters.Add("@PendingAmount", SqlDbType.Money).Value = this.PendingAmount;
                SqlCmd.Parameters.Add("@ZakaahAmount", SqlDbType.Money).Value = this.ZakaahAmount;

                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.ZakaahPaymentMasterId = Convert.ToInt32(SqlCmd.Parameters["@ZakaahPaymentMasterId"].Value);
                loanRecordStatus rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;

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
            }
        }
        #endregion

        #region Update
        public loanRecordStatus UpdateZakaahPaymentMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanZakaahPaymentMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ZakaahPaymentMasterId", SqlDbType.Int).Value = this.ZakaahPaymentMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }

                SqlCmd.Parameters.Add("@PaymentDate", SqlDbType.Date).Value = this.PaymentDate;
                SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                SqlCmd.Parameters.Add("@ActiveDate", SqlDbType.Date).Value = this.ActiveDate;
                SqlCmd.Parameters.Add("@PendingAmount", SqlDbType.Money).Value = this.PendingAmount;
                SqlCmd.Parameters.Add("@ZakaahAmount", SqlDbType.Money).Value = this.ZakaahAmount;

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
        public loanRecordStatus DeleteZakaahPaymentMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanZakaahPaymentMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ZakaahPaymentMasterId", SqlDbType.Int).Value = this.ZakaahPaymentMasterId;
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
        public bool SelectZakaahPaymentMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanZakaahPaymentMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ZakaahPaymentMasterId", SqlDbType.Int).Value = this.ZakaahPaymentMasterId;

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

        public List<loanZakaahPaymentMasterDAL> SelectAllZakaahPaymentMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanZakaahPaymentMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = this.FromDate;
                SqlCmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = this.ToDate;
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanZakaahPaymentMasterDAL> lstZakaahPaymentMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstZakaahPaymentMasterDAL;
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
