using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange;

    private ItemPickup itemInRange;

    private void Update()
    {
        RaycastHit hit;

        // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one * 0.5f);
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            ItemPickup item = hit.collider.GetComponent<ItemPickup>();

            if (item != null)
            {
                itemInRange = item;
                item.SetIconVisibility(true);
            }
            else if (itemInRange != null)
            {
                itemInRange.SetIconVisibility(false);
                itemInRange = null;
            }
        }
        else if (itemInRange != null)
        {
            itemInRange.SetIconVisibility(false);
            itemInRange = null;
        }

        if (Input.GetKeyDown(KeyCode.E) && itemInRange != null)
        {
            Debug.Log("Pick");
            itemInRange.SetIconVisibility(false);
            itemInRange.PickUpItem();
            itemInRange = null;

        }
    }
}