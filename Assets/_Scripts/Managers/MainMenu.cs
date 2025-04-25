using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private string mainSceneName = "MainScene";

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        PlayerPrefs.SetString("TargetScene", mainSceneName);   
        SceneManager.LoadScene(loadingSceneName);
    }

}