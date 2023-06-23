using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class WinScreenScores : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public IDbConnection dbcon;
    public IDbCommand dbcmd;

    // Start is called before the first frame update
    void Start()
    {
        init();
        SqliteSetup();
        initialCarrot();
        initialBee();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void init()
    {
        text1 = GameObject.FindWithTag("myText").GetComponent<Text>();
        text2 = GameObject.FindWithTag("EnemyText").GetComponent<Text>();
    }

    void SqliteSetup()
    {

        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "GameDatabase";

        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS tbl (Items INTEGER, Enemy INTEGER)";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();
    }

    void initialCarrot()
    {
        // Read and print all values in table
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT sum(Items) FROM tbl";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("sum of Items: " + reader[0].ToString());
            text1.text = reader[0].ToString();
        }
    }

    void initialBee()
    {
        // Read and print all values in table
        IDbCommand beeCmnd_read = dbcon.CreateCommand();
        IDataReader beeReader;
        string beeQuery = "SELECT sum(Enemy) FROM tbl";
        beeCmnd_read.CommandText = beeQuery;
        beeReader = beeCmnd_read.ExecuteReader();

        while (beeReader.Read())
        {
            Debug.Log("sum of Enemy: " + beeReader[0].ToString());
            text2.text = beeReader[0].ToString();
        }
    }
}
