using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBalloon : MonoBehaviour, IDamageTaker
{
    bool _isActive = true;
    
    public bool isActive()
    {
        return _isActive;
    }
    
    public void TakeDamage()
    {
        // Exlode();
        // _isActive = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

}
