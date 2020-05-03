using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    /// <summary>
    /// 数据库连接
    /// </summary>
    private SqliteConnection SqlConnection;
    /// <summary>
    /// 数据库命令
    /// </summary>
    private SqliteCommand SqlCommand;
    /// <summary>
    /// 数据库读取
    /// </summary>
    private SqliteDataReader SqlDataReader;


    /// <summary>
    /// 建立数据库连接
    /// </summary>

    // Use this for initialization
    void Start () {
        try
        {
            SqlConnection = new SqliteConnection("data source=" + Application.dataPath + "/sqlite4unity.db");
            SqlConnection.Open();
            SqlCommand = SqlConnection.CreateCommand();
            ExecuteReader("select * from Test");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public SqliteDataReader ExecuteReader(string command)
    {
#if UNITY_EDITOR
        Debug.Log("SQL:ExecuteReader " + command);
#endif
        SqlCommand.CommandText = command;
        SqlDataReader = SqlCommand.ExecuteReader();
        return SqlDataReader;
    }

}
