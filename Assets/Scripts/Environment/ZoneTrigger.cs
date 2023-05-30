using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject pictureOrModel; // Drag and drop your 3D model or Picture(prefab with sprite renderer) in the inspector.

    [SerializeField]
    private AudioClip soundClip; // Drag and drop your sound clip in the inspector.

    [SerializeField]
    private Light[] roomLight;

    [SerializeField]
    private float modelDistanceBehindPlayer = 10.0f;

    [SerializeField]
    private float delayOfDisappearance = 2.0f;

    [SerializeField]
    private float displayDuration = 15f; // duration that the picture or 3d model is visible, change according to your needs.

    [SerializeField]
    private float blinkDuration = 0.25f;

    [SerializeField]
    private int numBlinks = 3;

    private AudioSource audioSource;
    private bool isTriggered;
    private bool playerScream = false;
    private Transform playerTransform;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundClip;

        pictureOrModel.SetActive(false); // Start with the model or picture invisible.
    }

    private void Update()
    {
        if (playerScream)
        {
            bool modelInSight = CheckModelInSight();
            if (modelInSight)
            {
                playerScream = false;
                StopCoroutine(DisplayModelRoutine());
                StartCoroutine(PlayerSawDelayRoutine());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            Vector3 modelPosition =
                playerTransform.position - playerTransform.forward * modelDistanceBehindPlayer;
            modelPosition.y = playerTransform.position.y - playerTransform.localScale.y / 2f;
            pictureOrModel.transform.position = modelPosition;
            if (roomLight != null)
                foreach (var light in roomLight)
                {
                    if (light != null)
                        light.enabled = false;
                }
            pictureOrModel.SetActive(true); // Make the picture or model visible.
            audioSource.Play(); // Play the sound.
            StartCoroutine(DisplayModelRoutine());
            isTriggered = true;
            playerScream = true;
        }
    }

    private bool CheckModelInSight()
    {
        Vector3 viewDirection = (
            pictureOrModel.transform.position - playerTransform.position
        ).normalized;
        float dotProduct = Vector3.Dot(playerTransform.forward, viewDirection);

        // Change this value to set the angle the player has to look at the model for it to disappear
        float requiredDotProduct = Mathf.Cos(45.0f * 0.5f * Mathf.Deg2Rad);

        return dotProduct >= requiredDotProduct;
    }

    private IEnumerator DisplayModelRoutine()
    {
        yield return new WaitForSeconds(displayDuration);

        pictureOrModel.SetActive(false); // Hide the picture or model after the desired time has passed.
        StartCoroutine(BlinkAndEnableLight());
    }

    private IEnumerator PlayerSawDelayRoutine()
    {
        yield return new WaitForSeconds(delayOfDisappearance);

        pictureOrModel.SetActive(false); // Hide the picture or model after the desired time has passed.
        StartCoroutine(BlinkAndEnableLight());
    }

    private IEnumerator BlinkAndEnableLight()
    {
        for (int i = 0; i < numBlinks; i++)
        {
            foreach (var light in roomLight)
            {
                light.enabled = !light.enabled;
            }
            yield return new WaitForSeconds(blinkDuration);
        }

        foreach (var light in roomLight)
        {
            light.enabled = true;
        }
    }
}
