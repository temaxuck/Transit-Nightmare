using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour {
    [SerializeField] private Button[] buttons;
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
        buttons[focusedButtonIndex].OnDeselect(null);
        focusedButtonIndex = (focusedButtonIndex + direction + buttons.Length) % buttons.Length;
        buttons[focusedButtonIndex].Select();
    }

    private void ActivateButton() {
        buttons[focusedButtonIndex].onClick.Invoke();
    }
}
