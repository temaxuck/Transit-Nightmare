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
    private float brakeForce;

    [SerializeField]
    private float maxSteerAngle;
    #endregion


    public bool isBraking = false;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = Mathf.Clamp(agent.speed, 0, motorForce);
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
    }

    private void Update()
    {
        agent.speed = Mathf.Clamp(agent.speed, 0, motorForce);
        agent.SetDestination(target.position);
    }

    private void FixedUpdate()
    {
        HandleSteering();
        HandleMotor();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        // Calculate desired motor torque based on NavMeshAgent's desired velocity
        float desiredSpeed = agent.desiredVelocity.magnitude;
        float acceleration = (desiredSpeed - currentSpeed) / Time.deltaTime;
        float motorTorque = acceleration * rb.mass;
        currentSpeed = desiredSpeed;

        Vector3 agentDirection = agent.desiredVelocity.normalized;
        float carAgentAngle = Vector3.Angle(transform.forward, agentDirection);

        // Stop and rotate towards target if angle is greater than maxSteerAngle
        if (carAgentAngle > maxSteerAngle)
        {
            ApplyBraking();

            // Rotate towards target
            float turnDirection = Vector3.Cross(transform.forward, agentDirection).y;
            rb.AddTorque(0, turnDirection * motorForce * Time.deltaTime, 0);
        }
        else
        {
            StopBraking();
            ApplyMotorForce(motorTorque);
        }
    }

    private void ApplyMotorForce(float motorTorque)
    {
        if (!isBraking)
        {
            // The car is front wheels driven so apply motorForce only on front wheels
            for (int i = 0; i < 2; i++)
            {
                wheelColliders[i].motorTorque = motorTorque;
            }
        }
    }

    private void ApplyBraking()
    {
        isBraking = true;

        if (rb.velocity.magnitude < 1)
            isBraking = false;

        if (isBraking)
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].brakeTorque = brakeForce;
            }

            Vector3 brakingForce = -rb.velocity.normalized * brakeForce;

            // Apply braking force
            rb.AddForce(brakingForce);
        }
    }

    private void StopBraking()
    {
        isBraking = false;

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].brakeTorque = 0;
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
