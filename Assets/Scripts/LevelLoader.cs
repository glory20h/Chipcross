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

    void Start()
    {
        // MANUALLY SET STARTING DIFFICULTYFACTOR BY CHANGING THIS VALUE
        // PlayerPrefs.SetFloat("PlayerDFactor", -1f);
        // PlayerPrefs.SetFloat("RateChange", 4f);

        // MANUALLY SET STARTING TUTLEVEL BY CHANGING THIS VALUE; DEFAULT 0 -> 평소에는 주석처리 되어 있어야함
        // PlayerPrefs.SetInt("tutorial", 1);
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

        loadingScreen.SetActive(false);
        progressScreen.SetActive(true);

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;

            float progress = (op.progress < 0.9f) ? op.progress : 1f;
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, timer);
            Rocket.transform.position = Vector3.Lerp(Rocket.transform.position, Target.transform.position, op.progress);

            if (progressBar.fillAmount >= progress)
            {
                timer = 0f;
            }

            if (progressBar.fillAmount >= 0.99f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
    }
}
