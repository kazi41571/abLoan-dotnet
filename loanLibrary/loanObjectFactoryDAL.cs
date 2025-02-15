using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loanLibrary
{
    public class loanObjectFactoryDAL
    {
        #region Public Static Methods
        /// <summary>
        /// Create a new Connection object
        /// </summary>
        /// <returns>a new Connection object</returns>
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["loanConnectionString"].ConnectionString);
        }

        ///// <summary>
        ///// Open the Connection if state is open
        ///// </summary>
        ///// <param name="sqlConnection">The connection the Command will use</param>
        //public static void OpenConnection(SqlConnection sqlConnection)
        //{
        //    if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
        //    {
        //        sqlConnection.Open();
        //    }
        //}

        ///// <summary>
        ///// Close the Connection if state is open
        ///// </summary>
        ///// <param name="sqlConnection">The connection the Command will use</param>
        //public static void CloseConnection(SqlConnection sqlConnection)
        //{
        //    if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
        //    {
        //        sqlConnection.Close();
        //    }
        //}

        /// <summary>
        /// Close the Connection if state is open and dispose the Connection object
        /// </summary>
        /// <param name="sqlConnection">The connection the Command will use</param>
        public static void DisposeConnection(SqlConnection sqlConnection)
        {
            if (sqlConnection != null)
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Dispose();
            }
        }

        /// <summary>
        /// Dispose the Transaction object
        /// </summary>
        /// <param name="sqlCommand">The Transaction</param>
        public static void DisposeTransaction(SqlTransaction sqlTransaction)
        {
            if (sqlTransaction != null)
            {
                sqlTransaction.Dispose();
            }
        }

        /// <summary>
        /// Dispose the Command object
        /// </summary>
        /// <param name="sqlCommand">The command</param>
        public static void DisposeCommand(SqlCommand sqlCommand)
        {
            if (sqlCommand != null)
            {
                sqlCommand.Dispose();
            }
        }

        /// <summary>
        /// Close the DataReader if open and dispose the DataReader object
        /// </summary>
        /// <param name="sqlDataReader">SqlDataReader</param>
        public static void DisposeDataReader(SqlDataReader sqlDataReader)
        {
            if (sqlDataReader != null)
            {
                if (sqlDataReader.IsClosed == false)
                {
                    sqlDataReader.Close();
                }
                sqlDataReader.Dispose();
            }
        }
        #endregion
    }
}
