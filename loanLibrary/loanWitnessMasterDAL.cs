using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanWitnessMaster
    /// </summary>
    public class loanWitnessMasterDAL
    {
        #region Properties
        public int WitnessMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public string WitnessName { get; set; }
        public string IdNo { get; set; }
        public string PhotoIdImageName { get; set; }
        public string Address1 { get; set; }
        public string Phone1 { get; set; }
        public string Mobile1 { get; set; }
        public string Fax1 { get; set; }
        public string Address2 { get; set; }
        public string Phone2 { get; set; }
        public string Mobile2 { get; set; }
        public string Fax2 { get; set; }
        public string Mobile3 { get; set; }
        //public string City { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string xsPhotoIdImageName { get; set; }
        public string smPhotoIdImageName { get; set; }
        public string mdPhotoIdImageName { get; set; }
        public string lgPhotoIdImageName { get; set; }
        public string xlPhotoIdImageName { get; set; }
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = System.Configuration.ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Witness/";
            if (sqlRdr.Read())
            {
                this.WitnessMasterId = Convert.ToInt32(sqlRdr["WitnessMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.WitnessName = Convert.ToString(sqlRdr["WitnessName"]);
                this.IdNo = Convert.ToString(sqlRdr["IdNo"]);
                this.PhotoIdImageName = Convert.ToString(sqlRdr["PhotoIdImageName"]);
                this.Address1 = Convert.ToString(sqlRdr["Address1"]);
                this.Phone1 = Convert.ToString(sqlRdr["Phone1"]);
                this.Mobile1 = Convert.ToString(sqlRdr["Mobile1"]);
                this.Fax1 = Convert.ToString(sqlRdr["Fax1"]);
                this.Address2 = Convert.ToString(sqlRdr["Address2"]);
                this.Phone2 = Convert.ToString(sqlRdr["Phone2"]);
                this.Mobile2 = Convert.ToString(sqlRdr["Mobile2"]);
                this.Fax2 = Convert.ToString(sqlRdr["Fax2"]);
                this.Mobile3 = Convert.ToString(sqlRdr["Mobile3"]);
                //this.City = Convert.ToString(sqlRdr["City"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.xsPhotoIdImageName = ImageRetrieveUrl + "xs_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                this.smPhotoIdImageName = ImageRetrieveUrl + "sm_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                this.mdPhotoIdImageName = ImageRetrieveUrl + "md_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                this.lgPhotoIdImageName = ImageRetrieveUrl + "lg_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                this.xlPhotoIdImageName = ImageRetrieveUrl + "xl_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                return true;
            }
            return false;
        }

        private List<loanWitnessMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = System.Configuration.ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Witness/";
            List<loanWitnessMasterDAL> lstWitnessMaster = new List<loanWitnessMasterDAL>();
            loanWitnessMasterDAL objWitnessMaster = null;
            while (sqlRdr.Read())
            {
                objWitnessMaster = new loanWitnessMasterDAL();
                objWitnessMaster.WitnessMasterId = Convert.ToInt32(sqlRdr["WitnessMasterId"]);
                objWitnessMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objWitnessMaster.WitnessName = Convert.ToString(sqlRdr["WitnessName"]);
                objWitnessMaster.IdNo = Convert.ToString(sqlRdr["IdNo"]);
                objWitnessMaster.PhotoIdImageName = Convert.ToString(sqlRdr["PhotoIdImageName"]);
                objWitnessMaster.Address1 = Convert.ToString(sqlRdr["Address1"]);
                objWitnessMaster.Phone1 = Convert.ToString(sqlRdr["Phone1"]);
                objWitnessMaster.Mobile1 = Convert.ToString(sqlRdr["Mobile1"]);
                objWitnessMaster.Fax1 = Convert.ToString(sqlRdr["Fax1"]);
                objWitnessMaster.Address2 = Convert.ToString(sqlRdr["Address2"]);
                objWitnessMaster.Phone2 = Convert.ToString(sqlRdr["Phone2"]);
                objWitnessMaster.Mobile2 = Convert.ToString(sqlRdr["Mobile2"]);
                objWitnessMaster.Fax2 = Convert.ToString(sqlRdr["Fax2"]);
                objWitnessMaster.Mobile3 = Convert.ToString(sqlRdr["Mobile3"]);
                //objWitnessMaster.City = Convert.ToString(sqlRdr["City"]);
                objWitnessMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objWitnessMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objWitnessMaster.xsPhotoIdImageName = ImageRetrieveUrl + "xs_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                objWitnessMaster.smPhotoIdImageName = ImageRetrieveUrl + "sm_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                objWitnessMaster.mdPhotoIdImageName = ImageRetrieveUrl + "md_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                objWitnessMaster.lgPhotoIdImageName = ImageRetrieveUrl + "lg_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                objWitnessMaster.xlPhotoIdImageName = ImageRetrieveUrl + "xl_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                lstWitnessMaster.Add(objWitnessMaster);
            }
            return lstWitnessMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertWitnessMaster(int? contractMasterId = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                loanRecordStatus rs = loanRecordStatus.Success;

                if (this.WitnessMasterId == 0)
                {
                    SqlCon = loanObjectFactoryDAL.CreateConnection();
                    SqlCmd = new SqlCommand("loanWitnessMaster_Insert", SqlCon);
                    SqlCmd.CommandType = CommandType.StoredProcedure;

                    SqlCmd.Parameters.Add("@WitnessMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    if (this.linktoCompanyMasterId > 0)
                    {
                        SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                    }
                    SqlCmd.Parameters.Add("@WitnessName", SqlDbType.NVarChar).Value = this.WitnessName;
                    SqlCmd.Parameters.Add("@IdNo", SqlDbType.NVarChar).Value = this.IdNo;
                    SqlCmd.Parameters.Add("@PhotoIdImageName", SqlDbType.NVarChar).Value = this.PhotoIdImageName;
                    SqlCmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = this.Address1;
                    SqlCmd.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = this.Phone1;
                    SqlCmd.Parameters.Add("@Mobile1", SqlDbType.NVarChar).Value = this.Mobile1;
                    SqlCmd.Parameters.Add("@Fax1", SqlDbType.NVarChar).Value = this.Fax1;
                    SqlCmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = this.Address2;
                    SqlCmd.Parameters.Add("@Phone2", SqlDbType.NVarChar).Value = this.Phone2;
                    SqlCmd.Parameters.Add("@Mobile2", SqlDbType.NVarChar).Value = this.Mobile2;
                    SqlCmd.Parameters.Add("@Fax2", SqlDbType.NVarChar).Value = this.Fax2;
                    SqlCmd.Parameters.Add("@Mobile3", SqlDbType.NVarChar).Value = this.Mobile3;
                    //SqlCmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = this.City;
                    SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                    SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                    SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                    SqlCon.Open();
                    SqlCmd.ExecuteNonQuery();
                    SqlCon.Close();

                    this.WitnessMasterId = Convert.ToInt32(SqlCmd.Parameters["@WitnessMasterId"].Value);
                    rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                }

                if (rs == loanRecordStatus.Success && contractMasterId != null)
                {
                    loanContractWitnessTranDAL objContractWitnessTranDAL = new loanContractWitnessTranDAL();
                    objContractWitnessTranDAL.linktoContractMasterId = contractMasterId.Value;
                    objContractWitnessTranDAL.linktoWitnessMasterId = this.WitnessMasterId;
                    rs = objContractWitnessTranDAL.InsertContractWitnessTran();
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
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion

        #region Update
        public loanRecordStatus UpdateWitnessMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanWitnessMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@WitnessMasterId", SqlDbType.Int).Value = this.WitnessMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@WitnessName", SqlDbType.NVarChar).Value = this.WitnessName;
                SqlCmd.Parameters.Add("@IdNo", SqlDbType.NVarChar).Value = this.IdNo;
                SqlCmd.Parameters.Add("@PhotoIdImageName", SqlDbType.NVarChar).Value = this.PhotoIdImageName;
                SqlCmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = this.Address1;
                SqlCmd.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = this.Phone1;
                SqlCmd.Parameters.Add("@Mobile1", SqlDbType.NVarChar).Value = this.Mobile1;
                SqlCmd.Parameters.Add("@Fax1", SqlDbType.NVarChar).Value = this.Fax1;
                SqlCmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = this.Address2;
                SqlCmd.Parameters.Add("@Phone2", SqlDbType.NVarChar).Value = this.Phone2;
                SqlCmd.Parameters.Add("@Mobile2", SqlDbType.NVarChar).Value = this.Mobile2;
                SqlCmd.Parameters.Add("@Fax2", SqlDbType.NVarChar).Value = this.Fax2;
                SqlCmd.Parameters.Add("@Mobile3", SqlDbType.NVarChar).Value = this.Mobile3;
                //SqlCmd.Parameters.Add("@City", SqlDbType.NVarChar).Value = this.City;
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
        public loanRecordStatus DeleteWitnessMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanWitnessMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@WitnessMasterId", SqlDbType.Int).Value = this.WitnessMasterId;
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
        public bool SelectWitnessMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanWitnessMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.WitnessMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@WitnessMasterId", SqlDbType.Int).Value = this.WitnessMasterId;
                }
                if (!string.IsNullOrEmpty(this.IdNo))
                {
                    SqlCmd.Parameters.Add("@IdNo", SqlDbType.VarChar).Value = this.IdNo;
                }

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

        public List<loanWitnessMasterDAL> SelectAllWitnessMasterPageWise(int startRowIndex, int pageSize, out int totalRecords, int? contractMasterId = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanWitnessMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Witness", SqlDbType.NVarChar).Value = this.WitnessName;
                SqlCmd.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = this.Phone1;

                if (contractMasterId != null)
                {
                    SqlCmd.Parameters.Add("@ContractMasterId", SqlDbType.Int).Value = contractMasterId;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanWitnessMasterDAL> lstWitnessMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstWitnessMasterDAL;
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
