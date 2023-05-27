using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public GameObject pickupIcon;
    public string itemName = "Key";

    private void Start()
    {
        // Ensure that the pickup icon is initially hidden
        SetIconVisibility(false);
    }

    public void SetIconVisibility(bool isVisible)
    {
        pickupIcon.SetActive(isVisible);
    }

    public void PickUpItem()
    {
        // Add your logic for picking up the item, e.g., adding it to the player's inventory
        if (gameObject.CompareTag(itemName))
        {
            // Add the key to the player's inventory.
            KeyInventory.instance.AddKey(gameObject);
            Debug.Log($"Picked up: {itemName}");
            // Destroy the key object.
            Destroy(gameObject);
        }
    }
}
