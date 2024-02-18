using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanRoleRightsTran
    /// </summary>
    public class loanRoleRightsTranDAL
    {
        #region Properties
        public int RoleRightsTranId { get; set; }
        public int linktoRoleRightsMasterId { get; set; }
        public int linktoRoleMasterId { get; set; }
        public bool IsViewList { get; set; }
        public bool IsViewRecord { get; set; }
        public bool IsAddRecord { get; set; }
        public bool IsEditRecord { get; set; }
        public bool IsDeleteRecord { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        /// Extra
        public string PageName { get; set; }
        #endregion

        #region Class Methods
        private List<loanRoleRightsTranDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanRoleRightsTranDAL> lstRoleRightsTranDAL = new List<loanRoleRightsTranDAL>();
            loanRoleRightsTranDAL objRoleRightsTranDAL = null;
            while (sqlRdr.Read())
            {
                objRoleRightsTranDAL = new loanRoleRightsTranDAL();
                objRoleRightsTranDAL.RoleRightsTranId = Convert.ToInt32(sqlRdr["RoleRightsTranId"]);
                objRoleRightsTranDAL.linktoRoleRightsMasterId = Convert.ToInt32(sqlRdr["linktoRoleRightsMasterId"]);
                objRoleRightsTranDAL.linktoRoleMasterId = Convert.ToInt32(sqlRdr["linktoRoleMasterId"]);
                objRoleRightsTranDAL.IsViewList = Convert.ToBoolean(sqlRdr["IsViewList"]);
                objRoleRightsTranDAL.IsViewRecord = Convert.ToBoolean(sqlRdr["IsViewRecord"]);
                objRoleRightsTranDAL.IsAddRecord = Convert.ToBoolean(sqlRdr["IsAddRecord"]);
                objRoleRightsTranDAL.IsEditRecord = Convert.ToBoolean(sqlRdr["IsEditRecord"]);
                objRoleRightsTranDAL.IsDeleteRecord = Convert.ToBoolean(sqlRdr["IsDeleteRecord"]);
                objRoleRightsTranDAL.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objRoleRightsTranDAL.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                objRoleRightsTranDAL.PageName = Convert.ToString(sqlRdr["PageName"]);

                lstRoleRightsTranDAL.Add(objRoleRightsTranDAL);
            }
            return lstRoleRightsTranDAL;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertRoleRightsTran(List<loanRoleRightsTranDAL> lstRoleRightsTranDAL)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlTransaction SqlTran = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCon.Open();
                SqlTran = SqlCon.BeginTransaction();

                loanRoleRightsTranDAL objRoleRightsTranDAL = new loanRoleRightsTranDAL();
                objRoleRightsTranDAL.linktoRoleMasterId = this.linktoRoleMasterId;
                loanRecordStatus rs = objRoleRightsTranDAL.DeleteAllRoleRightsTranByRoleMasterId(SqlCon, SqlTran);
                if (rs != loanRecordStatus.Success)
                {
                    SqlTran.Rollback();
                    SqlCon.Close();
                    return rs;
                }

                SqlCmd = new SqlCommand("loanRoleRightsTran_Insert", SqlCon, SqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                foreach (loanRoleRightsTranDAL obj in lstRoleRightsTranDAL)
                {
                    SqlCmd.Parameters.Clear();
                    SqlCmd.Parameters.Add("@RoleRightsTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    SqlCmd.Parameters.Add("@linktoRoleRightsMasterId", SqlDbType.Int).Value = obj.linktoRoleRightsMasterId;
                    SqlCmd.Parameters.Add("@linktoRoleMasterId", SqlDbType.Int).Value = obj.linktoRoleMasterId;
                    SqlCmd.Parameters.Add("@IsViewList", SqlDbType.Bit).Value = obj.IsViewList;
                    SqlCmd.Parameters.Add("@IsViewRecord", SqlDbType.Bit).Value = obj.IsViewRecord;
                    SqlCmd.Parameters.Add("@IsAddRecord", SqlDbType.Bit).Value = obj.IsAddRecord;
                    SqlCmd.Parameters.Add("@IsEditRecord", SqlDbType.Bit).Value = obj.IsEditRecord;
                    SqlCmd.Parameters.Add("@IsDeleteRecord", SqlDbType.Bit).Value = obj.IsDeleteRecord;
                    SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = obj.UpdateDateTime;
                    SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = obj.SessionId;
                    SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                    SqlCmd.ExecuteNonQuery();

                    this.RoleRightsTranId = Convert.ToInt32(SqlCmd.Parameters["@RoleRightsTranId"].Value);
                    rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                    if (rs != loanRecordStatus.Success)
                    {
                        SqlTran.Rollback();
                        SqlCon.Close();
                        return rs;
                    }
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

        #region DeleteAll
        public loanRecordStatus DeleteAllRoleRightsTranByRoleMasterId(SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlCommand SqlCmd = null;
            try
            {
                SqlCmd = new SqlCommand("loanRoleRightsTran_DeleteAll", sqlCon, sqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoRoleMasterId", SqlDbType.Int).Value = this.linktoRoleMasterId;
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

        #region SelectAll
        public List<loanRoleRightsTranDAL> SelectAllRoleRightsTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleRightsTran_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoRoleMasterId", SqlDbType.Int).Value = this.linktoRoleMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanRoleRightsTranDAL> lstRoleRightsTranDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstRoleRightsTranDAL;
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
