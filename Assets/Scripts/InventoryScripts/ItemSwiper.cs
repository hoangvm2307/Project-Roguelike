using System.Collections;
using UnityEngine;

public class ItemSwiper : MonoBehaviour
{
    public float swipeSpeed = 1.0f;
    public float maxDarkenAmount = 0.5f;
    private int currentIndex = 0;
    private int itemCount;
    private float itemWidth;
    private ItemData currentItem;

    private void OnEnable()
    {
        //InventoryManager.Instance.OnChangedItem += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        //InventoryManager.Instance.OnChangedItem -= Refresh;
    }

    public void Refresh()
    {

        itemCount = transform.childCount;
        //Debug.Log("REFRESH");
        if (itemCount == 0) return;

        itemWidth = transform.GetChild(0).GetComponent<Renderer>().bounds.size.x * 2;

        // Set initial positions
        for (int i = 0; i < itemCount; i++)
        {
            float xPos = (i - currentIndex) * itemWidth;
            transform.GetChild(i).localPosition = new Vector3(xPos, 1, 0);
        }
        UpdateContentPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwipeToNextItem();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwipeToPreviousItem();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InventoryManager.Instance.SetCurrentItem(currentItem);
            Debug.Log("Current item: " + currentItem.name);
        }
    }

    void SwipeToNextItem()
    {
        currentIndex = (currentIndex + 1) % itemCount;
        UpdateContentPosition();
    }

    void SwipeToPreviousItem()
    {
        currentIndex = (currentIndex - 1 + itemCount) % itemCount;
        UpdateContentPosition();
    }

    private void UpdateContentPosition()
    {
        if (itemCount == 0) return;

        for (int i = 0; i < itemCount; i++)
        {
            int relativeIndex = (i - currentIndex + itemCount) % itemCount;
            if (itemCount > 2)
            {
                relativeIndex -= itemCount / 2;
            }

            float xPos = relativeIndex * itemWidth;
            float distanceToCenter = Mathf.Abs(relativeIndex);

            float darkenAmount = 0f;
            if (distanceToCenter == 2)
            {
                darkenAmount = maxDarkenAmount;
            }
            else if (distanceToCenter == 1)
            {
                darkenAmount = maxDarkenAmount * 0.75f;
            }

            if (relativeIndex == 0)
            {
                currentItem = transform.GetChild(i).GetComponent<ItemComponent>().itemData;
            }

            StartCoroutine(SmoothMove(transform.GetChild(i), xPos, darkenAmount));
        }


    }

    IEnumerator SmoothMove(Transform item, float targetX, float darkenAmount)
    {
        float startX = item.localPosition.x;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime * swipeSpeed;
            float newX = Mathf.Lerp(startX, targetX, t);
            item.localPosition = new Vector3(newX, item.localPosition.y, item.localPosition.z);
            // Darken the item based on distance to center
            Color itemColor = item.GetComponent<Renderer>().material.color;
            itemColor.a = 1.0f - darkenAmount;
            item.GetComponent<Renderer>().material.color = itemColor;
            yield return null;
        }
    }

}
