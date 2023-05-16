using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public class CarMovement : MonoBehaviour
{
    #region Wheel objects variables
        [SerializeField] private WheelCollider[] wheelColliders; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
        [SerializeField] private Transform[] wheelMeshes; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
    #endregion

    private void Update() {
        UpdateWheelsPositions();
    }

    public void ApplyMotorForce(float motorTorque) 
    {
        // The car is front wheels driven so apply motorTorque only on first 2 wheels
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].motorTorque = motorTorque;
        }
    }

    public void ApplySteering(float steerAngle) 
    {
        // Apply steering only on front wheels
        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = steerAngle;
        }
    }

    public void ApplyBraking(float brakeForce) 
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].brakeTorque = brakeForce;
        }
    }

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

}
