using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private float fakeLoadDelay = 0.5f;

    private AsyncOperation loadingOperation;
    private float progressValue;
    private float loadProgress;

    private void Start()
    {
        progressSlider.value = 0;
        progressText.text = "0%";
        
        string targetScene = PlayerPrefs.GetString("TargetScene", "MainScene");
        StartCoroutine(LoadSceneAsync(targetScene));
    }

    private System.Collections.IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return new WaitForSeconds(fakeLoadDelay);
        
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            loadProgress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            
            progressValue = Mathf.MoveTowards(progressValue, loadProgress, Time.deltaTime);
            progressSlider.value = progressValue;
            progressText.text = Mathf.RoundToInt(progressValue * 100) + "%";
            
            if (loadingOperation.progress >= 0.9f)
            {
                loadingOperation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
}