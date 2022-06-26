using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{


    public GameObject[] platformPrefabs;
    public float[] spawnChances;

    [Range(0f,1f)]
    public float creditSpawnChance;

    private List<GameObject> platforms = new List<GameObject>();

    public GameObject[] enemyPrefabs;
    private List<GameObject> enemies = new List<GameObject>();

    public float levelWidth = 2.5f;
    public float minY = 0.2f;

    private float spawnHeight = 25f;
    private float heightToGenerate;
    private float accessibleHeight;

    private float enemyHeight = 0f; //The height at which enemies are currently spawned (after platforms)

    private Transform player;

    private GameObject creditPrefab;

    private float lazerEnemySpawnInterval = 15f; //Spawns lazer enemy every 15 seconds

    

    // Start is called before the first frame update
    void Start()
    {
        creditPrefab = Resources.Load("Prefabs/Credits") as GameObject;
        player = FindObjectOfType<Player>().gameObject.transform;
        AddStartPlatforms();
        InvokeRepeating("SpawnLazerEnemy", 2f, lazerEnemySpawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            heightToGenerate = player.transform.position.y + spawnHeight;
            GeneratePlatforms();
            GenerateEnemies();
            RemovePlatforms();
            RemoveEnemies();
        }
    }


    private void AddStartPlatforms()
    {
        Transform startPlatforms = GameObject.Find("Start Platforms").transform;

        foreach (Transform platform in startPlatforms)
        {
            platforms.Add(platform.gameObject);
        }
        accessibleHeight = platforms[0].transform.position.y;
    }

    private void GenerateEnemies()
    {

        if (enemyHeight < heightToGenerate)
        {
            Vector3 spawnPosition = new Vector3();

            spawnPosition.y = enemyHeight + Random.Range(10f,20f);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);

            GameObject newEnemy = Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity, transform);

            enemies.Add(newEnemy);
            enemyHeight = newEnemy.transform.position.y;
        }
    }

    private void RemoveEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy= enemies[i];

            if (enemy == null)
            {
                platforms.Remove(enemy);
            }
            else if (enemy.transform.position.y <= player.transform.position.y - Camera.main.orthographicSize - 5f)
            {
                platforms.Remove(enemy);
                Destroy(enemy);
            }
        }
    }


    private void GeneratePlatforms()
    {
        
        if (accessibleHeight < heightToGenerate) //Generate a platform if inacessible
        {
            GameObject platformToSpawn = SpawnRandomWeighted();
            Vector3 spawnPosition = new Vector3();

            float maxY = platformToSpawn.GetComponent<Platform>().maxY;
            spawnPosition.y = Random.Range(accessibleHeight - minY, accessibleHeight - maxY);
            spawnPosition.x = Random.Range(-levelWidth, levelWidth);
            GameObject newPlatform = Instantiate(platformToSpawn, spawnPosition, Quaternion.identity, transform);

            TrySpawnCredit(newPlatform.transform);
            platforms.Add(newPlatform);

            accessibleHeight = spawnPosition.y + maxY;
        }

    }

    private void TrySpawnCredit(Transform platform)
    {
        if (Random.value < creditSpawnChance)
        {
            GameObject credit = Instantiate(creditPrefab, new Vector2(platform.position.x,platform.position.y + 0.25f), Quaternion.identity);
            credit.transform.parent = platform;
            
        }
    }

    private GameObject SpawnRandomWeighted()
    {

        float sum = 0;
        foreach (float spawnChance in spawnChances) //Try to make sure sum adds up to 100%
        {
            sum += spawnChance;
        }

        float randomWeight = 0;
        do
        {
            //No weight on any number?
            if (sum == 0)
                return null;
            randomWeight = Random.Range(0, sum);
        }
        while (randomWeight == sum);

        for (int i = 0; i < spawnChances.Length; i++)
        {
            if (randomWeight < spawnChances[i])
                return platformPrefabs[i];
            randomWeight -= spawnChances[i];
        }

        return null; //Shouldn't get here
    }

    private void RemovePlatforms()
    {

        for(int i = platforms.Count - 1; i >= 0; i--)
        {
            GameObject platform = platforms[i];

            if(platform == null)
            {
                platforms.Remove(platform);
            }
            else if (platform.transform.position.y <= player.transform.position.y - Camera.main.orthographicSize - 5f)
            {
                platforms.Remove(platform);
                Destroy(platform);
            }
        }
    }

    
    public void SpawnLazerEnemy()
    {
        float xOffset;
        if (Random.value > 0.5f)
        {
            xOffset = -levelWidth - 1f;
        } else
        {
            xOffset = levelWidth + 1f;
        }
        Vector3 spawnPosition = new Vector3(Camera.main.transform.position.x + xOffset, Camera.main.transform.position.y - Camera.main.orthographicSize + 1f);

        GameObject newEnemy = Instantiate(enemyPrefabs[1], spawnPosition, Quaternion.identity, Camera.main.transform);
        enemies.Add(newEnemy);
        
    }

    public void SpawnDemonEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-levelWidth, levelWidth), Camera.main.transform.position.y + Camera.main.orthographicSize + 1f);

        GameObject newEnemy = Instantiate(enemyPrefabs[2], spawnPosition, Quaternion.identity);
        enemies.Add(newEnemy);
        
    }
}
