using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) 
    {
        GameObject colliderObject = collider.gameObject;

        if (colliderObject.CompareTag("Player"))
        {
            FPMovementController playerController = colliderObject.GetComponent<FPMovementController>();
            playerController.Die();
        }
    }
}