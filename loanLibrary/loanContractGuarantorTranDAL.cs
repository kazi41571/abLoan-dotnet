using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanContractGuarantorTran
    /// </summary>
    public class loanContractGuarantorTranDAL
    {
        #region Properties
        public int ContractGuarantorTranId { get; set; }
        public int linktoContractMasterId { get; set; }
        public int linktoGuarantorMasterId { get; set; }

        /// Extra
        #endregion

        #region Insert
        public loanRecordStatus InsertContractGuarantorTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractGuarantorTran_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractGuarantorTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                }
                if (this.linktoGuarantorMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoGuarantorMasterId", SqlDbType.Int).Value = this.linktoGuarantorMasterId;
                }
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.ContractGuarantorTranId = Convert.ToInt32(SqlCmd.Parameters["@ContractGuarantorTranId"].Value);
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
        public loanRecordStatus DeleteContractGuarantorTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractGuarantorTran_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                if (this.linktoGuarantorMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoGuarantorMasterId", SqlDbType.Int).Value = this.linktoGuarantorMasterId;
                }
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
    }
}
