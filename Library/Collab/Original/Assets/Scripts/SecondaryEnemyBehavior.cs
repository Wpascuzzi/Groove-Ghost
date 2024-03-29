﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryEnemyBehavior : MonoBehaviour
{
    private Transform player;
    public float moveSpeed = 2;
    public GameObject bullet;
    private float nextFire = 1;
    public float fireRate = 0.2f;
    public int beamCount = 10;
    private bool firing = false;
    public float bulletSpeed = 5;
    public float beamDensity = 15;
    
    // Update is called once per frame
    private void Start()
    {
        nextFire = Time.time + +Random.Range(.5f, 2f);
        //ignore collisions between enemybullets and enemy
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
                Fire(player.position);
            }

        


    }

    public void Fire(Vector3 pos)
    {
        GameObject blt;
        blt = Instantiate(bullet, transform.position, transform.rotation);
        blt.GetComponent<Rigidbody2D>().velocity = (pos - transform.position).normalized * bulletSpeed;
    }
    private IEnumerator Laser()
    {
        firing = true;
        Vector3 playerPos = player.position;

        for (int i = 0; i < beamCount; i++)
        {
            Fire(playerPos);
            yield return new WaitForSeconds(1f / beamDensity);
        }
        firing = false;
        yield return null;
    }
}
