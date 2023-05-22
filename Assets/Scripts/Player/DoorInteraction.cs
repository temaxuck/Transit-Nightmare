using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange;

    private Door itemInRange;

    private void Update()
    {
        RaycastHit hit;

        // Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Ray ray = playerCamera.ViewportPointToRay(Vector3.one * 0.5f);
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            Door item = hit.collider.GetComponent<Door>();

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
            Debug.Log("Open door");
            itemInRange.SetIconVisibility(false);
            itemInRange.OpenDoor();
            itemInRange = null;

        }
    }
}