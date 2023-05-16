using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour, IDamageTaker
{
    // Literally how many hits can car get. Might be changed to relative system in %
    [SerializeField] private int maxHitPoints = 4; 
    [SerializeField] private GameObject nextDamageStatePrefab; 
    private static int currentHitPoints;
    private static bool isInitialized = false;
    private CarController carController;
    
    private void Awake() {
        if (!isInitialized) {
            currentHitPoints = maxHitPoints;
            isInitialized = true;
        }
    }

    private void Start() {
        carController = GetComponent<CarController>();
    }

    private void Update() {
        Debug.Log(currentHitPoints);
        if (currentHitPoints < 1) 
            Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision) {
        GameObject collisionObject = collision.gameObject;
        IDamageTaker collisionDamageTaker = collisionObject.GetComponent<IDamageTaker>();
        
        float speedThreshold = 3;

        if (collisionDamageTaker != null && carController.GetCurrentSpeed() >= speedThreshold)
        {
            collisionDamageTaker.TakeDamage();
            TakeDamage();
        }
    }

    public bool isActive()
    {
        return true;
    }

    public void TakeDamage()
    {
        currentHitPoints -= 1;
        SpawnNextDamageStateCar();
    }

    private void SpawnNextDamageStateCar()
    {
        Vector3 carPosition = transform.position;
        Quaternion carRotation = transform.rotation;

        if (nextDamageStatePrefab == null)
        {
            Destroy(gameObject);
            return;
        }

        GameObject newCar = Instantiate(nextDamageStatePrefab, carPosition, carRotation);
        newCar.GetComponent<CarController>().SetTarget(carController.GetTarget());
        newCar.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    // private void OnDrawGizmosSelected() {
    //     try
    //     {
    //         // Gizmos.color = Color.yellow;
    //         // Gizmos.DrawSphere(newTarget.transform.position, 1f);
    //         // foreach (ContactPoint contact in _collision) {
    //         //     Gizmos.DrawRay(contact.point, contact.normal);
    //         // }

    //     }
    //     catch (System.Exception) {}
    // }
}
