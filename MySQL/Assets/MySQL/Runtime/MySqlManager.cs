using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace UnityFramework.Database.MySQL
{
    public class MySqlManager
    {
        static MySqlManager mySqlManager = null;
        static readonly  object locker = new object();
        MySqlConnection sqlConnecter;

        public static MySqlManager Get()
        {
            lock (locker)
            {
                if (mySqlManager == null)
                {
                    mySqlManager = new MySqlManager();
                }

                return mySqlManager;
            }
        }
        
        MySqlManager(){}

        public void Open(string host, string dbName, string id, string password, string port)
        {
            try
            {
                string str = string.Format("Host={0};Database={1};User ID={2};Password={3};Port={4}", host, dbName, id, password, port);
                sqlConnecter = new MySqlConnection(str);
                sqlConnecter.Open();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        public void Close()
        {
            if (sqlConnecter != null)
            {
                sqlConnecter.Close();
                sqlConnecter.Dispose();
                sqlConnecter = null;
            }
        }
        
        DataSet ExcuteStatements(string str)
        {
            if (sqlConnecter.State == ConnectionState.Open)
            {
                DataSet dataSet = new DataSet();
                try
                {
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(str, sqlConnecter);
                    mySqlDataAdapter.Fill(dataSet);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message.ToString());
                }

                return dataSet;
            }

            return null;
        }
        
        public DataSet CreateTable(string tableName, string[] datas, string[] types)
        {
            if (datas.Length != types.Length)
            {
                throw new Exception("Wrong Input");
            }

            string query = "CREATE TABLE " + tableName + "(" + datas[0] + " " + types[0];
            for (int i = 1; i < datas.Length; i++)
            {
                query += "," + datas[i] + " " + types[i];
            }

            query += ")";
            return ExcuteStatements(query);
        }
        
        public DataSet CreateAutoIDTable(string tableName, string[] datas, string[] types)
        {
            if (datas.Length != types.Length)
            {
                throw new Exception("Wrong Input");
            }

            string query = "CREATE TABLE " + tableName + "(" + datas[0] + " " + types[0] + " NOT NULL AUTO_INCREMENT";
            for (int i = 1; i < datas.Length; i++)
            {
                query += ", " + datas[i] + " " + types[i];
            }

            query += ", PRIMARY KEY (" + datas[0] + ")" + ")";
            return ExcuteStatements(query);
        }
        
        public DataSet InsertPartRow(string tableName, string[] colNames, string[] colValues)
        {
            if (colNames.Length != colValues.Length)
            {
                throw new Exception("Wrong Input");
            }

            string query = "INSERT INTO " + tableName + " (" + colNames[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                query += ", " + colNames[i];
            }

            query += ") VALUES (" + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                query += ", " + "'" + colValues[i] + "'";
            }

            query += ")";
            return ExcuteStatements(query);
        }
        
        public DataSet InsertFullRow(string tableName, string[] colValues)
        {
            string query = "INSERT INTO " + tableName + " VALUES (" + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                query += ", " + "'" + colValues[i] + "'";
            }

            query += ")";
            return ExcuteStatements(query);
        }
        
        public DataSet DeleteOneRow(string tableName, string colName, string colValue)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + colName + " = " + "'" + colValue + "'";
            return ExcuteStatements(query);
        }
        
        public DataSet DeleteMultipleRows(string tableName, string colName, string[] colValues)
        {
            string query = "DELETE FROM " + tableName + " WHERE " + colName + " IN (" + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                query += ", " + colValues[i];
            }

            query += ")";
            return ExcuteStatements(query);
        }
        
        public DataSet DeleteAllRows(string tableName)
        {
            string query = "DELETE FROM " + tableName;
            return ExcuteStatements(query);
        }
        
        public DataSet UpdateOneCol(string tableName, string updateColName, string updateColValue, string selectColName, string selectColValue)
        {
            string query = "UPDATE " + tableName + " SET " + updateColName + " = " + "'" + updateColValue + "'" + " WHERE " +
                           selectColName + " = " + "'" + selectColValue + "'";
            return ExcuteStatements(query);
        }
        
        public DataSet UpdateMultipleCols(string tableName, string[] updateColNames, string[] updateColValues, string selectColName, string selectColValue)
        {
            if (updateColNames.Length != updateColValues.Length)
            {
                throw new Exception("Wrong Input");
            }

            string query = "UPDATE " + tableName + " SET " + updateColNames[0] + " = " + "'" + updateColValues[0] + "'";
            for (int i = 1; i < updateColNames.Length; i++)
            {
                query += ", " + updateColNames[i] + " = " + "'" + updateColValues[i] + "'";
            }

            query += " WHERE " + selectColName + " = " + "'" + selectColValue + "'";
            return ExcuteStatements(query);
        }
        
        public DataSet SelectOneCol(string tableName, string colName)
        {
            string query = "SELECT " + colName + " FROM " + tableName;
            return ExcuteStatements(query);
        }
        
        public DataSet SelectMultipleCols(string tableName, string[] colNames)
        {
            string query = "SELECT " + colNames[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                query += "," + colNames[i];
            }

            query += " FROM " + tableName;
            return ExcuteStatements(query);
        }
        
        public  DataSet SelectAllCols(string tableName)
        {
            string query = "SELECT * FROM " + tableName;
            return ExcuteStatements(query);
        }
    }
}