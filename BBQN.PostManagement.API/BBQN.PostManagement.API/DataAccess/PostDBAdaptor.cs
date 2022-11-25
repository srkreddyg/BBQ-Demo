using BBQN.DBFactory;
using BBQN.PostManagement.API.Common;
using BBQN.PostManagement.API.Models;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BBQN.PostManagement.API.DataAccess
{
    public class PostDBAdaptor : IPostDBAdaptor
    {

        private readonly IDBFactory _dbFactory;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostDBAdaptor(IDBFactory dbFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dbFactory = dbFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Public Method
        public async Task<int> CreatePost(SocialPost post)
        {
            var socialPostJson = JsonSerializer.Serialize(post);
            int postID = 0;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 500, "@SocialPost", ParameterDirection.Input,socialPostJson),
                  _dbFactory.CreateParameter(DbType.Int32, 1, "@PostIDVal", ParameterDirection.Output, 0)
            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.CreatePost;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    postID = Convert.ToInt32(param[1].Value);
                }
                con.Close();

            }
            return await Task.FromResult(postID);
        }

        /// <summary>
        /// To Update Post
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePost(SocialPost post)
        {
            var socialPostJson = JsonSerializer.Serialize(post);
            bool isPostUpdated = false;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.String, 50, "@GroupUsers", ParameterDirection.Input,socialPostJson),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.UpdatePost;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    isPostUpdated = Convert.ToBoolean(param[1].Value);
                }

            }
            return await Task.FromResult(isPostUpdated);
        }

        /// <summary>
        /// TO Delete a Post
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public async Task<bool> DeletePost(int postId)
        {
            int id = Convert.ToInt32(_httpContextAccessor.HttpContext?.Items["Authenticated"]);
            bool isPostDeleted = false;
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@PostIDValue", ParameterDirection.Input,postId),
                      _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),
            };

            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.DeletePost;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    isPostDeleted = Convert.ToBoolean(param[1].Value);
                }
                con.Close();
            }
            return await Task.FromResult(isPostDeleted);
        }
        /// <summary>
        /// Return all Posts where the user is part
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<List<SocialPost>> GetAllPosts(int UserId)
        {
            IDbDataParameter[] param = new[]
            {
                  _dbFactory.CreateParameter(DbType.Int32, 50, "@UserIdValue", ParameterDirection.Input,UserId),
            };
            Task<List<SocialPost>> posts = _dbFactory.Executeprocedure<SocialPost>(this._configuration.GetConnectionString("dbString"), Constants.GetPosts, param);
            return await posts;
        }

        /// <summary>
        /// Return all Posts Reported
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<List<SocialPost>> GetAllReportedPosts()
        {
            IDbDataParameter[] param = new IDbDataParameter[0];
            Task<List<SocialPost>> posts = _dbFactory.Executeprocedure<SocialPost>(this._configuration.GetConnectionString("dbString"), Constants.GetReportedPosts, param);
            return await posts;
        }
        /// <summary>
        /// Commenting Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task<bool> CommentPost(int postID, int userId, string comment)
        {
           bool isPostUpdated = false;
            IDbDataParameter[] param = new[]
            {
                         _dbFactory.CreateParameter(DbType.Int32, 50, "@SocialPostID", ParameterDirection.Input,postID),
                     _dbFactory.CreateParameter(DbType.Int32, 50, "@UserID", ParameterDirection.Input,userId),
                      _dbFactory.CreateParameter(DbType.String, 5000, "@Comment", ParameterDirection.Input,comment),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.CommentPost;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[1].Value != DBNull.Value && param[1].Value != null)
                {
                    isPostUpdated = Convert.ToBoolean(param[1].Value);
                }

            }
            return await Task.FromResult(isPostUpdated);
        }
        /// <summary>
        /// Like Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> LikePost(int postID, int userId)
        {
             bool isPostUpdated = false;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.Int32, 50, "@SocialPostID", ParameterDirection.Input,postID),
                     _dbFactory.CreateParameter(DbType.Int32, 50, "@UserID", ParameterDirection.Input,userId),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.LikePost;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[2].Value != DBNull.Value && param[2].Value != null)
                {
                    isPostUpdated = Convert.ToBoolean(param[2].Value);
                }

            }
            return await Task.FromResult(isPostUpdated);
        }
        /// <summary>
        /// Report Post
        /// </summary>
        /// <param name="postID"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> ReportPost(int postID, int userId)
        {
            bool isPostReported = false;
            IDbDataParameter[] param = new[]
            {
                 _dbFactory.CreateParameter(DbType.Int32, 50, "@SocialPostID", ParameterDirection.Input,postID),
                     _dbFactory.CreateParameter(DbType.Int32, 50, "@UserID", ParameterDirection.Input,userId),
                  _dbFactory.CreateParameter(DbType.Boolean, 1, "@flag", ParameterDirection.Output, 0),

            };
            using (IDbConnection con = _dbFactory.GetDbConnection(this._configuration.GetConnectionString("dbString")))
            {
                IDbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = Constants.ReportPost;
                cmd.CommandTimeout = 0;
                AddParameter(cmd, param);
                con.Open();
                IDbDataAdapter adapter = _dbFactory.GetDbDataAdapter();
                adapter.SelectCommand = cmd;
                cmd.ExecuteNonQuery();
                if (param[2].Value != DBNull.Value && param[2].Value != null)
                {
                    isPostReported = Convert.ToBoolean(param[2].Value);
                }

            }
            return await Task.FromResult(isPostReported);
        }
        #endregion

        #region Private method

        private void AddParameter(IDbCommand cmd, IDbDataParameter[] param)
        {
            foreach (var p in param)
            {
                cmd.Parameters.Add(p);
            }
        }

   

        #endregion
    }
}
