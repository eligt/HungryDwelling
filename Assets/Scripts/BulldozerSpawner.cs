using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerSpawner : RandomSpawner<BulldozerSpawner>
{
    public float spawnInterval = 12.0f;
    public int maxPupulation = 1;

    private float timeSinceSpawn = 0.0f;
    private int totalPopulation = 0;

    // Use this for initialization
    void Start()
    {
        timeSinceSpawn = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn >= spawnInterval)
        {
            timeSinceSpawn = 0.0f;

            SpawnBulldozer();
        }
    }

    public void SpawnBulldozer()
    {
        if (totalPopulation >= maxPupulation)
            return;
        
        Vector2 spawnPoint = getNextSpawnPoint();
        Vector2 minDistance = new Vector2(0.1f, 0.06f);
        Vector2 maxDistance = new Vector2(0.6f, 0.3f);
        
        {
            Vector2 instSpawn = spawnPoint + minDistance + maxDistance * Random.insideUnitCircle;
            GameObject bulldozer = Instantiate(spawnObject, spawnRoot.transform, false);
            bulldozer.transform.localPosition = new Vector3(instSpawn.x, instSpawn.y, spawnObject.transform.position.z);
            bulldozer.SetActive(true);

            totalPopulation++;
        }
    }

    public void UnspawnBulldozer(BulldozerControl bulldozer)
    {
        Destroy(bulldozer.gameObject);
        totalPopulation--;
    }
}
