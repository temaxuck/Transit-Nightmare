using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomFov = 20.0f;
    public float zoomTime = 1.0f;
    private Camera cam;
    private float defualtFov;
    Coroutine zoomCoroutine;

    private void Start()
    {
        cam = Camera.main;
        defualtFov = cam.fieldOfView;
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Stop old coroutine
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            //Start new coroutine and zoom within 1 second
            zoomCoroutine = StartCoroutine(lerpFieldOfView(cam, zoomFov, zoomTime));
        }

        if (Input.GetMouseButtonUp(1))
        {
            //Stop old coroutine
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            //Start new coroutine and zoom within 1 second
            zoomCoroutine = StartCoroutine(lerpFieldOfView(cam, defualtFov, zoomTime));
        }

    }


    IEnumerator lerpFieldOfView(Camera targetCamera, float toFOV, float duration)
    {
        float counter = 0;

        float fromFOV = targetCamera.fieldOfView;

        while (counter < duration)
        {
            counter += Time.deltaTime;

            float fOVTime = counter / duration;

            //Change FOV
            targetCamera.fieldOfView = Mathf.Lerp(fromFOV, toFOV, fOVTime);
            //Wait for a frame
            yield return null;
        }
    }
}
