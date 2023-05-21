using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour, IDamageTaker
{
    [SerializeField] private float explosionForce = 100f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private GameObject damagedPrefab;


    public bool _isActive = true; 
    
    public bool isActive()
    {
        return _isActive;
    }

    public void TakeDamage()
    {
        if (_isActive)
        {
            Explode();
            ReplacePrefab();
            _isActive = false;
        }
        // gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    private void Explode()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders) 
        {
            Rigidbody hit_rb = hit.GetComponent<Rigidbody>();
            if (hit_rb != null && !hit.gameObject.CompareTag("Player")) 
            {
                Debug.Log(hit_rb.gameObject);
                hit_rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
        }
    }

    private void ReplacePrefab()
    {
        if (damagedPrefab != null)
        {
            GameObject damagedObject = Instantiate(damagedPrefab, transform.position, transform.rotation);
            gameObject.SetActive(false);
            Destroy(gameObject);   

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        // if (spherePosition != null)
        // {

        //     // Gizmos.DrawSphere(spherePosition, 5);
        // }
    }

}
