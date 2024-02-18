using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanCustomerMaster
    /// </summary>
    public class loanCustomerMasterDAL
    {
        #region Properties
        public int CustomerMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public string CustomerName { get; set; }
        public string IdNo { get; set; }
        public string PhotoIdImageName { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }
        public string Gender { get; set; }
        public string Occupation { get; set; }
        public string PlaceOfWork { get; set; }
        public string CityOfResidence { get; set; }
        public string Address1 { get; set; }
        public string District { get; set; }
        public string Phone1 { get; set; }
        public string Relation1 { get; set; }
        public string ContactName1 { get; set; }
        public string Address2 { get; set; }
        public string Phone2 { get; set; }
        public string Relation2 { get; set; }
        public string ContactName2 { get; set; }
        public string Links { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }
        public bool? Hasurl { get; set; }
        public bool? IsRedFlag { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountNumber2 { get; set; }
        public string BankAccountNumber3 { get; set; }
        public string BankAccountNumber4 { get; set; }

        /// Extra
        public string xsPhotoIdImageName { get; set; }
        public string Guarantors { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool? InvalidMobile { get; set; }
        public bool? InvalidIdNo { get; set; }
        public int? linktoContractStatusMasterId { get; set; }

        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = System.Configuration.ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Customer/";
            if (sqlRdr.Read())
            {
                this.CustomerMasterId = Convert.ToInt32(sqlRdr["CustomerMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                this.IdNo = Convert.ToString(sqlRdr["IdNo"]);
                if (sqlRdr["PhotoIdImageName"] != DBNull.Value)
                {
                    this.PhotoIdImageName = Convert.ToString(sqlRdr["PhotoIdImageName"]);
                    this.xsPhotoIdImageName = ImageRetrieveUrl + "xs_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                }
                this.Mobile1 = Convert.ToString(sqlRdr["Mobile1"]);
                this.Mobile2 = Convert.ToString(sqlRdr["Mobile2"]);
                this.Mobile3 = Convert.ToString(sqlRdr["Mobile3"]);
                this.Gender = Convert.ToString(sqlRdr["Gender"]);
                this.Occupation = Convert.ToString(sqlRdr["Occupation"]);
                this.PlaceOfWork = Convert.ToString(sqlRdr["PlaceOfWork"]);
                this.CityOfResidence = Convert.ToString(sqlRdr["CityOfResidence"]);
                this.Address1 = Convert.ToString(sqlRdr["Address1"]);
                this.District = Convert.ToString(sqlRdr["District"]);
                this.Phone1 = Convert.ToString(sqlRdr["Phone1"]);
                this.Relation1 = Convert.ToString(sqlRdr["Relation1"]);
                this.ContactName1 = Convert.ToString(sqlRdr["ContactName1"]);
                this.Address2 = Convert.ToString(sqlRdr["Address2"]);
                this.Phone2 = Convert.ToString(sqlRdr["Phone2"]);
                this.Relation2 = Convert.ToString(sqlRdr["Relation2"]);
                this.ContactName2 = Convert.ToString(sqlRdr["ContactName2"]);
                this.Links = Convert.ToString(sqlRdr["Links"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                this.BankAccountNumber = Convert.ToString(sqlRdr["BankAccountNumber"]);
                this.BankAccountNumber2 = Convert.ToString(sqlRdr["BankAccountNumber2"]);
                this.BankAccountNumber3 = Convert.ToString(sqlRdr["BankAccountNumber3"]);
                this.BankAccountNumber4 = Convert.ToString(sqlRdr["BankAccountNumber4"]);

                /// Extra
                if (sqlRdr["IsRedFlag"] != DBNull.Value)
                {
                    this.IsRedFlag = Convert.ToBoolean(sqlRdr["IsRedFlag"]);
                }
                return true;
            }
            return false;
        }

        private List<loanCustomerMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = System.Configuration.ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Customer/";
            List<loanCustomerMasterDAL> lstCustomerMaster = new List<loanCustomerMasterDAL>();
            loanCustomerMasterDAL objCustomerMaster = null;
            while (sqlRdr.Read())
            {
                objCustomerMaster = new loanCustomerMasterDAL();
                objCustomerMaster.CustomerMasterId = Convert.ToInt32(sqlRdr["CustomerMasterId"]);
                objCustomerMaster.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objCustomerMaster.CustomerName = Convert.ToString(sqlRdr["CustomerName"]);
                objCustomerMaster.PhotoIdImageName = Convert.ToString(sqlRdr["PhotoIdImageName"]);
                objCustomerMaster.IdNo = Convert.ToString(sqlRdr["IdNo"]);
                if (!string.IsNullOrEmpty(objCustomerMaster.PhotoIdImageName))
                {
                    objCustomerMaster.xsPhotoIdImageName = ImageRetrieveUrl + "xs_" + Convert.ToString(sqlRdr["PhotoIdImageName"]);
                }

                objCustomerMaster.Mobile1 = Convert.ToString(sqlRdr["Mobile1"]);
                objCustomerMaster.Mobile2 = Convert.ToString(sqlRdr["Mobile2"]);
                objCustomerMaster.Mobile3 = Convert.ToString(sqlRdr["Mobile3"]);
                objCustomerMaster.Gender = Convert.ToString(sqlRdr["Gender"]);
                objCustomerMaster.Occupation = Convert.ToString(sqlRdr["Occupation"]);
                objCustomerMaster.PlaceOfWork = Convert.ToString(sqlRdr["PlaceOfWork"]);
                objCustomerMaster.CityOfResidence = Convert.ToString(sqlRdr["CityOfResidence"]);
                objCustomerMaster.Address1 = Convert.ToString(sqlRdr["Address1"]);
                objCustomerMaster.District = Convert.ToString(sqlRdr["District"]);
                objCustomerMaster.Phone1 = Convert.ToString(sqlRdr["Phone1"]);
                objCustomerMaster.Relation1 = Convert.ToString(sqlRdr["Relation1"]);
                objCustomerMaster.ContactName1 = Convert.ToString(sqlRdr["ContactName1"]);
                objCustomerMaster.Address2 = Convert.ToString(sqlRdr["Address2"]);
                objCustomerMaster.Phone2 = Convert.ToString(sqlRdr["Phone2"]);
                objCustomerMaster.Relation2 = Convert.ToString(sqlRdr["Relation2"]);
                objCustomerMaster.ContactName2 = Convert.ToString(sqlRdr["ContactName2"]);
                objCustomerMaster.Links = Convert.ToString(sqlRdr["Links"]);
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objCustomerMaster.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objCustomerMaster.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                objCustomerMaster.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objCustomerMaster.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                objCustomerMaster.BankAccountNumber = Convert.ToString(sqlRdr["BankAccountNumber"]);
                objCustomerMaster.BankAccountNumber2 = Convert.ToString(sqlRdr["BankAccountNumber2"]);
                objCustomerMaster.BankAccountNumber3 = Convert.ToString(sqlRdr["BankAccountNumber3"]);
                objCustomerMaster.BankAccountNumber4 = Convert.ToString(sqlRdr["BankAccountNumber4"]);
                if (sqlRdr["IsRedFlag"] != DBNull.Value)
                {
                    objCustomerMaster.IsRedFlag = Convert.ToBoolean(sqlRdr["IsRedFlag"]);
                }
                //extra
                objCustomerMaster.Guarantors = Convert.ToString(sqlRdr["Guarantors"]);
                objCustomerMaster.VerifiedBy = Convert.ToString(sqlRdr["VerifiedBy"]);
                objCustomerMaster.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);

                lstCustomerMaster.Add(objCustomerMaster);
            }
            return lstCustomerMaster;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertCustomerMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = this.CustomerName;
                SqlCmd.Parameters.Add("@IdNo", SqlDbType.NVarChar).Value = this.IdNo;
                SqlCmd.Parameters.Add("@PhotoIdImageName", SqlDbType.NVarChar).Value = this.PhotoIdImageName;
                SqlCmd.Parameters.Add("@Mobile1", SqlDbType.NVarChar).Value = this.Mobile1;
                SqlCmd.Parameters.Add("@Mobile2", SqlDbType.NVarChar).Value = this.Mobile2;
                SqlCmd.Parameters.Add("@Mobile3", SqlDbType.NVarChar).Value = this.Mobile3;
                SqlCmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = this.Gender;
                SqlCmd.Parameters.Add("@Occupation", SqlDbType.NVarChar).Value = this.Occupation;
                SqlCmd.Parameters.Add("@PlaceOfWork", SqlDbType.NVarChar).Value = this.PlaceOfWork;
                SqlCmd.Parameters.Add("@CityOfResidence", SqlDbType.NVarChar).Value = this.CityOfResidence;
                SqlCmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = this.Address1;
                SqlCmd.Parameters.Add("@District", SqlDbType.NVarChar).Value = this.District;
                SqlCmd.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = this.Phone1;
                SqlCmd.Parameters.Add("@Relation1", SqlDbType.NVarChar).Value = this.Relation1;
                SqlCmd.Parameters.Add("@ContactName1", SqlDbType.NVarChar).Value = this.ContactName1;
                SqlCmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = this.Address2;
                SqlCmd.Parameters.Add("@Phone2", SqlDbType.NVarChar).Value = this.Phone2;
                SqlCmd.Parameters.Add("@Relation2", SqlDbType.NVarChar).Value = this.Relation2;
                SqlCmd.Parameters.Add("@ContactName2", SqlDbType.NVarChar).Value = this.ContactName2;
                SqlCmd.Parameters.Add("@Links", SqlDbType.NVarChar).Value = this.Links;
                SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;
                SqlCmd.Parameters.Add("@BankAccountNumber2", SqlDbType.NVarChar).Value = this.BankAccountNumber2;
                SqlCmd.Parameters.Add("@BankAccountNumber3", SqlDbType.NVarChar).Value = this.BankAccountNumber3;
                SqlCmd.Parameters.Add("@BankAccountNumber4", SqlDbType.NVarChar).Value = this.BankAccountNumber4;
                SqlCmd.Parameters.Add("@IsRedFlag", SqlDbType.Bit).Value = this.IsRedFlag;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.CustomerMasterId = Convert.ToInt32(SqlCmd.Parameters["@CustomerMasterId"].Value);
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
        public loanRecordStatus UpdateCustomerMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerMasterId", SqlDbType.Int).Value = this.CustomerMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@CustomerName", SqlDbType.NVarChar).Value = this.CustomerName;
                SqlCmd.Parameters.Add("@IdNo", SqlDbType.NVarChar).Value = this.IdNo;
                SqlCmd.Parameters.Add("@PhotoIdImageName", SqlDbType.NVarChar).Value = this.PhotoIdImageName;
                SqlCmd.Parameters.Add("@Mobile1", SqlDbType.NVarChar).Value = this.Mobile1;
                SqlCmd.Parameters.Add("@Mobile2", SqlDbType.NVarChar).Value = this.Mobile2;
                SqlCmd.Parameters.Add("@Mobile3", SqlDbType.NVarChar).Value = this.Mobile3;
                SqlCmd.Parameters.Add("@Gender", SqlDbType.NVarChar).Value = this.Gender;
                SqlCmd.Parameters.Add("@Occupation", SqlDbType.NVarChar).Value = this.Occupation;
                SqlCmd.Parameters.Add("@PlaceOfWork", SqlDbType.NVarChar).Value = this.PlaceOfWork;
                SqlCmd.Parameters.Add("@CityOfResidence", SqlDbType.NVarChar).Value = this.CityOfResidence;
                SqlCmd.Parameters.Add("@Address1", SqlDbType.NVarChar).Value = this.Address1;
                SqlCmd.Parameters.Add("@District", SqlDbType.NVarChar).Value = this.District;
                SqlCmd.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = this.Phone1;
                SqlCmd.Parameters.Add("@Relation1", SqlDbType.NVarChar).Value = this.Relation1;
                SqlCmd.Parameters.Add("@ContactName1", SqlDbType.NVarChar).Value = this.ContactName1;
                SqlCmd.Parameters.Add("@Address2", SqlDbType.NVarChar).Value = this.Address2;
                SqlCmd.Parameters.Add("@Phone2", SqlDbType.NVarChar).Value = this.Phone2;
                SqlCmd.Parameters.Add("@Relation2", SqlDbType.NVarChar).Value = this.Relation2;
                SqlCmd.Parameters.Add("@ContactName2", SqlDbType.NVarChar).Value = this.ContactName2;
                SqlCmd.Parameters.Add("@Links", SqlDbType.NVarChar).Value = this.Links;
                SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;
                SqlCmd.Parameters.Add("@BankAccountNumber2", SqlDbType.NVarChar).Value = this.BankAccountNumber2;
                SqlCmd.Parameters.Add("@BankAccountNumber3", SqlDbType.NVarChar).Value = this.BankAccountNumber3;
                SqlCmd.Parameters.Add("@BankAccountNumber4", SqlDbType.NVarChar).Value = this.BankAccountNumber4;
                SqlCmd.Parameters.Add("@IsRedFlag", SqlDbType.Bit).Value = this.IsRedFlag;
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
        public loanRecordStatus DeleteCustomerMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerMasterId", SqlDbType.Int).Value = this.CustomerMasterId;
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
        public bool SelectCustomerMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.CustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@CustomerMasterId", SqlDbType.Int).Value = this.CustomerMasterId;
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

        public List<loanCustomerMasterDAL> SelectAllCustomerMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.CustomerName;
                SqlCmd.Parameters.Add("@Phone1", SqlDbType.NVarChar).Value = this.Phone1;
                SqlCmd.Parameters.Add("@Guarantors", SqlDbType.NVarChar).Value = this.Guarantors;
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;
                SqlCmd.Parameters.Add("@Hasurl", SqlDbType.Bit).Value = this.Hasurl;
                SqlCmd.Parameters.Add("@InvalidMobile", SqlDbType.Bit).Value = this.InvalidMobile;
                SqlCmd.Parameters.Add("@InvalidIdNo", SqlDbType.Bit).Value = this.InvalidIdNo;
                SqlCmd.Parameters.Add("@linktoContractStatusMasterId", SqlDbType.Int).Value = this.linktoContractStatusMasterId;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCustomerMasterDAL> lstCustomerMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;

                return lstCustomerMasterDAL;
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

        public static List<loanCustomerMasterDAL> SelectAllCustomerMasterCustomerName()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMasterCustomerName_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCustomerMasterDAL> lstCustomerMasterDAL = new List<loanCustomerMasterDAL>();
                loanCustomerMasterDAL objCustomerMasterDAL = null;
                while (SqlRdr.Read())
                {
                    objCustomerMasterDAL = new loanCustomerMasterDAL();
                    objCustomerMasterDAL.CustomerMasterId = Convert.ToInt32(SqlRdr["CustomerMasterId"]);
                    objCustomerMasterDAL.CustomerName = Convert.ToString(SqlRdr["CustomerName"]);
                    lstCustomerMasterDAL.Add(objCustomerMasterDAL);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return lstCustomerMasterDAL;
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

        public static List<loanCustomerMasterDAL> SelectAllCustomerMasterBankAccountNumbers(int customerMasterId)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerMasterBankAccountNumbers_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerMasterId", SqlDbType.NVarChar).Value = customerMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCustomerMasterDAL> lstCustomerMasterDAL = new List<loanCustomerMasterDAL>();
                loanCustomerMasterDAL objCustomerMasterDAL = null;
                while (SqlRdr.Read())
                {
                    objCustomerMasterDAL = new loanCustomerMasterDAL();
                    objCustomerMasterDAL.CustomerMasterId = Convert.ToInt32(SqlRdr["CustomerMasterId"]);
                    objCustomerMasterDAL.BankAccountNumber = Convert.ToString(SqlRdr["BankAccountNumber"]);
                    lstCustomerMasterDAL.Add(objCustomerMasterDAL);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return lstCustomerMasterDAL;
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
