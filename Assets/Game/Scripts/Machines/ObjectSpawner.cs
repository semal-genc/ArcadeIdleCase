using UnityEngine;
using System.Collections;
using System;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnPoint;
    public int targetSpawnCount = 10;
    public float spawnInterval = 5f;
    public float spawnHeightIncrement = .5f;

    private int currentSpawnCount = 0;

    void Start()
    {
        StartCoroutine(SpawnObjectsOverTime());
    }

    IEnumerator SpawnObjectsOverTime()
    {
        while (currentSpawnCount < targetSpawnCount)
        {
            SpawnObject();
            currentSpawnCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnObject()
    {
        Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);

        spawnPoint.position += new Vector3(0, spawnHeightIncrement, 0);
    }
}