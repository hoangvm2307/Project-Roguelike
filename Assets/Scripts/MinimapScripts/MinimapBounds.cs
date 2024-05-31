using UnityEngine;

public class MinimapBounds : MonoBehaviour
{
    public RectTransform minimapRect;
    public RectTransform boundsRect;

    void Update()
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(boundsRect, minimapRect.position))
        {
            minimapRect.gameObject.SetActive(false);
        }
        else
        {
            minimapRect.gameObject.SetActive(true);
        }
    }
}
