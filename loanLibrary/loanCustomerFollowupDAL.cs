using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading;

namespace loanLibrary
{
    /// <summary>
    /// Class for loanContractMaster
    /// </summary>
    public class loanCustomerFollowupDAL
    {
        #region Properties
        public int CustomerFollowupId { get; set; }
        public int linktoCompanyMasterId { get; set; }
        public int linktoCustomerMasterId { get; set; } 
        public string Notes { get; set; } 
        public DateTime UpdateDateTime { get; set; }
        public string SessionId { get; set; }

        public bool? IsVerified { get; set; }
        public DateTime? VerifiedDateTime { get; set; }
        public int? linktoUserMasterId { get; set; }
  
        

        /// Extra
        public string Customer { get; set; }
        public string CustomerIdNo { get; set; }
        public string ContractStatus { get; set; }
        public bool? CustomerIsRedFlag { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal IncomeAmount { get; set; }
        public DateTime? LastPaidDate { get; set; }
        public decimal? LastPaidAmount { get; set; }
        public string Bank { get; set; }
        public decimal DueAmount { get; set; }
        public string Last3Installments { get; set; }
        public string CustomerLinks { get; set; }
        public string VerifiedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public string GuarantorName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountNumber2 { get; set; }
        public string BankAccountNumber3 { get; set; }
        public string BankAccountNumber4 { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public int? DueInstallments { get; set; }
        public string CustomerAddress { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Mobile3 { get; set; }

        public string Username { get; set; }

        public int ContractsCount { get; set; }
        public int TotalInstallments { get; set; }
        public int RemainingInstallments { get; set; }
        public decimal TotalAmount { get; set; } 
        public decimal TotalRemains { get; set; }

        #endregion

        #region Class Methods
        private bool SetClassPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            if (sqlRdr.Read())
            {
                this.CustomerFollowupId = Convert.ToInt32(sqlRdr["CustomerFollowupId"]);
                this.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                this.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);
     
                this.Notes = Convert.ToString(sqlRdr["Notes"]); 
                this.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                this.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                this.Customer = Convert.ToString(sqlRdr["Customer"]);
                this.CustomerIdNo = Convert.ToString(sqlRdr["CustomerIdNo"]);
                this.Username = Convert.ToString(sqlRdr["Username"]);




                return true;
            }
            return false;
        }

        private List<loanCustomerFollowupDAL> SetListPropertiesFromSqlDataReader(SqlDataReader sqlRdr)
        {
            List<loanCustomerFollowupDAL> lstCustomerFollowup = new List<loanCustomerFollowupDAL>();
            loanCustomerFollowupDAL objCustomerFollowup = null;
            while (sqlRdr.Read())
            {
                objCustomerFollowup = new loanCustomerFollowupDAL();
                objCustomerFollowup.CustomerFollowupId = Convert.ToInt32(sqlRdr["CustomerFollowupId"]);
                objCustomerFollowup.linktoCompanyMasterId = Convert.ToInt32(sqlRdr["linktoCompanyMasterId"]);
                objCustomerFollowup.linktoCustomerMasterId = Convert.ToInt32(sqlRdr["linktoCustomerMasterId"]);

                objCustomerFollowup.Notes = Convert.ToString(sqlRdr["Notes"]); 
                if (sqlRdr["IsVerified"] != DBNull.Value)
                {
                    objCustomerFollowup.IsVerified = Convert.ToBoolean(sqlRdr["IsVerified"]);
                }
                if (sqlRdr["VerifiedDateTime"] != DBNull.Value)
                {
                    objCustomerFollowup.VerifiedDateTime = Convert.ToDateTime(sqlRdr["VerifiedDateTime"]);
                }
                

                if (sqlRdr["linktoUserMasterId"] != DBNull.Value)
                {
                    objCustomerFollowup.linktoUserMasterId = Convert.ToInt32(sqlRdr["linktoUserMasterId"]);
                }
                objCustomerFollowup.UpdateDateTime = Convert.ToDateTime(sqlRdr["UpdateDateTime"]);
                objCustomerFollowup.SessionId = Convert.ToString(sqlRdr["SessionId"]);

                /// Extra
                /// 
                objCustomerFollowup.Customer = Convert.ToString(sqlRdr["CustomerName"]);
                objCustomerFollowup.CustomerIdNo = Convert.ToString(sqlRdr["CustomerIdNo"]);
                if (sqlRdr["Mobile1"] != DBNull.Value)
                {
                    objCustomerFollowup.Mobile1 = Convert.ToString(sqlRdr["Mobile1"]);
                }

                if (sqlRdr["Mobile2"] != DBNull.Value)
                {
                    objCustomerFollowup.Mobile2 = Convert.ToString(sqlRdr["Mobile2"]);
                }

                if (sqlRdr["Mobile3"] != DBNull.Value)
                {
                    objCustomerFollowup.Mobile3 = Convert.ToString(sqlRdr["Mobile3"]);
                }

                if (sqlRdr["Username"] != DBNull.Value)
                {
                   
                    objCustomerFollowup.Username = Convert.ToString(sqlRdr["Username"]);

                }



                if (sqlRdr["ContractsCount"] != DBNull.Value)
                {

                    objCustomerFollowup.ContractsCount = Convert.ToInt32(sqlRdr["ContractsCount"]);

                }

                if (sqlRdr["TotalInstallments"] != DBNull.Value)
                {

                    objCustomerFollowup.TotalInstallments = Convert.ToInt32(sqlRdr["TotalInstallments"]);

                }


                if (sqlRdr["RemainingInstallments"] != DBNull.Value)
                {

                    objCustomerFollowup.RemainingInstallments = Convert.ToInt32(sqlRdr["RemainingInstallments"]);

                }

                if (sqlRdr["TotalAmount"] != DBNull.Value)
                {

                    objCustomerFollowup.TotalAmount = Convert.ToDecimal(sqlRdr["TotalAmount"]);

                }

                if (sqlRdr["TotalPaid"] != DBNull.Value)
                {

                    objCustomerFollowup.TotalPaid = Convert.ToDecimal(sqlRdr["TotalPaid"]);

                }

                if (sqlRdr["TotalRemains"] != DBNull.Value)
                {

                    objCustomerFollowup.TotalRemains = Convert.ToDecimal(sqlRdr["TotalRemains"]);

                }




                lstCustomerFollowup.Add(objCustomerFollowup);
            }
            return lstCustomerFollowup;
        }
        #endregion

     
        #region SelectAll
        public List<loanCustomerFollowupDAL> SelectAllCustomerFollowupsPageWise(int startRowIndex, int pageSize, out int totalRecords,   bool withDueAmount = false, int CustomerPaymentContractMasterId = 0, DateTime? installmentDateFrom = null, DateTime? installmentDateTo = null, string orderBy = null, string orderDir = null)
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowup_SelectAll", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.CustomerFollowupId > 0)
                {
                    SqlCmd.Parameters.Add("@CustomerFollowupId", SqlDbType.Int).Value = this.CustomerFollowupId;
                }
                if (this.linktoCustomerMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                }

                if (this.linktoUserMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                }
                //SqlCmd.Parameters.Add("@Customer", SqlDbType.NVarChar).Value = this.Customer;
                //SqlCmd.Parameters.Add("@BankAccountNumber", SqlDbType.NVarChar).Value = this.BankAccountNumber;



                //if ((this.linktoCompanyMasterId > 0))
                //{
                //    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                //}



                if (orderBy != null)
                { 
                    SqlCmd.Parameters.Add("@OrderBy", SqlDbType.VarChar).Value = orderBy;
                }
                if (orderDir != null)
                {
                    SqlCmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = orderDir;
                }

                SqlCmd.Parameters.Add("@StartRowIndex", SqlDbType.Int).Value = startRowIndex;
                SqlCmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = pageSize;
                SqlCmd.Parameters.Add("@TotalRowCount", SqlDbType.Int).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlRdr = SqlCmd.ExecuteReader();
                List<loanCustomerFollowupDAL> lstContractMasterDAL = SetListPropertiesFromSqlDataReader(SqlRdr);
                SqlRdr.Close();
                SqlCon.Close();

                totalRecords = (int)SqlCmd.Parameters["@TotalRowCount"].Value;
                return lstContractMasterDAL;
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

        #region  Notes
        public loanRecordStatus InsertCustomerfollowupNotes()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand(" loanCustomerFollowupNote_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

             
                SqlCmd.Parameters.Add("@linktoCustomerFollowupId", SqlDbType.Int).Value = this.CustomerFollowupId;
                SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes; 
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

       

        public loanRecordStatus UpdateCustomerfollowupNotes()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowupNote_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;


                SqlCmd.Parameters.Add("@CustomerFollowupId", SqlDbType.Int).Value = this.CustomerFollowupId;
                SqlCmd.Parameters.Add("@Notes", SqlDbType.NVarChar).Value = this.Notes;
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

        #region Insert
        public loanRecordStatus InsertCustomerfollowup()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowup_Insert", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerFollowupId", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;

                SqlCmd.Parameters.Add("@UpdateDateTime", SqlDbType.DateTime).Value = this.UpdateDateTime;
                SqlCmd.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = this.SessionId;
                SqlCmd.Parameters.Add("@Status", SqlDbType.SmallInt).Direction = ParameterDirection.Output;

                SqlCon.Open();
                SqlCmd.ExecuteNonQuery();
                SqlCon.Close();

                this.CustomerFollowupId = Convert.ToInt32(SqlCmd.Parameters["@CustomerFollowupId"].Value);
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
        public loanRecordStatus UpdateCustomerfollowup()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowup_Update", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlCmd.Parameters.Add("@CustomerFollowupId", SqlDbType.Int).Value = this.CustomerFollowupId;
                if (this.linktoCompanyMasterId > 0)
                {
                    SqlCmd.Parameters.Add("@linktoCompanyMasterId", SqlDbType.Int).Value = this.linktoCompanyMasterId;
                }
                SqlCmd.Parameters.Add("@linktoCustomerMasterId", SqlDbType.Int).Value = this.linktoCustomerMasterId;
                SqlCmd.Parameters.Add("@linktoUserMasterId", SqlDbType.Int).Value = this.linktoUserMasterId;

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
         
        #region Delete Record
        public loanRecordStatus DeleteCustomerfollowup()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowup_Delete", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;


                SqlCmd.Parameters.Add("@CustomerFollowupId", SqlDbType.Int).Value = this.CustomerFollowupId;  
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


        public bool SelectFollowupCustomer()
        {
            SqlConnection SqlCon = null;
            SqlCommand SqlCmd = null;
            SqlDataReader SqlRdr = null;
            try
            {
                SqlCon = loanObjectFactoryDAL.CreateConnection();
                SqlCmd = new SqlCommand("loanCustomerFollowup_Select", SqlCon);
                SqlCmd.CommandType = CommandType.StoredProcedure;

                if (this.CustomerFollowupId > 0)
                {
                    SqlCmd.Parameters.Add("@CustomerFollowupId", SqlDbType.Int).Value = this.CustomerFollowupId;
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


    }
}
