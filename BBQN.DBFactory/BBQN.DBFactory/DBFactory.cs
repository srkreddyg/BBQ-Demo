using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace BBQN.DBFactory
{
  /// <summary>
  /// Concrete Implementation for Getting Connection,Adaptor, Stored Procedure Execution
  /// </summary>
    public class DBFactory : IDBFactory
    {
        /// <summary>
        /// Get Concrete Connection 
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <returns></returns>
        public IDbConnection GetDbConnection(string ConnectionString)
        {
           return  new MySqlConnection(ConnectionString);
        }
        /// <summary>
        /// Get Concrete Adaptor
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetDbDataAdapter()
        {
            return new MySqlDataAdapter();
        }
        /// <summary>
        /// Adding Parameter
        /// </summary>
        /// <param name="parameterType"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="direction"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public  IDbDataParameter CreateParameter(DbType parameterType, int size, string name, ParameterDirection direction, object objValue)
        {
            return new MySqlParameter
            {
                DbType = parameterType,
                Size = size,
                ParameterName = name,
                Direction = direction,
                Value = objValue
            };
        }
        /// <summary>
        /// For Executing Stored Procedure and Getting Result dataset
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="procedureName"></param>
        /// <param name="dbDataParameters"></param>
        /// <returns></returns>
        public DataSet Executeprocedure(string connectionString, string procedureName, IDbDataParameter[] dbDataParameters)
        {
            DataSet ds = new DataSet();
            using (IDbConnection con = GetDbConnection(connectionString))
            {
               IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 0;
                IDbDataParameter[] param = dbDataParameters;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);
                con.Close();
            }
            return ds;
        }
        /// <summary>
        /// Execute procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public  async Task<List<T>> Executeprocedure<T>(string connectionString,string procedureName, IDbDataParameter[] dbDataParameters)
        {
            DataSet ds = new DataSet();
            using (IDbConnection con = GetDbConnection(connectionString))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procedureName;
                cmd.CommandTimeout = 0;
                IDbDataParameter[] param = dbDataParameters;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                await Task.Run(() => adapter.Fill(ds));
                con.Close();
            }
            return ConvertDataTable<T>(ds.Tables[0]);
        }
        #region Private Methods
        /// <summary>
        /// Add parameter
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="param"></param>
        private void AddParameter(IDbCommand cmd, IDbDataParameter[] param)
        {
            foreach (var p in param)
            {
                cmd.Parameters.Add(p);
            }
        }
        /// <summary>
        /// Method for Covering Datatable to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        /// <summary>
        /// Getting Item and Matching it with Prpoperty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == column.ColumnName.ToLower())
                    {
                        if (pro.Name.ToLower().StartsWith("is"))
                        {
                            pro.SetValue(obj, dr[column.ColumnName].ToString() == "0" ? false : true, null);
                        }
                        else
                            try
                            {
                                if (!(dr[column.ColumnName].GetType().Name == "DBNull"))
                                    pro.SetValue(obj, dr[column.ColumnName], null);
                                else
                                    pro.SetValue(obj, null);
                            }
                            catch(Exception ex)
                            {
                                throw;
                            }
                    }
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion
    }
}
