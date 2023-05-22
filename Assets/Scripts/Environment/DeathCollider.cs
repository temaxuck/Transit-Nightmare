using UnityEngine;

public class DeathCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) 
    {
        GameObject colliderObject = collider.gameObject;

        if (colliderObject.CompareTag("Player"))
        {
            Debug.LogFormat("Die {0} biiaatch..!", colliderObject);
        }
    }
}