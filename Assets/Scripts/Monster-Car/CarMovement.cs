using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.ComponentModel;

public class CarMovement : MonoBehaviour
{
    #region Car physics variables
        private float currentSteerAngle;
        [SerializeField] private Vector3 centerOfMassOffset;
        private Rigidbody rb;
    #endregion

    #region Car controller varibles
        [SerializeField] private Transform target;
        [SerializeField] private NavMeshAgent ghostAgent;
        // [SerializeField] private float angleDetectionDistance = 10f;
        private float backUpDistance = 100f; // distance to travel back when Entered Collision
        private bool isBackingUp = false;
        bool isBraking = false;
        private Vector3 lastPosition;
        private float distanceDriven;
        [SerializeField] private float obstaclesDetectionDistance = 100f;
        [SerializeField] private LayerMask obstaclesMask;

    #endregion

    #region Wheel objects variables
        [SerializeField] private WheelCollider[] wheelColliders; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
        [SerializeField] private Transform[] wheelMeshes; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
    #endregion

    #region Car physics parameters
        [SerializeField] private float maxMotorForce;
        [SerializeField] private float brakeForce;
        [SerializeField] private float maxSteerAngle;
        [SerializeField] private float maxSpeed;
    #endregion

    #region Car auxiliary variables
        public bool showGizmos = false;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;

        ghostAgent.speed = maxMotorForce / 60f;
    }

    private void Update()
    {   
        if (ghostAgent != null)
            CalculatePath();

        UpdateWheelsPositions();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
    }

    private void OnCollisionEnter(Collision other) {
        // The car is not moving
        isBackingUp = true;
    }

    private void CalculatePath()
    {
        // Stick ghost agent to the car
        ghostAgent.nextPosition = transform.position;
        ghostAgent.velocity = rb.velocity;

        // Calculate path to the target
        ghostAgent.SetDestination(target.position);
    }

    #region Motor
        private void HandleMotor() 
        {
            Debug.Log(isBraking);
            StopBraking();

            Vector3 desiredVelocity = ghostAgent.desiredVelocity; // Desired velocity by ghost agent
            Vector3 actualVelocity = rb.velocity;

            if (isBackingUp)
            {   
                DriveBack(actualVelocity, desiredVelocity);
                float distance = Vector3.Distance(transform.position, lastPosition);
                distanceDriven += distance;
                Debug.Log(distanceDriven);

                if (distanceDriven < backUpDistance)
                    return;
            }

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, obstaclesDetectionDistance, obstaclesMask) && !isBackingUp)
            {
                float speedThreshold = 20f;
                if (actualVelocity.magnitude < speedThreshold)
                {
                    ApplyBraking(50000f);
                    isBackingUp = true;
                }
                return ;
            }

            isBackingUp = false;
            distanceDriven = 0f;

            DriveForward(actualVelocity, desiredVelocity);
            
            lastPosition = transform.position;
        }

        private void DriveForward(Vector3 actualVelocity, Vector3 desiredVelocity)
        {
            float motorTorque = maxMotorForce;
            float velocityProduct = Vector3.Dot(desiredVelocity, actualVelocity);

            if (actualVelocity.magnitude > desiredVelocity.magnitude && velocityProduct < 0f)
            {
                motorTorque = desiredVelocity.magnitude * 60f;    
                // Applying half of brakeForce, because this is not the extreme braking
                ApplyBraking(brakeForce / 2); 
            }
            
            if (actualVelocity.magnitude <= maxSpeed)
                ApplyMotorForce(motorTorque);
        }

        private void DriveBack(Vector3 actualVelocity, Vector3 desiredVelocity)
        {
            float motorTorque = -maxMotorForce;

            if (actualVelocity.magnitude <= maxSpeed)
                ApplyMotorForce(motorTorque);
        }

        private void ApplyMotorForce(float motorTorque) 
    {
        // The car is front wheels driven so apply motorTorque only on first 2 wheels
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].motorTorque = motorTorque;
        }
    }
    #endregion

    #region Steering
        private void HandleSteering() 
        {
            float steerAngle;
            if (ghostAgent.path.corners.Length > 1 && !isBackingUp) {
                Vector3 wayPoint = ghostAgent.path.corners[1];
                Vector3 targetDirection = wayPoint - transform.position;
                float carTargetAngle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);

                steerAngle = Mathf.Clamp(carTargetAngle, -maxSteerAngle, maxSteerAngle);
            }
            else 
            {
                steerAngle = 0;
            }
            
            ApplySteering(steerAngle);
        }

        private void ApplySteering(float steerAngle) 
        {
            // Apply steering only on front wheels
            for (int i = 0; i < 2; i++)
            {
                wheelColliders[i].steerAngle = steerAngle;
            }
        }
    #endregion

    #region Braking
        private void ApplyBraking(float brakeForce) 
        {
            isBraking = true;
            
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                wheelColliders[i].brakeTorque = brakeForce;
            }

        }
        private void StopBraking() 
        {
            ApplyBraking(0);
            isBraking = false;
        }
    #endregion

    private void UpdateWheelsPositions()
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

    private void OnDrawGizmosSelected() 
    {
        if (!showGizmos)
            return ;

        try
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < ghostAgent.path.corners.Length; i++)
            {
                Gizmos.DrawSphere(ghostAgent.path.corners[i], 0.5f);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastPosition, 3f);
        }
        catch (Exception)
        {
            // ...
        }
    }
}
