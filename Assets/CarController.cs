using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public WheelCollider[] wheelColliders; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;
    public GameObject[] wheelMeshes; // 0 - Front Left; 1 - Front Right; 2 - Rear Left; 3 - Rear Right;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {

            WheelCollider wheelCollider = wheelColliders[i];
            GameObject wheelMesh = wheelMeshes[i];
            Vector3 wheelPosition;
            Quaternion wheelRotation;
            wheelCollider.GetWorldPose(out wheelPosition, out wheelRotation);

            // Set the wheel mesh's position and rotation
            wheelMesh.transform.position = wheelPosition;
            wheelMesh.transform.rotation = wheelRotation;

        }
        
    }
}
