using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is missing on the GameObject.");
        }

        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup is missing on the GameObject.");
        }

        if (canvas == null)
        {
            Debug.LogError("Canvas is missing in parent hierarchy.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // Store the original parent
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
        {
            float scaleFactor = canvas.scaleFactor;
            rectTransform.anchoredPosition += eventData.delta / scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;

        // Check if the item is dropped in a valid drop area
        if (transform.parent == originalParent)
        {
            // If the parent is still the same, revert the position
            transform.localPosition = Vector3.zero;
        }
    }
}
