using System;
using System.Data;
using System.Data.SqlClient;

namespace RegistrationSystem.Models
{
    /// <summary>
    /// MSSQL資料庫溝通物件
    /// </summary>
    abstract public class MSDatabaseCommunication
    {
        /// <summary>
        /// 資料庫連接字串
        /// </summary>
        protected string ConnectionString = "";
        /// <summary>
        /// 取得資料庫資料，回傳DataTable
        /// </summary>
        /// <param name="_Command">SQL指令</param>
        /// <param name="_Success">是否執行成功</param>
        /// <returns></returns>
        public DataTable GetDBDataResultDataTable(SqlCommand _Command, out bool _Success)
        {
            DataTable dt = null;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    _Command.Connection = conn;
                    try
                    {
                        dt = new DataTable();
                        da.SelectCommand = _Command;
                        da.Fill(dt);
                        _Success = true;
                    }
                    catch (Exception e)
                    {
                        _Success = false;
                        Console.WriteLine(e);
                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// 取得資料庫資料，回傳DataSet
        /// </summary>
        /// <param name="_Command"></param>
        /// <param name="_Success"></param>
        /// <returns></returns>
        public DataSet GetDBDataResultDataSet(SqlCommand _Command, out bool _Success)
        {
            DataTable dt = GetDBDataResultDataTable(_Command, out _Success);
            DataSet ds = null;
            if (_Success)
            {
                ds = new DataSet();
                ds.Tables.Add(dt);
            }
            return ds;
        }
        /// <summary>
        /// 取得資料庫資料，回傳DataReader
        /// </summary>
        /// <param name="_Connection"></param>
        /// <param name="_Command"></param>
        /// <param name="_Success"></param>
        /// <returns></returns>
        public SqlDataReader GetDBDataResultDataReader(SqlConnection _Connection, SqlCommand _Command, out bool _Success)
        {
            SqlDataReader dr = null;
            _Connection.ConnectionString = ConnectionString;
            _Command.Connection = _Connection;
            try
            {
                if (_Connection.State == ConnectionState.Closed)
                    _Connection.Open();

                dr = _Command.ExecuteReader();
                _Success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("SQL查詢失敗:" + e);
                _Success = false;
            }
            return dr;
        }
        /// <summary>
        /// 取得查詢結果的第一個資料列的第一個資料行的資料
        /// </summary>
        /// <param name="_Command"></param>
        /// <param name="_Success"></param>
        /// <returns></returns>
        public object GetDBDataResult(SqlCommand _Command, out bool _Success)
        {
            object ob = null;

            using (SqlConnection _Connection = new SqlConnection(ConnectionString))
            {
                _Connection.Open();
                _Command.Connection = _Connection;
                try
                {
                    ob = _Command.ExecuteScalar();
                    _Success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    _Success = false;
                }
            }
            return ob;
        }
        /// <summary>
        /// 執行指令回傳受影響筆數
        /// </summary>
        /// <param name="_Command"></param>
        /// <param name="_Success"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(SqlCommand _Command, out bool _Success)
        {
            int re = 0;
            using (SqlConnection _Connection = new SqlConnection(ConnectionString))
            {
                _Connection.Open();
                using (SqlTransaction Trans = _Connection.BeginTransaction())
                {
                    _Command.Transaction = Trans;
                    _Command.Connection = _Connection;
                    try
                    {
                        re = _Command.ExecuteNonQuery();
                        _Success = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        _Success = false;
                    }
                    if (_Success)
                        Trans.Commit();
                    else
                        Trans.Rollback();
                }
            }
            return re;
        }
        /// <summary>
        /// 執行批次複製
        /// </summary>
        /// <param name="dt">屬性TableName要與匯入目標TableName一致，ColumnName亦同</param>
        /// <returns></returns>
        public bool ExecuteSQLBulkCopy(DataTable dt)
        {
            bool Success = false;
            using (SqlConnection _Connection = new SqlConnection(ConnectionString))
            {
                _Connection.Open();
                using (SqlTransaction Trans = _Connection.BeginTransaction())
                {
                    using (SqlBulkCopy SqlBC = new SqlBulkCopy(_Connection, SqlBulkCopyOptions.Default, Trans))
                    {
                        SqlBC.BatchSize = 1000;
                        SqlBC.NotifyAfter = 100;
                        SqlBC.BulkCopyTimeout = 60;
                        SqlBC.DestinationTableName = dt.TableName;
                        DataColumnCollection columns = dt.Columns;
                        foreach (DataColumn column in columns)
                        {
                            SqlBC.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        try
                        {
                            SqlBC.WriteToServer(dt);
                            Success = true;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("批次複製資料失敗:" + e);
                            Success = false;
                        }
                        if (Success)
                            Trans.Commit();
                        else
                            Trans.Rollback();
                    }
                }
            }
            return Success;
        }
    }
    /// <summary>
    /// MSSQL Test30資料庫溝通物件
    /// </summary>
    public class Test30DBCommunication : MSDatabaseCommunication
    {
        /// <summary>
        /// MSSQL Test30資料庫溝通物件
        /// </summary>
        public Test30DBCommunication()
        {
            ConnectionString = "Data Source = .\\SQLEXPRESS; database=Test30; uid=sa; pwd=abcd1234";
        }
    }

    /// <summary>
    /// DatabaseCommunication創建工廠
    /// </summary>
    public class DatabaseCommunicationFactory
    {
        public MSDatabaseCommunication GetDatabaseCommunication(DatabaseEnum _dbe)
        {
            MSDatabaseCommunication DC = null;
            switch (_dbe)
            {
                case DatabaseEnum.Test30:
                    DC = new Test30DBCommunication();
                    break;
                default:
                    break;
            }
            return DC;
        }
    }
    public enum DatabaseEnum
    {
        Test30
    }
}