// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.ComponentModel;

// public class CarMovement : MonoBehaviour
// {
//     #region "Car physics variables"
//         private float currentSteerAngle;
//         private float currentSpeed;
//         [SerializeField] private Vector3 centerOfMassOffset;
//         private Rigidbody rb;
//     #endregion

//     #region "Car controller varibles"
//         [SerializeField] private Transform target;
//         private UnityEngine.AI.NavMeshAgent agent;
//         private bool shouldChaseTarget = true;
//         private bool isBackingUp = false;
//         private float backupDistance = 2f;
//         private Vector3 backupTarget;
//         private bool isBraking = false;
//     #endregion

//     #region "Wheel objects variables"
//         [SerializeField] private WheelCollider[] wheelColliders; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
//         [SerializeField] private Transform[] wheelMeshes; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
//     #endregion

//     #region "Car physics parameters"
//         [SerializeField] private float motorForce;
//         [SerializeField] private float brakeForce;
//         [SerializeField] private float maxSteerAngle;
//     #endregion

//     void Start()
//     {
//         agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//         agent.speed = motorForce;
//         rb = GetComponent<Rigidbody>();
//         rb.centerOfMass += centerOfMassOffset;
//     }

//     private void Update()
//     {
//         HandleChasing();
//         // // if ()
//         // agent.SetDestination(target.position);
//     }

//     private void FixedUpdate()
//     {
//         HandleBraking();
//         HandleMotor();
//         HandleSteering();
//         UpdateWheels();
//     }

//     #region "Handle chasing"
//         private void HandleChasing()
//         {
//             CheckIfShouldChaseTarget();
//             if (shouldChaseTarget)
//             {
//                 agent.enabled = true;
//                 agent.SetDestination(target.position);
//             }
//             else
//                 agent.enabled = false;
//         }

//         private void CheckIfShouldChaseTarget()
//         {
//             Vector3 targetDirection = (target.position - transform.position).normalized;
//             float carTargetAngle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
            
//             // if target is not in front of reachable angle and the car is not already backing up
//             if (Mathf.Abs(carTargetAngle) > maxSteerAngle / 2f && !isBackingUp)
//             {
//                 // Stop the car and start backing up.
//                 isBraking = true;
//                 isBackingUp = true;
//                 shouldChaseTarget = false;

//                 // set steer angle opposite to angle between car and target when backing up
//                 currentSteerAngle = -carTargetAngle;

//                 backupTarget = transform.position - transform.forward * backupDistance;
//             }
//             else
//             {
//                 // if target is in front of reachable angle and the car is not backing up
//                 if (!isBackingUp)
//                 {
//                     // proceed chasing target
//                     isBraking = false;
//                     isBackingUp = false;
//                     shouldChaseTarget = true;
//                 }
//             }
//         }
//     #endregion

//     #region "Handle braking"
//         private void HandleBraking()
//         {
//             if (isBraking)
//                 if (rb.velocity.magnitude == 0f)
//                 {
//                     isBraking = false;
//                     StopBraking();
//                 }
//                 else
//                     ApplyBraking(); 
//         }
        
//         private void ApplyBraking()
//         {
//             for (int i = 0; i < wheelColliders.Length; i++)
//             {
//                 wheelColliders[i].motorTorque = 0;
//                 wheelColliders[i].brakeTorque = brakeForce;
//             }
//         }

//         private void StopBraking() 
//         {
//             for (int i = 0; i < wheelColliders.Length; i++)
//             {
//                 wheelColliders[i].brakeTorque = 0;
//             }
//         }
//     #endregion

//     #region "Handle motor"
//         private void HandleMotor()
//         {
//             if (!isBraking)
//             {
//                 if (isBackingUp)
//                     DriveBackwards();
//                 else
//                     DriveForwards();
//             }
//         }

//         private void DriveBackwards() 
//         {
//             float motorTorque = -motorForce;

//             // Stop backing up when the car reaches the backup target
//             if (Vector3.Distance(transform.position, backupTarget) < 0.1f)
//                 isBackingUp = false;     

//             ApplyMotorTorque(motorTorque);
//         }

//         private void DriveForwards()
//         {
//             // Calculate desired motor torque based on NavMeshAgent's desired velocity
//             float desiredSpeed = agent.desiredVelocity.magnitude;
//             float acceleration = (desiredSpeed - currentSpeed) / Time.deltaTime;
//             float motorTorque = acceleration * rb.mass;
//             currentSpeed = desiredSpeed;

//             ApplyMotorTorque(motorTorque);
//         }

//         private void ApplyMotorTorque(float motorTorque)
//         {
//             // The car is front wheels driven so apply motorTorque only on front wheels
//             for (int i = 0; i < 2; i++)
//                 wheelColliders[i].motorTorque = motorTorque;
//         }
//     #endregion

//     #region "Handle steering"
//         private void HandleSteering()
//         {
//             float steerDirection;

//             if (!isBackingUp)
//             {
//                 // Calculate desired steering angle based on NavMeshAgent's desired velocity
//                 Vector3 relativeVelocity = transform.InverseTransformDirection(agent.desiredVelocity);
//                 steerDirection = relativeVelocity.x / relativeVelocity.magnitude;
//                 currentSteerAngle = steerDirection * maxSteerAngle;
//             }
//             // else the currentSteerAngle already set in CheckIfShouldChaseTarget

//             // Set steering angle for the front wheels
//             for (int i = 0; i < 2; i++)
//                 wheelColliders[i].steerAngle = currentSteerAngle;
//         }    
//     #endregion

//     private void UpdateWheels()
//     {
//         for (int i = 0; i < wheelColliders.Length; i++)
//         {
//             WheelCollider wheelCollider = wheelColliders[i];
//             Transform wheelMesh = wheelMeshes[i];
//             Vector3 wheelPosition;
//             Quaternion wheelRotation;
//             wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);

//             // Set the wheel mesh's position and rotation
//             wheelMesh.transform.position = wheelPosition;
//             wheelMesh.transform.rotation = wheelRotation;
//         }
//     }
// }
