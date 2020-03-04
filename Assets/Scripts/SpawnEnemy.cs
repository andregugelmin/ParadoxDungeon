using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemy : MonoBehaviour
{
    ObjectPooler objectPooler;

    [SerializeField]
    private int radius;
    [SerializeField]
    private float spawnDelay;
    private float nextSpawnTime;

    [SerializeField]
    private string tag;

    private int spawnQtt;

    private bool spawnedAtStart = false;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        if (!spawnedAtStart)
        {
            SpawnAtStart();
        }
        else if (ShouldSpawn())
        {
            Spawn();
        }
    }

    private void SpawnAtStart()
    {
        Vector3 position = new Vector3(gameObject.transform.position.x - Random.Range(-radius, radius),
            gameObject.transform.position.y, gameObject.transform.position.z - Random.Range(-radius, radius));
        if (objectPooler.poolDictionary[tag].Count > 0)
        {    
            objectPooler.SpawnFromPool(tag, position, Quaternion.identity);
        }
        else
        {
            spawnedAtStart = true;
            nextSpawnTime = Time.time + spawnDelay;
        }
            
    }

    private void Spawn()
    {
        Vector3 position = new Vector3(gameObject.transform.position.x - Random.Range(-radius, radius),
            gameObject.transform.position.y, gameObject.transform.position.z - Random.Range(-radius, radius));
        nextSpawnTime = Time.time + spawnDelay;
        if (objectPooler.poolDictionary[tag].Count > 0)
        {
            objectPooler.SpawnFromPool(tag, position, Quaternion.identity);
        }
    }

    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
   
}
