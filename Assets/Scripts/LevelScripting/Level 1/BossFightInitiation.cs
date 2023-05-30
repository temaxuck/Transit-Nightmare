using UnityEngine;

public class BossFightInitiation : MonoBehaviour
{
    [SerializeField] private Level1 levelScript;
    private bool isActive = true;
    
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
            if (isActive)
            {
                isActive = false;
                levelScript.TriggerBossFight();
            }
    }


}
