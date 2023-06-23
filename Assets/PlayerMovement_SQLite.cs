using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class PlayerMovement_SQLite : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    public Text text1;
    public Text text2;
    public IDbConnection dbcon;
    public IDbCommand dbcmd;

    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    public AudioSource source;
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
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void init()
    {
        source = GetComponent<AudioSource>();
        text1 = GameObject.FindWithTag("myText").GetComponent<Text>();
        text2 = GameObject.FindWithTag("EnemyText").GetComponent<Text>();
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Items"))
        {
            // Insert values in table
            IDbCommand cmnd = dbcon.CreateCommand();
            cmnd.CommandText = "INSERT INTO tbl (Items) VALUES (1)";
            cmnd.ExecuteNonQuery();

            source.Play();
            Destroy(other.gameObject);

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

        else if(other.gameObject.CompareTag("Enemy"))
        {
            // Insert values in table
            IDbCommand beeCmnd = dbcon.CreateCommand();
            beeCmnd.CommandText = "INSERT INTO tbl (Enemy) VALUES (1)";
            beeCmnd.ExecuteNonQuery();

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