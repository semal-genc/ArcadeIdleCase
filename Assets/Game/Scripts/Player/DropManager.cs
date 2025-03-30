using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public Transform dropArea;
    public Transform trashArea;
    CollectibleManager collectibleManager;
    public float dropOffset = 0.2f;
    public int maxDropCount = 10;

    private List<Transform> droppedItems = new List<Transform>();

    private void Start()
    {
        collectibleManager = GetComponent<CollectibleManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropArea"))
        {
            DropAllItems();
        }
        else if (other.CompareTag("Trash"))
        {
            TrashItems();
        }
    }

    private void DropAllItems()
    {
        if (droppedItems.Count >= maxDropCount)
        {
            Debug.Log("DropArea dolu! Daha fazla obje býrakýlamaz.");
            return;
        }

        List<Transform> itemsToDrop = new List<Transform>(collectibleManager.collectedItems);

        for (int i = 0; i < itemsToDrop.Count; i++)
        {
            if (droppedItems.Count >= maxDropCount)
                break;

            Transform item = itemsToDrop[i];

            if (item != null)
            {
                item.SetParent(null);

                Vector3 dropPosition = dropArea.position + Vector3.up * (droppedItems.Count * dropOffset);
                item.position = dropPosition;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;

                Collider col = item.GetComponent<Collider>();
                if (col != null) col.enabled = true;

                droppedItems.Add(item);
                collectibleManager.MarkAsDropped(item);
            }
        }

        collectibleManager.collectedItems.Clear();
    }

    private void TrashItems()
    {
        List<Transform> allItemsToTrash = new List<Transform>(collectibleManager.collectedItems);
        allItemsToTrash.AddRange(collectibleManager.transformedItems);

        for (int i = 0; i < allItemsToTrash.Count; i++)
        {
            Transform item = allItemsToTrash[i];
            if (item != null)
            {
                item.SetParent(null);

                item.position = trashArea.position;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;

                Collider col = item.GetComponent<Collider>();
                if (col != null) col.enabled = false;

                collectibleManager.RemoveItemFromListAndDestroy(item);
            }
        }
    }
}
