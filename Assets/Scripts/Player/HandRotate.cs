using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandRotate : MonoBehaviour
{
    [SerializeField]private GameObject player;
    void Update()
    {
        transform.Rotate(player.transform.eulerAngles);
    }
}
