
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING }  
    //private int enemyCount;
    private float searchCountDown = 1f;


        public float spawnRate = 2;
        public GameObject[] enemyPool = new GameObject[1]; //enemy types
        public GameObject[] enemies = new GameObject[100]; //max numl enemies
        public int dangerPoints = 4;
        private int enemyCount = 0; 
        //public Transform enemy2;
        //public Transform enemy3;
 
    public int currentWave = 1;
    public Transform[] spawnPoints; //different spawns
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
            if (state != SpawnState.SPAWNING) //dont execute if enemys still alive
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
            if (enemy.GetComponent<EnemyInterface>().dangerBudget <= budget) //spend our allocated budget
            {
                enemies[i] = enemy;
                budget -= enemy.GetComponent<EnemyInterface>().dangerBudget;
                enemyCount++;
                i++;
            }
            
        }
        yield break;
    }

    public bool EnemiesAlive() //check every 1 second, less resource intensive
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
        dangerPoints = (currentWave - 1) * 10; //increase our budget linearly
        if(currentWave == 5)
        {
            spawnRate *= 2;
        }
       
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
       
        while (spawnIndex == lastSpawn) //make sure we dont spawn 2 enemies from same point
            spawnIndex = Random.Range(0, spawnPoints.Length);

        Transform sp = spawnPoints[spawnIndex];
        Instantiate(enemy, sp.position, sp.rotation);
        lastSpawn = spawnIndex;
    }
}

