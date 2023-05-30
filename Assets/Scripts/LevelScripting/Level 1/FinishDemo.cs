using UnityEngine;

public class FinishDemo : MonoBehaviour
{
    [SerializeField] private Level1 levelScript;
    private bool isActive = true;
    
    private void OnTriggerEnter(Collider collider)
    {
        levelScript.TriggerFinalScene();
    }


}
