using UnityEngine;

public class ButtonController : MonoBehaviour {
    [SerializeField] private TextButton[] buttons;
    public int focusedButtonIndex = 0;

    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            ChangeFocus(-1);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            ChangeFocus(1);
        } else if (Input.GetKeyDown(KeyCode.Return))
        {
            ActivateButton();
        }
    }

    private void ChangeFocus(int direction) {
        buttons[focusedButtonIndex].OnPointerExit(null);
        focusedButtonIndex = (focusedButtonIndex + direction + buttons.Length) % buttons.Length;
        buttons[focusedButtonIndex].OnPointerEnter(null);
    }

    private void ActivateButton() {
        buttons[focusedButtonIndex].OnPointerClick(null);
    }
}
