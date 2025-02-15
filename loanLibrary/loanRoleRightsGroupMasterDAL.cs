using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanRoleRightsGroupMaster
    /// </summary>
    public class loanRoleRightsGroupMasterDAL
    {
        #region Properties
        public int RoleRightsGroupMasterId { get; set; }
        public string RoleRightsGroup { get; set; }
        public int? SortOrder { get; set; }
        #endregion

        #region Class Methods
        private List<loanRoleRightsGroupMasterDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanRoleRightsGroupMasterDAL> lstRoleRightsGroupMasterDAL = new List<loanRoleRightsGroupMasterDAL>();
            loanRoleRightsGroupMasterDAL objRoleRightsGroupMasterDAL = null;
            while (sqlRdr.Read())
            {
                objRoleRightsGroupMasterDAL = new loanRoleRightsGroupMasterDAL();
                objRoleRightsGroupMasterDAL.RoleRightsGroupMasterId = Convert.ToInt32(sqlRdr["RoleRightsGroupMasterId"]);
                objRoleRightsGroupMasterDAL.RoleRightsGroup = Convert.ToString(sqlRdr["RoleRightsGroup"]);
                if (sqlRdr["SortOrder"] != DBNull.Value)
                {
                    objRoleRightsGroupMasterDAL.SortOrder = Convert.ToInt32(sqlRdr["SortOrder"]);
                }
                lstRoleRightsGroupMasterDAL.Add(objRoleRightsGroupMasterDAL);
            }
            return lstRoleRightsGroupMasterDAL;
        }
        #endregion

        #region SelectAll
        public List<loanRoleRightsGroupMasterDAL> SelectAllRoleRightsGroupMaster()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanRoleRightsGroupMaster_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;


                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanRoleRightsGroupMasterDAL> lstRoleRightsGroupMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstRoleRightsGroupMasterDAL;
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
