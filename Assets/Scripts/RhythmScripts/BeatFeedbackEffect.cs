using UnityEngine;
using TMPro;

public class BeatFeedbackEffect : MonoBehaviour
{
    public float moveUpDistance = 30;
    public float fadeSpeed = 1.0f;

    private TextMeshProUGUI textComponent;
    private float alpha = 1.0f;
    private Vector3 startPosition;

    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Di chuyển lên trên
        transform.position += Vector3.up * moveUpDistance * Time.deltaTime;

        // Mờ dần
        alpha -= fadeSpeed * Time.deltaTime;
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, alpha);

        // Khi mờ hoàn toàn thì hủy GameObject
        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}