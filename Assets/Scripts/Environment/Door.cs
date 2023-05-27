using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorKey;
    public GameObject pickupIcon;
    private Animator animator;
    private bool isOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetIconVisibility(false);
    }

    public void SetIconVisibility(bool isVisible)
    {
        pickupIcon.SetActive(isVisible);
    }

    private bool CheckKey()
    {
        return KeyInventory.instance.HasKey(doorKey);
    }

    public void OpenDoor()
    {
        if (!isOpen && CheckKey())
        {
            isOpen = true;
            animator.SetTrigger("Open");
        }
        
    }
}
