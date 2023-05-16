using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
    // Literally how many hits can car get. Might be changed to relative system in %
    [SerializeField] private int hitPoints = 4; 
    [SerializeField] private string damageTriggerTag = "Damage Trigger";

    private void Update() {
        if (hitPoints < 1) 
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        GameObject collisionObject = collision.gameObject;
        if (collisionObject.CompareTag(damageTriggerTag))
        {
            Destroy(collisionObject);
            hitPoints -= 1;
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    // private void OnDrawGizmosSelected() {
    //     try
    //     {
    //         foreach (ContactPoint contact in _collision) {
    //             Gizmos.color = Color.yellow;
    //             Gizmos.DrawRay(contact.point, contact.normal);
    //         }
    //     }
    //     catch (System.Exception) {}
    // }
}
