using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameUIManager : MonoBehaviour
{
    [Header("End Game UI")]
 
    public Button restartButton;
    public Button homeButton;
    private Button[] buttons;
    private int selectedButtonIndex = 0;
 

    // Start is called before the first frame update
    void Start()
    {
 
        restartButton.onClick.AddListener(OnRestartButtonClick);
        homeButton.onClick.AddListener(OnHomeButtonClick);
        SelectPreviousButton();
        buttons = new Button[] { restartButton, homeButton };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousButton();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextButton();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {

            SelectButton(selectedButtonIndex);
        }
    }
    void SelectNextButton()
    {
        selectedButtonIndex++;
        if (selectedButtonIndex >= buttons.Length)
        {
            selectedButtonIndex = 0;
        }
        UpdateButtonSelection();
    }

    void SelectPreviousButton()
    {
        selectedButtonIndex--;
        if (selectedButtonIndex < 0)
        {
            selectedButtonIndex = buttons.Length - 1;
        }
        UpdateButtonSelection();
    }

    void UpdateButtonSelection()
    {

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == selectedButtonIndex)
            {
                buttons[i].Select();

            }
            else
            {
                buttons[i].OnDeselect(null);
            }
        }
    }
    void SelectButton(int index)
    {

        if (index >= 0 && index < buttons.Length)
        {
            buttons[index].onClick.Invoke();
        }
    }

    public void OnRestartButtonClick()
    {

        SceneLoader.Instance.LoadMainScene();

    }

    public void OnHomeButtonClick()
    {
        SceneLoader.Instance.LoadMenuScene();

    }
}
