using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanUserMaster
    /// </summary>
    public class loanUserMasterDAL
    {
        #region Properties
        public int UserMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int linktoRoleMasterId { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
        public int LoginFailCount { get; set; }
        public DateTime? LastLockoutDateTime { get; set; }
        public DateTime? LastPasswordChangedDateTime { get; set; }
        public DateTime? LastDataSyncDateTime { get; set; }
        public string Comment { get; set; }
        public bool IsEnabled { get; set; }
        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }
        public string VerificationCode { get; set; }
        public DateTime VerificationCodeDateTime { get; set; }

        /// Extra        
        public string Role { get; set; }
        public string CompanyName { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }

        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "User/";
            string LogoImageRetrieveUrl = ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Business/";
            if (sqlRdr.Read())
            {
                this.UserMasterId = Convert.ToInt32(sqlRdr["UserMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.Username = Convert.ToString(sqlRdr["Username"]);
                this.Password = Convert.ToString(sqlRdr["Password"]);
                this.Email = Convert.ToString(sqlRdr["Email"]);
                this.linktoRoleMasterId = Convert.ToInt32(sqlRdr["linktoRoleMasterId"]);
                if (sqlRdr["LastLoginDateTime"] != DBNull.Value)
                {
                    this.LastLoginDateTime = Convert.ToDateTime(sqlRdr["LastLoginDateTime"]);
                }
                this.LoginFailCount = Convert.ToInt32(sqlRdr["LoginFailCount"]);
                if (sqlRdr["LastLockoutDateTime"] != DBNull.Value)
                {
                    this.LastLockoutDateTime = Convert.ToDateTime(sqlRdr["LastLockoutDateTime"]);
                }
                if (sqlRdr["LastPasswordChangedDateTime"] != DBNull.Value)
                {
                    this.LastPasswordChangedDateTime = Convert.ToDateTime(sqlRdr["LastPasswordChangedDateTime"]);
                }

                this.Comment = Convert.ToString(sqlRdr["Comment"]);
                this.IsEnabled = Convert.ToBoolean(sqlRdr["IsEnabled"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);
                if (sqlRdr["VerificationCode"] != DBNull.Value)
                {
                    this.VerificationCode = Convert.ToString(sqlRdr["VerificationCode"]);
                }
                if (sqlRdr["VerificationCodeDateTime"] != DBNull.Value)
                {
                    this.VerificationCodeDateTime = Convert.ToDateTime(sqlRdr["VerificationCodeDateTime"]);
                }

                /// Extra
                this.Role = Convert.ToString(sqlRdr["Role"]);
                this.CompanyName = Convert.ToString(sqlRdr["CompanyName"]);

                return true;
            }
            return false;
        }

        private List<loanUserMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            string ImageRetrieveUrl = ConfigurationManager.AppSettings["ImageRetrieveUrl"] + "Employee/";
            List<loanUserMasterDAL> lstUserMasterDAL = new List<loanUserMasterDAL>();
            loanUserMasterDAL objUserMasterDAL = null;
            while (sqlRdr.Read())
            {
                objUserMasterDAL = new loanUserMasterDAL();
                objUserMasterDAL.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objUserMasterDAL.UserMasterId = Convert.ToInt32(sqlRdr["UserMasterId"]);
                objUserMasterDAL.Username = Convert.ToString(sqlRdr["Username"]);
                objUserMasterDAL.Password = Convert.ToString(sqlRdr["Password"]);
                objUserMasterDAL.Email = Convert.ToString(sqlRdr["Email"]);
                objUserMasterDAL.linktoRoleMasterId = Convert.ToInt32(sqlRdr["linktoRoleMasterId"]);
                if (sqlRdr["LastLoginDateTime"] != DBNull.Value)
                {
                    objUserMasterDAL.LastLoginDateTime = Convert.ToDateTime(sqlRdr["LastLoginDateTime"]);
                }

                objUserMasterDAL.LoginFailCount = Convert.ToInt32(sqlRdr["LoginFailCount"]);
                if (sqlRdr["LastLockoutDateTime"] != DBNull.Value)
                {
                    objUserMasterDAL.LastLockoutDateTime = Convert.ToDateTime(sqlRdr["LastLockoutDateTime"]);
                }
                if (sqlRdr["LastPasswordChangedDateTime"] != DBNull.Value)
                {
                    objUserMasterDAL.LastPasswordChangedDateTime = Convert.ToDateTime(sqlRdr["LastPasswordChangedDateTime"]);
                }

                objUserMasterDAL.Comment = Convert.ToString(sqlRdr["Comment"]);
                objUserMasterDAL.IsEnabled = Convert.ToBoolean(sqlRdr["IsEnabled"]);
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objUserMasterDAL.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objUserMasterDAL.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                objUserMasterDAL.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objUserMasterDAL.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objUserMasterDAL.Role = Convert.ToString(sqlRdr["Role"]);
                objUserMasterDAL.CompanyName = Convert.ToString(sqlRdr["CompanyName"]);
                objUserMasterDAL.VerifiedBy = Convert.ToString(sqlRdr["VerifiedBy"]);
                objUserMasterDAL.ModifiedBy = Convert.ToString(sqlRdr["ModifiedBy"]);

                lstUserMasterDAL.Add(objUserMasterDAL);
            }
            return lstUserMasterDAL;
        }
        #endregion

        #region Insert       

        public loanRecordStatus InsertUserMaster(SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                if (sqlCon == null)
                {
                    SqlCon = loanObjectFactoryDAL.CreateConnection();
                    SqlCmd = new SqlCommand("loanUserMaster_Insert", SqlCon);
                }
                else
                {
                    SqlCmd = new SqlCommand("loanUserMaster_Insert", sqlCon, sqlTran);
                }
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = this.Username;
                SqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = this.Password;
                SqlCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = this.Email;
                if (this.linktoRoleMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoRoleMasterId", SqlDbType.Int).Value = this.linktoRoleMasterId;
                }
                SqlCmd.Parameters.Add("@LastLoginDateTime", SqlDbType.DateTime).Value = this.LastLoginDateTime;
                SqlCmd.Parameters.Add("@LoginFailCount", SqlDbType.Int).Value = this.LoginFailCount;
                SqlCmd.Parameters.Add("@LastLockoutDateTime", SqlDbType.DateTime).Value = this.LastLockoutDateTime;
                SqlCmd.Parameters.Add("@LastPasswordChangedDateTime", SqlDbType.DateTime).Value = this.LastPasswordChangedDateTime;
                SqlCmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = this.Comment;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
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
                this.UserMasterId = Convert.ToInt32(SqlCmd.Parameters["@UserMasterId"].Value);
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

        #region Update
        public loanRecordStatus UpdateUserMaster(SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                if (sqlCon == null)
                {
                    SqlCon = loanObjectFactoryDAL.CreateConnection();
                    SqlCmd = new SqlCommand("loanUserMaster_Update", SqlCon);
                }
                else
                {
                    SqlCmd = new SqlCommand("loanUserMaster_Update", sqlCon, sqlTran);
                }
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = this.Username;
                SqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = this.Password;
                SqlCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = this.Email;
                if (this.linktoRoleMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoRoleMasterId", SqlDbType.Int).Value = this.linktoRoleMasterId;
                }
                SqlCmd.Parameters.Add("@LastLoginDateTime", SqlDbType.DateTime).Value = this.LastLoginDateTime;
                SqlCmd.Parameters.Add("@LoginFailCount", SqlDbType.Int).Value = this.LoginFailCount;
                SqlCmd.Parameters.Add("@LastLockoutDateTime", SqlDbType.DateTime).Value = this.LastLockoutDateTime;
                SqlCmd.Parameters.Add("@LastPasswordChangedDateTime", SqlDbType.DateTime).Value = this.LastPasswordChangedDateTime;
                SqlCmd.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = this.Comment;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
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

        public loanRecordStatus UpdateUserMasterLastLoginDateTime(int? loginFailCount = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMasterLastLoginDateTime_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;
                SqlCmd.Parameters.Add("@LastLoginDateTime", SqlDbType.DateTime).Value = this.LastLoginDateTime;
                SqlCmd.Parameters.Add("@LoginFailCount", SqlDbType.VarChar).Value = loginFailCount;
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

        public loanRecordStatus UpdateUserMasterPassword()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMasterPassword_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;
                SqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = this.Password;
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

        public loanRecordStatus UpdateUserMasterUnlock()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMasterUnlock_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;
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

        public bool SendVerificationCode()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                string Number = loanGlobalsDAL.GetRandomNumber(6);

                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMasterSendVerificationCode_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;
                SqlCmd.Parameters.Add("@VerificationCode", SqlDbType.NVarChar).Value = Number;
                SqlCmd.Parameters.Add("@VerificationCodeDateTime", SqlDbType.DateTime).Value = loanGlobalsDAL.GetCurrentDateTime();
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                string mailBody = "Your one-time code is: <b>" + Number + "</b><br><br>Please verify you're really you by entering this 6-digit code when you sign in. Just a heads up, this code will expire in 20 minutes for security reasons.";

                return loanGlobalsDAL.SendEmail(this.Email, "Al-Dahash : Sign In Verification Code", mailBody);
            }
            catch (Exception ex)
            {
                loanGlobalsDAL.SaveError(ex);
                return false;
            }
            finally
            {
                loanObjectFactoryDAL.DisposeCommand(SqlCmd);
                loanObjectFactoryDAL.DisposeConnection(SqlCon);
            }
        }
        #endregion

        #region Delete
        public loanRecordStatus DeleteUserMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;
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
        public bool SelectUserMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@UserMasterId", SqlDbType.Int).Value = this.UserMasterId;

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

        public bool SelectUserMasterByUsername()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMasterByUsername_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = this.Username;

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
        public List<loanUserMasterDAL> SelectAllUserMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = this.Username;
                if (this.linktoRoleMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoRoleMasterId", SqlDbType.Int).Value = this.linktoRoleMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
                SqlCmd.Parameters.Add("@IsVerified", SqlDbType.Bit).Value = this.IsVerified;

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanUserMasterDAL> lstUserMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstUserMasterDAL;
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

        public static List<loanUserMasterDAL> SelectAllUserMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanUserMaster_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanUserMasterDAL> lstUserMasterDAL = new List<loanUserMasterDAL>();
                loanUserMasterDAL objUserMasterDAL = null;
                while (SqlRdr.Read())
                {
                    objUserMasterDAL = new loanUserMasterDAL();
                    objUserMasterDAL.UserMasterId = Convert.ToInt32(SqlRdr["UserMasterId"]);
                    objUserMasterDAL.Username = Convert.ToString(SqlRdr["Username"]);
                    lstUserMasterDAL.Add(objUserMasterDAL);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return lstUserMasterDAL;
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
