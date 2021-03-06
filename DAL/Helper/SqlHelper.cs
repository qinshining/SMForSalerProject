using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    class SqlHelper
    {
        private readonly static string connString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        /// <summary>
        /// 执行insert/delete/update操作，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteUpdate(string sql, SqlParameter[] param = null)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行查询，返回单一结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetSingleResult(string sql, SqlParameter[] param = null)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行查询，返回Reader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader GetReader(string sql, SqlParameter[] param = null)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取DataSet结果集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (param != null)
            {
                cmd.Parameters.AddRange(param);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 启用事务执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool UpdateByTran(List<string> sqlList)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.Transaction = conn.BeginTransaction();
                foreach (string sql in sqlList)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                cmd.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction = null;
                }
                conn.Close();
            }
        }
    }
}
