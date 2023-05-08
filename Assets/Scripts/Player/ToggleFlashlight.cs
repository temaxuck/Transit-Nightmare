using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFlashlight : MonoBehaviour
{
    public Light flashlight;
    private bool isOn;

    void Start()
    {
        // Ensure that flashlight is initially turned off
        flashlight.enabled = false;
        isOn = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    private void ToggleLight()
    {
        isOn = !isOn;
        flashlight.enabled = isOn;
    }
}
