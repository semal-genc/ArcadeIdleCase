using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public Transform backpack;
    public List<Transform> collectedItems = new List<Transform>();
    public List<Transform> transformedItems = new List<Transform>();
    private HashSet<Transform> droppedItems = new HashSet<Transform>();
    public float stackOffset = 0.05f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CollectibleArea"))
        {
            CollectItemInArea(other);
        }
        if (other.CompareTag("TransformedArea"))
        {
            TransformedItemInArea(other);
        }
    }

    private void CollectItemInArea(Collider area)
    {
        Collider[] colliders = Physics.OverlapBox(area.bounds.center, area.bounds.extents);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Collectible") && !droppedItems.Contains(col.transform))
            {
                CollectItem(col.transform);
            }
        }
    }

    private void TransformedItemInArea(Collider area)
    {
        Collider[] colliders = Physics.OverlapBox(area.bounds.center, area.bounds.extents);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Transformed") && !droppedItems.Contains(col.transform))
            {
                TransformedItem(col.transform);
            }
        }
    }

    private void CollectItem(Transform item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            item.SetParent(backpack);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Collider col = item.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            UpdateItemPositions();
        }
    }

    private void TransformedItem(Transform item)
    {
        if (!transformedItems.Contains(item))
        {
            transformedItems.Add(item);
            item.SetParent(backpack);

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Collider col = item.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            UpdateItemPositions();
        }
    }

    private void UpdateItemPositions()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            Vector3 newPosition = backpack.position + Vector3.up * (i * stackOffset);
            collectedItems[i].position = newPosition;
        }
        for (int i = 0; i < transformedItems.Count; i++)
        {
            Vector3 newPosition = backpack.position + Vector3.up * (i * stackOffset);
            transformedItems[i].position = newPosition;
        }
    }

    public void MarkAsDropped(Transform item)
    {
        droppedItems.Add(item);
    }

    public void RemoveItemFromListAndDestroy(Transform item)
    {
        if (item != null)
        {
            if (collectedItems.Contains(item))
            {
                collectedItems.Remove(item);
                Debug.Log("Item removed from collectedItems");
            }

            if (transformedItems.Contains(item))
            {
                transformedItems.Remove(item);
                Debug.Log("Item removed from transformedItems");
            }

            Destroy(item.gameObject);
            Debug.Log("Item destroyed: " + item.name);
        }
    }
}