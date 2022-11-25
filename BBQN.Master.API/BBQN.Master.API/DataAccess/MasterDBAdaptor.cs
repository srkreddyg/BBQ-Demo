using BBQN.DBFactory;
using BBQN.Master.API.Common;
using System.Data;
using BBQN.Master.API.Models;
using System.Reflection;

namespace BBQN.Master.API.DataAccess
{
    public class MasterDBAdaptor : IMasterDBAdaptor
    {
        private readonly IDBFactory _dbFactory;
        private IConfiguration _configuration;

        public MasterDBAdaptor(IDBFactory dBFactory, IConfiguration configuration)
        {
            _dbFactory = dBFactory;
            _configuration = configuration;
        }
        #region db operation functions
        /// <summary>
        /// Fetch all admin records from user table 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Privileges>> GetAllPrivileges()
        {
            IDbDataParameter[] param = new IDbDataParameter[0];
            Task<List<Privileges>> privileges = _dbFactory.Executeprocedure<Privileges>(this._configuration.GetConnectionString("dbString"), Constants.GetAllPrivileges, param);
            return await privileges;
        }
        /// <summary>
        /// Fetch master data from database 
        /// </summary>
        /// <returns></returns>
        public async Task<MasterData> FetchMasterData()
            {
            MasterData masterData = new MasterData();
            IDbDataParameter[] param = new IDbDataParameter[0];
            DataSet md = _dbFactory.Executeprocedure(this._configuration.GetConnectionString("dbString"), Constants.GetMasterData, param);
            if (md.Tables.Count > 0)
            {
                masterData.Company = ConvertDataTable<Company>(md.Tables[0]);
                masterData.Regions = ConvertDataTable<Regions>(md.Tables[1]);
                masterData.Departments  = ConvertDataTable<Departments>(md.Tables[2]);
                masterData.Grade = ConvertDataTable<Grade>(md.Tables[3]);
                masterData.UserRole = ConvertDataTable<UserRole>(md.Tables[4]);
                masterData.EmployeeCategory = ConvertDataTable<EmployeeCategory>(md.Tables[5]);
                masterData.Divisions= ConvertDataTable<Divisions>(md.Tables[6]);            
            
            }
            return  masterData;
        }
        /// <summary>
        /// get Departments data 
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        public async Task<List<SubDepartments>> GetDepartments(int departmentId)
        {
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@DepartmentIDval", ParameterDirection.Input,departmentId),
            };
            Task<List<SubDepartments>> subDepartments = _dbFactory.Executeprocedure<SubDepartments>(this._configuration.GetConnectionString("dbString"), Constants.GetSubDepartmentsMasterData, param);
            return await subDepartments;
        }
        /// <summary>
        /// Get OutLets data 
        /// </summary>
        /// <param name="regionID"></param>
        /// <returns></returns>
        public async Task<List<Outlets>> GetOutlets(int regionID)
        {
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@RegionID", ParameterDirection.Input,regionID),
            };
            Task<List<Outlets>> groups = _dbFactory.Executeprocedure<Outlets>(this._configuration.GetConnectionString("dbString"), Constants.GetOutletsMasterData, param);
            return await groups;
        }
        #endregion
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
                    if (pro.Name.ToUpper() == column.ColumnName.ToUpper())
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
        #endregion
    }
}
