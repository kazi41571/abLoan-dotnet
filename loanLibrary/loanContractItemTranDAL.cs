using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanContractItemTran
    /// </summary>
    public class loanContractItemTranDAL
    {
        #region Properties
        public int ContractItemTranId { get; set; }
        public int linktoContractMasterId { get; set; }
        public int linktoItemMasterId { get; set; }

        /// Extra
        #endregion

        #region Class Methods
        private List<loanContractItemTranDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanContractItemTranDAL> lstContractItemTran = new List<loanContractItemTranDAL>();
            loanContractItemTranDAL objContractItemTran = null;
            while (sqlRdr.Read())
            {
                objContractItemTran = new loanContractItemTranDAL();
                objContractItemTran.ContractItemTranId = Convert.ToInt32(sqlRdr["ContractItemTranId"]);
                objContractItemTran.linktoContractMasterId = Convert.ToInt32(sqlRdr["linktoContractMasterId"]);
                objContractItemTran.linktoItemMasterId = Convert.ToInt32(sqlRdr["linktoItemMasterId"]);

                /// Extra
                lstContractItemTran.Add(objContractItemTran);
            }
            return lstContractItemTran;
        }
        #endregion

        #region Insert
        public loanRecordStatus InsertContractItemTran(List<loanContractItemTranDAL> lstContractItemTranDAL, SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlCommand SqlCmd = null;
            try
            {
                SqlCmd = new SqlCommand("loanContractItemTran_Insert", sqlCon, sqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                loanRecordStatus rs = loanRecordStatus.Success;
                foreach (loanContractItemTranDAL obj in lstContractItemTranDAL)
                {
                    SqlCmd.Parameters.Clear();
                    SqlCmd.Parameters.Add("@ContractItemTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                    if (this.linktoContractMasterId > 0)
                    {
                        SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                    }
                    if (obj.linktoItemMasterId > 0)
                    {
                        SqlCmd.Parameters.Add("@linktoItemMasterId", SqlDbType.Int).Value = obj.linktoItemMasterId;
                    }
                    SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                    SqlCmd.ExecuteNonQuery();

                    this.ContractItemTranId = Convert.ToInt32(SqlCmd.Parameters["@ContractItemTranId"].Value);
                    rs = (loanRecordStatus)(short)SqlCmd.Parameters["@Status"].Value;
                    if (rs != loanRecordStatus.Success)
                    {
                        return rs;
                    }
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
            }
        }
        #endregion

        #region Delete
        public loanRecordStatus DeleteContractItemTran(SqlConnection sqlCon, SqlTransaction sqlTran)
        {
            SqlCommand SqlCmd = null;
            try
            {
                SqlCmd = new SqlCommand("loanContractItemTran_Delete", sqlCon, sqlTran);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
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

        public List<loanContractItemTranDAL> SelectAllContractItemTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractItemTran_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanContractItemTranDAL> lstContractItemTranDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                return lstContractItemTranDAL;
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
