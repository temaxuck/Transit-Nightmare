using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class CarMovement : MonoBehaviour
{
    #region "Car physics variables"
    private float currentSteerAngle;
    private float currentSpeed;

    [SerializeField]
    private Vector3 centerOfMassOffset;

    private Rigidbody rb;
    #endregion

    #region "Car controller varibles"
    [SerializeField]
    private Transform target;

    private UnityEngine.AI.NavMeshAgent agent;
    #endregion

    #region "Wheel objects variables"
    [SerializeField]
    private WheelCollider[] wheelColliders; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;

    [SerializeField]
    private Transform[] wheelMeshes; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
    #endregion

    #region "Car physics parameters"
    [SerializeField]
    private float motorForce;

    [SerializeField]
    private float breakForce;

    [SerializeField]
    private float maxSteerAngle;
    #endregion

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = motorForce;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {
        HandleSteering();
        ApplyMotorForce();
        UpdateWheels();
    }

    private void ApplyMotorForce()
    {
        // Calculate desired motor torque based on NavMeshAgent's desired velocity
        float desiredSpeed = agent.desiredVelocity.magnitude;
        float acceleration = (desiredSpeed - currentSpeed) / Time.deltaTime;
        float motorTorque = acceleration * rb.mass;
        currentSpeed = desiredSpeed;

        // The car is front wheels driven so apply motorForce only on front wheels
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].motorTorque = motorTorque;
        }
    }

    private void HandleSteering()
    {
        // Calculate desired steering angle based on NavMeshAgent's desired velocity
        Vector3 relativeVelocity = transform.InverseTransformDirection(agent.desiredVelocity);
        float steerDirection = relativeVelocity.x / relativeVelocity.magnitude;
        currentSteerAngle = steerDirection * maxSteerAngle;

        // Set steering angle for front wheels
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = currentSteerAngle;
        }
    }

    private void UpdateWheels()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            WheelCollider wheelCollider = wheelColliders[i];
            Transform wheelMesh = wheelMeshes[i];
            Vector3 wheelPosition;
            Quaternion wheelRotation;
            wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);

            // Set the wheel mesh's position and rotation
            wheelMesh.transform.position = wheelPosition;
            wheelMesh.transform.rotation = wheelRotation;
        }
    }
}
