using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarPointLevel : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;
    //public AudioSource source;
    public Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(collider);
            StartCoroutine(LoadScene());
        }
        else
        {
            collider = GetComponent<Collider2D>();
        }
    }

    void init()
    {
        //source = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
    }

    IEnumerator LoadScene()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
