using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.ComponentModel;

public class CarController : MonoBehaviour
{
    #region Car physics variables
        [SerializeField] private Vector3 centerOfMassOffset;
        private Rigidbody rb;
        private BoxCollider boxCollider;
        private Vector3 carSize;
        private CarMovement carMovement;
    #endregion

    #region Car controller varibles
        [SerializeField] private Transform target;
        [SerializeField] private NavMeshAgent ghostAgent;
        // [SerializeField] private float angleDetectionDistance = 10f;
        private float backUpDistance = 100f; // distance to travel back when Entered Collision
        private bool isBackingUp = false;
        private Vector3 lastPosition;
        private float distanceDrivenBackwards;
        [SerializeField] private float obstaclesDetectionDistance = 1f;
        [SerializeField] private LayerMask obstaclesMask;
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
        carMovement = GetComponent<CarMovement>();

        boxCollider = GetComponent<BoxCollider>();
        carSize = boxCollider.size;
        

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;

        ghostAgent.speed = maxMotorForce / 60f;
    }

    private void Update()
    {   
        if (ghostAgent != null)
            CalculatePath();
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

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void CalculatePath()
    {
        // Stick ghost agent to the car
        ghostAgent.nextPosition = transform.position;
        ghostAgent.velocity = rb.velocity;

        // Calculate path to the target
        ghostAgent.SetDestination(target.position);
    }

    #region Handle Motor
        private void HandleMotor() 
        {
            StopBraking();

            Vector3 desiredVelocity = ghostAgent.desiredVelocity; // Desired velocity by ghost agent
            Vector3 actualVelocity = rb.velocity;
            RaycastHit forwardHit;
            RaycastHit backwardHit;
            float velocityProduct = Vector3.Dot(desiredVelocity, actualVelocity);
            
            // Scale the Obstacle Detection Distance in relation to actual speed
            float scaledODD = actualVelocity.magnitude / 100 * obstaclesDetectionDistance; 

            // If the car should drive backwards, drive backwards obviously 
            if (isBackingUp)
            {   
                // If the car cannot drive backwards just force driving forwards
                if (Physics.Raycast(transform.position, -transform.forward, out backwardHit, carSize.z, obstaclesMask)) {
                    DriveForward(actualVelocity, desiredVelocity);
                    
                    return;
                }

                DriveBackward(actualVelocity, desiredVelocity);
                
                if (distanceDrivenBackwards < backUpDistance)
                    return;
            }
            
            // If there's obstacle in front of the car, start extreme braking and backing up
            if (Physics.Raycast(transform.position, transform.forward, out forwardHit, scaledODD, obstaclesMask) && !isBackingUp)
            {
                float speedThreshold = 20f;
                if (actualVelocity.magnitude < speedThreshold)
                {
                    carMovement.ApplyBraking(50000f);
                    isBackingUp = true;
                }
                return ;
            }

            // If there's further turn and it's in front of the car start mild braking
            if (actualVelocity.magnitude > desiredVelocity.magnitude && velocityProduct > 0f)
                carMovement.ApplyBraking(brakeForce / 2); 

            // All checks passed, drive forward with desired velocity
            DriveForward(actualVelocity, desiredVelocity);
        }

        private void DriveForward(Vector3 actualVelocity, Vector3 desiredVelocity)
        {
            isBackingUp = false;
            distanceDrivenBackwards = 0f;
            float motorTorque = desiredVelocity.magnitude * 60f;

            // To think about: What if the car drives back at max speed
            // And you want the car to suddenly drive forward.
            // This check won't allow the car apply positive motorForce
            if (actualVelocity.magnitude <= maxSpeed) 
                carMovement.ApplyMotorForce(motorTorque);
         
            lastPosition = transform.position;
        }

        private void DriveBackward(Vector3 actualVelocity, Vector3 desiredVelocity)
        {
            float motorTorque = -maxMotorForce;

            // Update distance driven backwards
            float distance = Vector3.Distance(transform.position, lastPosition);
            distanceDrivenBackwards += distance; 

            if (actualVelocity.magnitude <= maxSpeed)
                carMovement.ApplyMotorForce(motorTorque);
        }
    #endregion

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

        carMovement.ApplySteering(steerAngle);
    }

    private void StopBraking() 
    {
        carMovement.ApplyBraking(0);
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
            Gizmos.DrawSphere(lastPosition, 1.5f);
        }
        catch (Exception) {}
    }
}
