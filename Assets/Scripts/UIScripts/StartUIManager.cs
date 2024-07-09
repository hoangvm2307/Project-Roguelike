using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartUIManager : MonoBehaviour
{
    public Text pressAnyButtonText;
    public float blinkDuration = 0.5f;

    [Header("Sword")]
    public float moveDistance = 20f;
    public float moveDuration = 1f;
    public GameObject swordUI;

    void Start()
    {
        StartMoving();
        if (pressAnyButtonText == null)
        {
            pressAnyButtonText = GetComponent<Text>();
        }


        StartBlinking();
    }

    void StartBlinking()
    {

        pressAnyButtonText.DOFade(0, blinkDuration)
            .SetLoops(-1, LoopType.Yoyo);
    }

    void StartMoving()
    {
        Vector3 originPos = swordUI.transform.position;
        swordUI.transform.DOMoveY(originPos.y + moveDistance, moveDuration).SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
