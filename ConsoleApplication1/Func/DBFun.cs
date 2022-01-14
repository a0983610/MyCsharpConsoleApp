using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    /// <summary>
    /// 使用using(){} [IDisposable]
    /// </summary>
    // "Server=localhost\\SQLEXPRESS;Database=test;Trusted_Connection=True;";
    // "Server=localhost\\SQLEXPRESS;Database=test;User ID=1234;Password=1234";
    class DBFun : IDisposable
    {
        public string sServer;
        public string sDatabase;
        public string sUserID;
        public string sPassword;

        public SqlConnection con = null;
        public SqlCommand cmd = null;

        private bool bDisCon = true;

        public void getAppconfig()
        {
            try
            {
                sServer = System.Configuration.ConfigurationManager.AppSettings["Server"];
                sDatabase = System.Configuration.ConfigurationManager.AppSettings["Database"];
                sUserID = System.Configuration.ConfigurationManager.AppSettings["UserID"];
                sPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 預設App.config
        /// </summary>
        /// <param name="bnull">fales=不開啟連線,只使用其中的Function</param>
        public DBFun(bool bnull = true)
        {
            if (!bnull) return;

            bDisCon = true;
            con = getCon();
            cmd = getCmd();
        }
        /// <summary>
        /// 不會去關閉外來的SqlConnection
        /// </summary>
        /// <param name="con"></param>
        public DBFun(SqlConnection con)
        {
            bDisCon = false;
            this.con = con;
            this.cmd = getCmd(con);
        }
        public DBFun(string sv, string db, string uid, string pw, string str = "")
        {
            sServer = sv;
            sDatabase = db;
            sUserID = uid;
            sPassword = pw;

            bDisCon = true;
            con = getCon(str);
            cmd = getCmd();
        }


        public string getConStr()
        {
            if (sServer == null) getAppconfig();
            return string.Format("Server={0};Database={1};User ID={2};Password={3}", sServer, sDatabase, sUserID, sPassword);
        }
        public SqlConnection getCon(string str = "")
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = getConStr() + str;
                con.Open();
                return con;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public SqlCommand getCmd(SqlConnection con = null, SqlTransaction tra = null)
        {
            try
            {
                if (con == null)
                {
                    if (this.con == null) this.con = getCon();
                    con = this.con;
                }
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = con.CreateCommand();
                if (tra != null) cmd.Transaction = tra;
                return cmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// SqlDataReader必需要關閉
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public SqlDataReader getDataReader(SqlCommand cmd, string Sql, params SqlParameter[] par)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = Sql;
                
                cmd.Parameters.AddRange(par);

                SqlDataReader DtReader = cmd.ExecuteReader();
                return DtReader;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "發生錯誤的SQL語法:" + Sql);
            }
        }
        /// <summary>
        /// SqlDataReader必需要關閉
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public SqlDataReader getDataReader(string Sql, params SqlParameter[] par)
        {
            try
            {
                return getDataReader(this.cmd, Sql, par);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable FillDataTable(SqlCommand cmd, string Sql, params SqlParameter[] par)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = Sql;
                
                cmd.Parameters.AddRange(par);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "發生錯誤的SQL語法:" + Sql);
            }
        }
        public DataTable FillDataTable(string Sql, params SqlParameter[] par)
        {
            try
            {
                return FillDataTable(this.cmd, Sql, par);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int ExecuteNonQuery(SqlCommand cmd, string Sql, params SqlParameter[] par)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = Sql;
                
                cmd.Parameters.AddRange(par);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "發生錯誤的SQL語法:" + Sql);
            }
        }
        public int ExecuteNonQuery(string Sql, params SqlParameter[] par)
        {
            try
            {
                return ExecuteNonQuery(this.cmd, Sql, par);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public object ExecuteScalar(SqlCommand cmd, string Sql, params SqlParameter[] par)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = Sql;
                
                cmd.Parameters.AddRange(par);

                object obj = cmd.ExecuteScalar();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "發生錯誤的SQL語法:" + Sql);
            }
        }
        public object ExecuteScalar(string Sql, params SqlParameter[] par)
        {
            try
            {
                return ExecuteScalar(this.cmd, Sql, par);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool isFound(SqlCommand cmd, string Sql, params SqlParameter[] par)
        {
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = Sql;
                
                cmd.Parameters.AddRange(par);

                object obj = cmd.ExecuteScalar();
                if (obj != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString() + "發生錯誤的SQL語法:" + Sql);
            }
        }
        public bool isFound(string Sql, params SqlParameter[] par)
        {
            try
            {
                return isFound(this.cmd, Sql, par);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public SqlParameter Para(string Key, object Val)
        {
            try
            {
                return new SqlParameter(Key, Val == null ? DBNull.Value : Val);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string Para(string Key)
        {
            if (Key == null) throw new Exception("[SqlParameter]Key=null");
            return "@" + Key;
            //SQL Server +'@'
            //Oracle +':'
        }

        void IDisposable.Dispose()
        {
            try
            {
                if (bDisCon)
                {
                    if (con != null)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }

                if (cmd != null)
                {
                    cmd.Dispose();
                }
            }
            catch (Exception)
            {
                throw;
            }
            //throw new NotImplementedException();
        }
    }
}
