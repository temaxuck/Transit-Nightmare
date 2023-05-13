using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetFlashlight : MonoBehaviour
{
    private Vector3 v3Offset;
    [SerializeField]private GameObject goFollow;
    [SerializeField]private float speed = 0.5f;
 
     void Start () {
         v3Offset = transform.position - goFollow.transform.position;
     }
     
     void Update () {
         transform.position = goFollow.transform.position + v3Offset;
         transform.rotation  = Quaternion.Slerp (transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
     }
 }
