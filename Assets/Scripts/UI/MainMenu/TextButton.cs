using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler 
{
    [SerializeField] private float focusedFontSize = 192f;
    [SerializeField] private float normalFontSize = 84f;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private ClickEvent clickEvent = ClickEvent.None;

    private TextMeshProUGUI text;
    private float targetFontSize;
    private float animationStartTime;

    void Start() {
        text = GetComponent<TextMeshProUGUI>();
        targetFontSize = normalFontSize;
        text.fontSize = normalFontSize;
    }

    void Update() {
        float t = (Time.time - animationStartTime) / animationDuration;
        text.fontSize = Mathf.Lerp(text.fontSize, targetFontSize, t);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        targetFontSize = focusedFontSize;
        animationStartTime = Time.time;
    }

    public void OnPointerExit(PointerEventData eventData) {
        targetFontSize = normalFontSize;
        animationStartTime = Time.time;
    }

    public void OnPointerClick(PointerEventData eventData) {
        ClickEventHandlers.Get(clickEvent).Activate();
    }
}


