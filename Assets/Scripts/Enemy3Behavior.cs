using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Behavior : MonoBehaviour
{
    private Transform player;
    public float moveSpeed = 2;
    public GameObject bullet;
    private float nextFire ;
    public float fireRate = 0.2f;
    public int beamCount = 10;
    private bool firing = false;
    public float bulletSpeed = 5;
    public float beamDensity = 15;
    public int ringSize = 8;
    public AudioSource source;
    // Update is called once per frame
    private void Start()
    {
        source = GetComponent<AudioSource>();
        nextFire = Time.time +Random.Range(1f, 3f);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    
    void Update()
    {
        if (!firing && !transform.GetComponent<EnemyInterface>().stunned)
            transform.position += (player.position - transform.position).normalized * moveSpeed * Time.deltaTime;
     
        if (Time.time > nextFire && !transform.GetComponent<EnemyInterface>().stunned)
        {
            nextFire = Time.time + (1f / fireRate);
            //StartCoroutine(Laser());
            RingFire();
            GetComponent<Animator>().SetBool("firings", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("firings", false);
        }
       
        //if(Time.time > nextFire)


    }

    public void RingFire()
    {
        source.Play();
        for (int i = 0; i < ringSize; i++)
        {
            float angle = (i * 2 * Mathf.PI) / ringSize;
            GameObject blt;
            blt = Instantiate(bullet, transform.position, transform.rotation);
            blt.GetComponent<Rigidbody2D>().velocity = new Vector3(bulletSpeed*Mathf.Cos(angle), bulletSpeed * Mathf.Sin(angle), 0 );
        }
    }
}
