using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersonSpawner : RandomSpawner<PersonSpawner>
{
    public GameObject overlayRoot;
    public GameObject arrowObject;
    public float spawnInterval = 12.0f;
    public int minSpawnAmount = 2;
    public int maxSpawnAmount = 8;
    public int maxPupulation = 25;

    private float timeSinceSpawn = 0.0f;
    private int totalPopulation = 0;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeSinceSpawn += Time.deltaTime;

        if (timeSinceSpawn >= spawnInterval)
        {
            timeSinceSpawn = 0.0f;

            SpawnPeople();
        }

        //UpdateOverlay();
	}

    // possibly slow... 
    public void UpdateOverlay()
    {
        Bounds cameraBounds = getCameraBounds();
        Bounds spawnBounds = getSpawnBounds();
        PersonControl[] people = spawnRoot.GetComponentsInChildren<PersonControl>();
        
        int slotsX = 12;
        int slotsY = 6;

        //Dictionary<int, >

        foreach (PersonControl person in people)
        {
            Transform tran = person.gameObject.transform;
            Vector3 pos = new Vector3(tran.position.x, tran.position.y, cameraBounds.center.z);

            if (cameraBounds.Contains(pos))
                continue;

            Vector3 edgePoint = cameraBounds.ClosestPoint(pos);
        }

    }

    public void SpawnPeople(int spawnAmount = -1)
    {
        if (totalPopulation >= maxPupulation)
            return;

        if (spawnAmount == -1)
        {
            spawnAmount = Random.Range(minSpawnAmount, maxSpawnAmount);
        }

        // Note: we don't do the following bounds check because it makes spawning a bit too predictable
        //       for instance, if you reach max population and then eat 2 people, it's then gonna spawn exactly 2 people
        //       instead, removing the bounds check, is gonna spawn random amount (which might exceed maxPopulation this one time)
        //spawnAmount = Mathf.Min(spawnAmount, maxPupulation - totalPopulation);

        //if (spawnAmount <= 0)
        //    return;

        Vector2 spawnPoint = getNextSpawnPoint();
        Vector2 minDistance = new Vector2(0.1f, 0.06f);
        Vector2 maxDistance = new Vector2(0.6f, 0.3f) * spawnAmount;

        while (spawnAmount > 0)
        {
            Vector2 instSpawn = spawnPoint + minDistance + maxDistance * Random.insideUnitCircle;
            GameObject person = Instantiate(spawnObject, spawnRoot.transform, false);
            person.transform.localPosition = new Vector3(instSpawn.x, instSpawn.y, spawnObject.transform.position.z);
            person.SetActive(true);

            spawnAmount--;
            totalPopulation++;
        }
    }

    public void UnspawnPerson(PersonControl person)
    {
        Destroy(person.gameObject);
        totalPopulation--;
    }
}
