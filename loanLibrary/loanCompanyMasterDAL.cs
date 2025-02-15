using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanCompanyMaster
    /// </summary>
    public class loanCompanyMasterDAL
    {
        #region Properties
        public int CompanyMasterId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDetails { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string LogoImageName { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string xsLogoImageName { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = System.Configuration.ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Company/";
            if (sqlRdr.Read())
            {
                this.CompanyMasterId = Convert.ToInt32(sqlRdr["CompanyMasterId"]);
                this.CompanyName = Convert.ToString(sqlRdr["CompanyName"]);
                this.CompanyDetails = Convert.ToString(sqlRdr["CompanyDetails"]);
                this.Address = Convert.ToString(sqlRdr["Address"]);
                this.ContactNo = Convert.ToString(sqlRdr["ContactNo"]);
                this.LogoImageName = Convert.ToString(sqlRdr["LogoImageName"]);
                if (sqlRdr["LogoImageName"] != DBNull.Value)
                {
                    this.LogoImageName = Convert.ToString(sqlRdr["LogoImageName"]);
                    this.xsLogoImageName = ImageRetrieveUrl + "xs_" + Convert.ToString(sqlRdr["LogoImageName"]);
                }
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                return true;
            }
            return false;
        }

        private List<loanCompanyMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = System.Configuration.ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Company/";
            List<loanCompanyMasterDAL> lstCompanyMaster = new List<loanCompanyMasterDAL>();
            loanCompanyMasterDAL objCompanyMaster = null;
            while (sqlRdr.Read())
            {
                objCompanyMaster = new loanCompanyMasterDAL();
                objCompanyMaster.CompanyMasterId = Convert.ToInt32(sqlRdr["CompanyMasterId"]);
                objCompanyMaster.CompanyName = Convert.ToString(sqlRdr["CompanyName"]);
                objCompanyMaster.CompanyDetails = Convert.ToString(sqlRdr["CompanyDetails"]);
                objCompanyMaster.Address = Convert.ToString(sqlRdr["Address"]);
                objCompanyMaster.ContactNo = Convert.ToString(sqlRdr["ContactNo"]);
                objCompanyMaster.LogoImageName = Convert.ToString(sqlRdr["LogoImageName"]);
                if (!string.IsNullOrEmpty(objCompanyMaster.LogoImageName))
                {
                    objCompanyMaster.xsLogoImageName = ImageRetrieveUrl + "xs_" + Convert.ToString(sqlRdr["LogoImageName"]);
                }
                objCompanyMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objCompanyMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                lstCompanyMaster.Add(objCompanyMaster);
            }
            return lstCompanyMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertCompanyMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCompanyMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CompanyMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                SqlCmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = this.CompanyName;
                SqlCmd.Parameters.Add("@CompanyDetails", SqlDbType.NVarChar).Value = this.CompanyDetails;
                SqlCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = this.Address;
                SqlCmd.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = this.ContactNo;
                SqlCmd.Parameters.Add("@LogoImageName", SqlDbType.NVarChar).Value = this.LogoImageName;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.CompanyMasterId = Convert.ToInt32(SqlCmd.Parameters["@CompanyMasterId"].Value);
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
        public loanRecordStatus UpdateCompanyMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCompanyMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CompanyMasterId", SqlDbType.Int).Value = this.CompanyMasterId;
                SqlCmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar).Value = this.CompanyName;
                SqlCmd.Parameters.Add("@CompanyDetails", SqlDbType.NVarChar).Value = this.CompanyDetails;
                SqlCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = this.Address;
                SqlCmd.Parameters.Add("@ContactNo", SqlDbType.NVarChar).Value = this.ContactNo;
                SqlCmd.Parameters.Add("@LogoImageName", SqlDbType.NVarChar).Value = this.LogoImageName;
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
        public loanRecordStatus DeleteCompanyMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCompanyMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CompanyMasterId", SqlDbType.Int).Value = this.CompanyMasterId;
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
        public bool SelectCompanyMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCompanyMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CompanyMasterId", SqlDbType.Int).Value = this.CompanyMasterId;

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

        public List<loanCompanyMasterDAL> SelectAllCompanyMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCompanyMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCompanyMasterDAL> lstCompanyMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstCompanyMasterDAL;
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
