using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public GameObject transformedPrefab;
    public Transform spawnPoint;
    private Queue<GameObject> collectibles = new Queue<GameObject>();
    private int stackCount = 0;
    public float spawnHeightIncrement = 0.5f;

    private void Start()
    {
        StartCoroutine(TransformRoutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            collectibles.Enqueue(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DropArea") && collectibles.Count > 0)
        {
            TransformCollectible();
        }
    }

    private IEnumerator TransformRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (collectibles.Count > 0)
            {
                TransformCollectible();
            }
        }
    }

    private void TransformCollectible()
    {
        if (collectibles.Count > 0)
        {
            GameObject collectible = collectibles.Dequeue();

            if (collectible == null) return;

            Instantiate(transformedPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnPoint.position += new Vector3(0, spawnHeightIncrement, 0);

            stackCount++;

            Destroy(collectible);
        }
    }
}
