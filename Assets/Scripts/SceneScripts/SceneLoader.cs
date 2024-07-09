using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if(GetCurrentSceneName() == "Menu")
        {
            if (Input.anyKeyDown)
            {
                SceneLoader.Instance.LoadMainScene();
            }
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMainScene()
    {
        LoadScene("Main");  
    }

    public void LoadMenuScene()
    {
        LoadScene("Menu");  
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
