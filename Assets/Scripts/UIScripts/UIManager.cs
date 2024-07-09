using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    [Header("Rhythm Bar")]
    public GameObject rhythmBar;
    [Header("Item Pickup")]
    public GameObject pickedUpItemPanel;
    public Text itemText;
    public Image itemImage;
    public float displayTime = 2f;
    public float yOffset = 50f;
    public CanvasGroup canvasGroup;
    [Header("End Game UI")]
    public GameObject endGameUI;
 

    void Start()
    {
        pickedUpItemPanel.SetActive(false); 
        canvasGroup = endGameUI.GetComponent<CanvasGroup>();
        
        canvasGroup.alpha = 0f; 
        endGameUI.SetActive(false);
    
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void ShowEndGame()
    {
        endGameUI.SetActive(true);

        canvasGroup.DOFade(1f, 1f);
    }
    
    public void ShowRhythmBar()
    {
        rhythmBar.SetActive(true);
    }
    public void CloseRhythmBar()
    {
        rhythmBar.SetActive(false);
    }
    public void ShowItemPickupMessage(ItemData itemData)
    {
        pickedUpItemPanel.SetActive(true);
        itemText.text = itemData.itemName;
        itemImage.sprite = itemData.sprite;

        RectTransform panelRectTransform = pickedUpItemPanel.GetComponent<RectTransform>();

        panelRectTransform.anchoredPosition = new Vector2(-Screen.width, yOffset);

        panelRectTransform.DOAnchorPos(new Vector2(0, yOffset), 0.5f).SetEase(Ease.OutBack,1).OnComplete(() =>
        {
            DOVirtual.DelayedCall(displayTime, () =>
            {
                panelRectTransform.DOAnchorPos(new Vector2(Screen.width, yOffset), 0.5f).SetEase(Ease.InBack, 1).OnComplete(() =>
                {
                    pickedUpItemPanel.SetActive(false);
                });
            });
        });

    }
}
