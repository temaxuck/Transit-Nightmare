using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour, IDamageTaker
{
    // Literally how many hits can car get. Might be changed to relative system in %
    [SerializeField]
    private int maxHitPoints = 4;

    [SerializeField]
    private GameObject nextDamageStatePrefab;

    [SerializeField]
    private GameObject[] debreeProps;

    [SerializeField]
    private float minimumSpeedCollision = 3f;

    private static int currentHitPoints;
    private static bool isInitialized = false;
    private CarController carController;

    private void Awake()
    {
        if (!isInitialized)
        {
            currentHitPoints = maxHitPoints;
            isInitialized = true;
        }
    }

    private void Start()
    {
        carController = GetComponent<CarController>();
    }

    private void Update()
    {
        if (currentHitPoints < 1)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        IDamageTaker collisionDamageTaker = collisionObject.GetComponent<IDamageTaker>();

        if (
            collisionDamageTaker != null && carController.GetCurrentSpeed() >= minimumSpeedCollision
        )
        {
            if (collisionDamageTaker.isActive())
            {
                TakeDamage();
                collisionDamageTaker.TakeDamage();
            }
        }

        if (
            collisionObject.CompareTag("Player")
            && carController.GetCurrentSpeed() >= minimumSpeedCollision
        )
        {
            FPMovementController playerController =
                collisionObject.GetComponent<FPMovementController>();
            playerController.Die();
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
        SpawnDebreeProps();
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
        CarController newCarController = newCar.GetComponent<CarController>();
        if (newCarController != null)
            newCarController.SetTarget(carController.GetTarget());
        newCar.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    private void SpawnDebreeProps()
    {
        Vector3 carPosition = transform.position;
        Quaternion carRotation = transform.rotation;

        foreach (GameObject debreeProp in debreeProps)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-10.0f, 10.0f),
                2f,
                Random.Range(-10.0f, 10.0f)
            );
            GameObject debreeItem = Instantiate(
                debreeProp,
                carPosition + randomPosition,
                carRotation
            );
        }
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
