using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorKey;
    public GameObject pickupIcon;
    public GameObject messageIcon;
    private Animator animator;
    private bool isOpen = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetIconVisibility(false);
    }

    public void SetIconVisibility(bool isVisible)
    {
        pickupIcon.SetActive(false);
        messageIcon.SetActive(false);

        if (!isOpen)
            if (CheckKey()) 
                pickupIcon.SetActive(isVisible);
            else
                messageIcon.SetActive(isVisible);
    }

    public bool CheckKey()
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
