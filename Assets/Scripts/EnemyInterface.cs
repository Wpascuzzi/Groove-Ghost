using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterface : MonoBehaviour
{
    public int dangerBudget = 1;
    public int health = 10;
    public bool stunned = false;
    public float stunnedTime = 3f;
    public GameObject canvas;
    public GameObject score;
    public GameObject deathSound;


    private void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
       foreach(Transform child in canvas.transform)
        {
            if(child.tag == "Score")
            {
                score = child.gameObject;
            }
        }
        deathSound = GameObject.Find("deathSound").gameObject;
    }


    private void Update()
    {

        
        if (stunned)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            GetComponent<Animator>().SetBool("stunned", true);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            GetComponent<Animator>().SetBool("stunned", false);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == 12)
        {
            deathSound.GetComponent<AudioSource>().Play();
            score.GetComponent<ScoreKeeper>().score += (dangerBudget * 10);
            Destroy(gameObject);
        }
    }
    public IEnumerator Unstun()
    {
        yield return new WaitForSeconds(stunnedTime);
        stunned = false;
        // GetComponent<CircleCollider2D>().enabled = true;
        GetComponent<Rigidbody2D>().mass /= 10;
    }
}


