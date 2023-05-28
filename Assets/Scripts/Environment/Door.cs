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
        if (!isOpen) 
            pickupIcon.SetActive(isVisible);
    }

    private bool CheckKey()
    {
        return (doorKey == null) ? true : KeyInventory.instance.HasKey(doorKey);
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
