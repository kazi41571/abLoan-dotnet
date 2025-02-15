using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanRoleRightsMaster
    /// </summary>
    public class loanRoleRightsMasterDAL
    {
        #region Properties
        public int RoleRightsMasterId { get; set; }
        public int linktoRoleRightsGroupMasterId { get; set; }
        public string RoleRight { get; set; }
        public string PageName { get; set; }
        public bool IsAvailableViewList { get; set; }
        public bool IsAvailableViewRecord { get; set; }
        public bool IsAvailableAddRecord { get; set; }
        public bool IsAvailableEditRecord { get; set; }
        public bool IsAvailableDeleteRecord { get; set; }
        public int? SortOrder { get; set; }
        #endregion

        #region Class Methods
        private List<loanRoleRightsMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanRoleRightsMasterDAL> lstRoleRightsMasterDAL = new List<loanRoleRightsMasterDAL>();
            loanRoleRightsMasterDAL objRoleRightsMasterDAL = null;
            while (sqlRdr.Read())
            {
                objRoleRightsMasterDAL = new loanRoleRightsMasterDAL();
                objRoleRightsMasterDAL.RoleRightsMasterId = Convert.ToInt32(sqlRdr["RoleRightsMasterId"]);
                objRoleRightsMasterDAL.linktoRoleRightsGroupMasterId = Convert.ToInt32(sqlRdr["linktoRoleRightsGroupMasterId"]);
                objRoleRightsMasterDAL.RoleRight = Convert.ToString(sqlRdr["RoleRight"]);
                objRoleRightsMasterDAL.PageName = Convert.ToString(sqlRdr["PageName"]);
                objRoleRightsMasterDAL.IsAvailableViewList = Convert.ToBoolean(sqlRdr["IsAvailableViewList"]);
                objRoleRightsMasterDAL.IsAvailableViewRecord = Convert.ToBoolean(sqlRdr["IsAvailableViewRecord"]);
                objRoleRightsMasterDAL.IsAvailableAddRecord = Convert.ToBoolean(sqlRdr["IsAvailableAddRecord"]);
                objRoleRightsMasterDAL.IsAvailableEditRecord = Convert.ToBoolean(sqlRdr["IsAvailableEditRecord"]);
                objRoleRightsMasterDAL.IsAvailableDeleteRecord = Convert.ToBoolean(sqlRdr["IsAvailableDeleteRecord"]);
                if (sqlRdr["SortOrder"] != DBNull.Value)
                {
                    objRoleRightsMasterDAL.SortOrder = Convert.ToInt32(sqlRdr["SortOrder"]);
                }
                lstRoleRightsMasterDAL.Add(objRoleRightsMasterDAL);
            }
            return lstRoleRightsMasterDAL;
        }
        #endregion

        #region SelectAll
        public List<loanRoleRightsMasterDAL> SelectAllRoleRightsMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleRightsMaster_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoRoleRightsGroupMasterId", SqlDbType.Int).Value = this.linktoRoleRightsGroupMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanRoleRightsMasterDAL> lstRoleRightsMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstRoleRightsMasterDAL;
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
