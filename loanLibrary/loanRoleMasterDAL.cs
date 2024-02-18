using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanRoleMaster
    /// </summary>
    public class loanRoleMasterDAL
    {
        #region Properties
        public int RoleMasterId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }


        /// Extra
        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.RoleMasterId = Convert.ToInt32(sqlRdr["RoleMasterId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.Role = Convert.ToString(sqlRdr["Role"]);
                this.Description = Convert.ToString(sqlRdr["Description"]);
                this.IsEnabled = Convert.ToBoolean(sqlRdr["IsEnabled"]);
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                return true;
            }
            return false;
        }

        private List<loanRoleMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanRoleMasterDAL> lstRoleMasterDAL = new List<loanRoleMasterDAL>();
            loanRoleMasterDAL objRoleMasterDAL = null;
            while (sqlRdr.Read())
            {
                objRoleMasterDAL = new loanRoleMasterDAL();
                objRoleMasterDAL.RoleMasterId = Convert.ToInt32(sqlRdr["RoleMasterId"]);
                objRoleMasterDAL.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objRoleMasterDAL.Role = Convert.ToString(sqlRdr["Role"]);
                objRoleMasterDAL.Description = Convert.ToString(sqlRdr["Description"]);
                objRoleMasterDAL.IsEnabled = Convert.ToBoolean(sqlRdr["IsEnabled"]);
                objRoleMasterDAL.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objRoleMasterDAL.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                lstRoleMasterDAL.Add(objRoleMasterDAL);
            }
            return lstRoleMasterDAL;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertRoleMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleMaster_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@RoleMasterId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = this.Role;
                SqlCmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = this.Description;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;
                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.RoleMasterId = Convert.ToInt32(SqlCmd.Parameters["@RoleMasterId"].Value);
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
        public loanRecordStatus UpdateRoleMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleMaster_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@RoleMasterId", SqlDbType.Int).Value = this.RoleMasterId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = this.Role;
                SqlCmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = this.Description;
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
        public loanRecordStatus DeleteRoleMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleMaster_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@RoleMasterId", SqlDbType.Int).Value = this.RoleMasterId;
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
        public bool SelectRoleMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleMaster_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@RoleMasterId", SqlDbType.Int).Value = this.RoleMasterId;

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

        public List<loanRoleMasterDAL> SelectAllRoleMasterPageWise(int startRowIndex, int pageSize, out int totalRecords)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleMasterPageWise_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value = this.Role;
                SqlCmd.Parameters.Add("@IsEnabled", SqlDbType.Bit).Value = this.IsEnabled;


                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanRoleMasterDAL> lstRoleMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstRoleMasterDAL;
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

        public static List<loanRoleMasterDAL> SelectAllRoleMasterRole()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleMasterRole_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanRoleMasterDAL> lstRoleMasterDAL = new List<loanRoleMasterDAL>();
                loanRoleMasterDAL objRoleMasterDAL = null;
                while (SqlRdr.Read())
                {
                    objRoleMasterDAL = new loanRoleMasterDAL();
                    objRoleMasterDAL.RoleMasterId = Convert.ToInt32(SqlRdr["RoleMasterId"]);
                    objRoleMasterDAL.Role = Convert.ToString(SqlRdr["Role"]);
                    lstRoleMasterDAL.Add(objRoleMasterDAL);
                }
                SqlRdr.Close();
                SqlCon.Close();

                return lstRoleMasterDAL;
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
