using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBalloon : MonoBehaviour, IDamageTaker
{
    [SerializeField] private float explosionForce = 1000000f;
    [SerializeField] private float explosionRadius = 500000f;

    public bool _isActive = true; 
    
    private void OnCollisionEnter(Collision collision) {
        TakeDamage();
    }

    public bool isActive()
    {
        return _isActive;
    }

    public void TakeDamage()
    {
        if (_isActive)
        {
            Explode();
            _isActive = false;
        }
        // gameObject.SetActive(false);
        // Destroy(gameObject);
    }

    Vector3 spherePosition;
    private void Explode()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
        foreach (Collider hit in colliders) 
        {
            Rigidbody hit_rb = hit.GetComponent<Rigidbody>();
            if (hit_rb != null) 
            {
                hit_rb.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
            }
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
