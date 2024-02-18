using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanContractWitnessTran
    /// </summary>
    public class loanContractWitnessTranDAL
    {
        #region Properties
        public int ContractWitnessTranId { get; set; }
        public int linktoContractMasterId { get; set; }
        public int linktoWitnessMasterId { get; set; }

        /// Extra
        #endregion

        #region Insert
        public loanRecordStatus InsertContractWitnessTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractWitnessTran_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@ContractWitnessTranId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoContractMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                }
                if (this.linktoWitnessMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoWitnessMasterId", SqlDbType.Int).Value = this.linktoWitnessMasterId;
                }
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.ContractWitnessTranId = Convert.ToInt32(SqlCmd.Parameters["@ContractWitnessTranId"].Value);
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
        public loanRecordStatus DeleteContractWitnessTran()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanContractWitnessTran_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@linktoContractMasterId", SqlDbType.Int).Value = this.linktoContractMasterId;
                if (this.linktoWitnessMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoWitnessMasterId", SqlDbType.Int).Value = this.linktoWitnessMasterId;
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
