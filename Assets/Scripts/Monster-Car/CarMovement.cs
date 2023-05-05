using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    #region "Car physics variables"
        private float currentBreakForce;
        private float currentSteerAngle;
        private bool isBreaking;
        [SerializeField] private Vector3 centerOfMassOffset;
        private Rigidbody rb;   
    #endregion

    #region "Chase target varibles"
        [SerializeField] private Transform target;
    #endregion


    #region "Wheel objects variables"
        [SerializeField] private  WheelCollider[] wheelColliders; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
        [SerializeField] private Transform[] wheelMeshes; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
    #endregion
    

    #region "Car physics parameters"
        [SerializeField] private float motorForce;
        [SerializeField] private float breakForce;
        [SerializeField] private float maxSteerAngle;
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor() {
        // The car is front wheels driven so apply motorForce only on front wheels
        for (int i = 0; i < 2; i++) 
        {
            wheelColliders[i].motorTorque = motorForce;
        }
        
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking) 
        {
            ApplyBreaking();
        }

    }

    private void ApplyBreaking() 
    {
        for (int i = 0; i < wheelColliders.Length; i++) 
        {
            wheelColliders[i].brakeTorque = currentBreakForce;
        }
    }

    private void HandleSteering() 
    {
        
        Vector3 directionTowardsTarget = (target.position - transform.position).normalized;
        float distanceTowardsTarget = Vector3.Distance(target.position, transform.position);
        float angleTowardsTarget = Vector3.SignedAngle(transform.forward, directionTowardsTarget, Vector3.up);
        currentSteerAngle = Mathf.Clamp(angleTowardsTarget, -maxSteerAngle, maxSteerAngle);
        // Debug.Log(directionTowardsTarget);
        currentSteerAngle = (Mathf.Abs(currentSteerAngle) < 20 && distanceTowardsTarget < 20) ? 0 : currentSteerAngle;

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
