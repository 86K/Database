using UnityEngine;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace UnityFramework.Database.SQLite
{
    public class SQLiteManager
    {
        SqliteConnection dbConnection;
        SqliteCommand dbCommand;
        SqliteDataReader dataReader;
        
        public SQLiteManager(string connectionString)
        {
            try
            {
                dbConnection = new SqliteConnection(connectionString);
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="queryString">SQL命令字符串</param>
        public SqliteDataReader ExecuteQuery(string queryString)
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = queryString;
            dataReader = dbCommand.ExecuteReader();
            return dataReader;
        }
        
        public void Close()
        {
            if (dbCommand != null)
            {
                dbCommand.Cancel();
            }
            dbCommand = null;
            
            if (dataReader != null)
            {
                dataReader.Close();
            }
            dataReader = null;
            
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            dbConnection = null;
        }
        
        public SqliteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            string queryString = "CREATE TABLE " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }

            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }

        public SqliteDataReader InsertValues(string tableName, string[] values)
        {
            int fieldCount = ReadFullTable(tableName).FieldCount;
            if (values.Length != fieldCount)
            {
                throw new SqliteException("values.Length!=fieldCount");
            }

            string queryString = "INSERT INTO " + tableName + " VALUES (" + values[0];
            for (int i = 1; i < values.Length; i++)
            {
                queryString += ", " + values[i];
            }

            queryString += " )";
            return ExecuteQuery(queryString);
        }
        
        public SqliteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string operation, string value)
        {
            if (colNames.Length != colValues.Length)
            {
                throw new SqliteException("colNames.Length!=colValues.Length");
            }

            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + colValues[i];
            }

            queryString += " WHERE " + key + operation + value;
            return ExecuteQuery(queryString);
        }
        
        public SqliteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SqliteException(
                    "colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += "OR " + colNames[i] + operations[0] + colValues[i];
            }

            return ExecuteQuery(queryString);
        }
        
        public SqliteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] operations, string[] colValues)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SqliteException(
                    "colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + colValues[0];
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += " AND " + colNames[i] + operations[i] + colValues[i];
            }

            return ExecuteQuery(queryString);
        }
        
        public SqliteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
        {
            string queryString = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                queryString += ", " + items[i];
            }

            queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
            for (int i = 0; i < colNames.Length; i++)
            {
                queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
            }

            return ExecuteQuery(queryString);
        }
        
        public SqliteDataReader ReadFullTable(string tableName)
        {
            string queryString = "SELECT * FROM " + tableName;
            return ExecuteQuery(queryString);
        }
        
        public List<string> GetAllTableNames()
        {
            string queryString = "select name from sqlite_master where type='table'";
            var a = ExecuteQuery(queryString);
            List<string> ownTableNames = null;
            while (a.Read())
            {
                if (ownTableNames == null)
                {
                    ownTableNames = new List<string>();
                }

                ownTableNames.Add(a.GetString(0));
            }

            return ownTableNames;
        }
        
        public bool IsTableExist(string tableName)
        {
            string queryString = "select name from sqlite_master where type='table'";
            var a = ExecuteQuery(queryString);
            while (a.Read())
            {
                if (a.GetString(0) == tableName)
                    return true;
            }

            return false;
        }
    }
}