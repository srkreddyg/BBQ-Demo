using MySql.Data.MySqlClient;
using System.Data;

namespace BBQN.DBFactory
{
   public  interface IDBFactory
    {
        /// <summary>
        /// To Get DB Connection
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        IDbConnection GetDbConnection(string ConnectionString);
      /// <summary>
      /// To Get Db Adapter
      /// </summary>
      /// <returns></returns>
        IDbDataAdapter GetDbDataAdapter();

        /// <summary>
        /// Create parameter
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        IDbDataParameter CreateParameter(DbType parameterType, int size, string name, ParameterDirection direction, object objValue);
       
        /// <summary>
        /// Executing SP for getting DataSet as Result
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="dbDataParameters"></param>
        /// <returns></returns>
          DataSet Executeprocedure(string connectionString, string procedureName, IDbDataParameter[] dbDataParameters);
        /// <summary>
        /// For Executing Stored Procedure and Getting Result in Type Required
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="dbDataParameters"></param>
        /// <returns></returns>
        Task<List<T>> Executeprocedure<T>(string connectionString, string procedureName, IDbDataParameter[] dbDataParameters);

    }
}
