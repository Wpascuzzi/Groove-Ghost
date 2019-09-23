
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }  
    //private int enemyCount;
    private float searchCountDown = 1f;


        public float spawnRate = 2;
        public GameObject[] enemyPool = new GameObject[3];
        public GameObject[] enemies = new GameObject[100];
        public int dangerPoints = 10;
        private int enemyCount = 0; 
        //public Transform enemy2;
        //public Transform enemy3;
 
    private int currentWave = 1;


    public Transform[] spawnPoints;

    public float timeBetween = 5;
    public float waveCountDown = 0;

    private SpawnState state = SpawnState.COUNTING;

    int lastSpawn = -1;
    int spawnIndex = 0;
    void Start()
    {
       StartCoroutine(GenerateEnemies());
        waveCountDown = timeBetween;
    }

    void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if (!EnemiesAlive())
                WaveCompleted();
            else return;
               
        }
        if (waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                //spawns
                StartCoroutine(SpawnWave());
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    IEnumerator GenerateEnemies()
    {
        enemyCount = 0;
        int budget = dangerPoints;
        int i = 0;
        while (budget > 0)
        {
            GameObject enemy = enemyPool[Random.Range(0, enemyPool.Length)];
            if (enemy.GetComponent<EnemyInterface>().dangerBudget <= budget)
            {
                enemies[i] = enemy;
                budget -= enemy.GetComponent<EnemyInterface>().dangerBudget;
                enemyCount++;
                i++;
            }
            
        }
        yield break;
    }

    public bool EnemiesAlive()
    {
        
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            print("checking");
            searchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
                return false;
        }
        return true;
    }

    void WaveCompleted()
    {
        print("completed");
        state = SpawnState.COUNTING;
        waveCountDown = timeBetween;
        currentWave++;
        dangerPoints = currentWave * 10;
        StartCoroutine(GenerateEnemies());
    }
    IEnumerator SpawnWave()
    {
        state = SpawnState.SPAWNING;
        //spawn

        for (int i = 0; i < enemyCount; i++)
        {
            Spawn(enemies[i]);
            yield return new WaitForSeconds(1f/spawnRate);
        }
        state = SpawnState.WAITING;

        yield break;
    }
    void Spawn(GameObject enemy)
    {
       
        while (spawnIndex == lastSpawn)
            spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform sp = spawnPoints[spawnIndex];
        Instantiate(enemy, sp.position, sp.rotation);
        lastSpawn = spawnIndex;
    }
}

