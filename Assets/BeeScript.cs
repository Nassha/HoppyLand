using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Data;
using System.IO;


public class BeeScript : MonoBehaviour
{
    public Animator enemyAnimator;
    public AudioSource source;
    public Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        init();
  
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(collider);
            enemyAnimator.SetBool("isDead", true);
            source.Play();
            StartCoroutine(DeathWaiter());
            
        }
    }

    private IEnumerator DeathWaiter()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void init()
    {
        source = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
    }
}
