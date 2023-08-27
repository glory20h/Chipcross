using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public GameObject loadingScreen;
    public GameObject progressScreen;
    [SerializeField] Image progressBar;
    public GameObject Rocket;
    public GameObject Target;
    private const float lerpSpeedFactor = 0.1f; // 새로운 상수

    void Start()
    {
        // MANUALLY SET STARTING DIFFICULTYFACTOR BY CHANGING THIS VALUE
        // PlayerPrefs.SetFloat("PlayerDFactor", -1f);
        // PlayerPrefs.SetFloat("RateChange", 4f);

        // MANUALLY SET STARTING TUTLEVEL BY CHANGING THIS VALUE; DEFAULT 0 -> 평소에는 주석처리 되어 있어야함
        // PlayerPrefs.SetInt("tutorial", 1);
        InitializePlayerSettings();
    }

    private void InitializePlayerSettings()
    {
        // 이 부분에서 플레이어 설정을 초기화합니다.
    }

    public void LoadNextLevel()
    {
        string sceneName = (PlayerPrefs.GetInt("tutorial", 1) == 1) ? "Story" : "MainBoard";
        StartCoroutine(LoadLevel(sceneName));
    }

    IEnumerator LoadLevel(string sceneName)
    {
        transition.SetTrigger("Start");
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        ShowProgressScreen();

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            float progress = Mathf.Min(op.progress, 0.9f);
            UpdateProgressBar(progress, ref timer);

            if (IsLoadingComplete(op, progress))
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }

    private void ShowProgressScreen()
    {
        loadingScreen.SetActive(false);
        progressScreen.SetActive(true);
    }

    private void UpdateProgressBar(float progress, ref float timer)
    {
        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, timer * lerpSpeedFactor);
        Rocket.transform.position = Vector3.Lerp(Rocket.transform.position, Target.transform.position, progress);
        if (progressBar.fillAmount >= progress) timer = 0f;
    }

    private bool IsLoadingComplete(AsyncOperation op, float progress)
    {
        return progressBar.fillAmount >= 0.99f && progress >= 0.9f;
    }
}