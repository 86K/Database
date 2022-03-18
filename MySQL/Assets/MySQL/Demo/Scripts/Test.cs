using System.Data;
using UnityEngine;
using UnityFramework.Database.MySQL;

public class Test : MonoBehaviour
{
    void Start()
    {
        MySqlManager mysql = MySqlManager.Get();
        //Host address need exist a mysql database.
        mysql.Open("127.0..0.1", "test", "root", "root", "3006");

        mysql.CreateTable("table1", new string[] {"name", "age", "sex"}, new string[] {"text", "int", "text"});
        mysql.CreateAutoIDTable("table2", new string[] {"age", "name", "sex"}, new string[] {"int", "text", "text"});

        mysql.InsertFullRow("table1", new string[] {"赵一", "18", "男"});
        mysql.InsertFullRow("table1", new string[] {"钱二", "19", "男"});

        mysql.InsertPartRow("table2", new string[] {"age", "name", "sex"}, new string[] {"20", "孙三", "男"});
        mysql.InsertFullRow("table2", new string[] {"21", "李四", "男"});
        mysql.InsertFullRow("table2", new string[] {"22", "周五", "男"});
        mysql.InsertFullRow("table2", new string[] {"23", "吴六", "男"});

        mysql.DeleteOneRow("table2", "age", "20");
        mysql.DeleteMultipleRows("table2", "age", new string[] {"21", "22"});
        mysql.DeleteAllRows("table2");

        mysql.UpdateMultipleCols("table1", new string[] {"age", "sex"}, new string[] {"24", "女"}, "name", "赵一");
        mysql.UpdateOneCol("table1", "name", "郑七", "name", "钱二");

        mysql.InsertPartRow("table1", new string[] {"age", "sex"}, new string[] {"25", "男"});
        mysql.InsertFullRow("table1", new string[] {"王八", "28", "男"});
        
        DataSet dataSet = mysql.SelectAllCols("table1");
        DataTable dataTable = dataSet.Tables[0];
        foreach (DataRow row in dataTable.Rows)
        {
            foreach (DataColumn col in dataTable.Columns)
            {
                Debug.Log(row[col]);
            }
        }

        mysql.Close();
    }
}