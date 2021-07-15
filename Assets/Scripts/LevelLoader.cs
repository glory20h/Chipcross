using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public GameObject loadingScreen;
    [SerializeField]Image progressBar;
    [SerializeField]Image progressBarframe;
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(1));
    }
    IEnumerator LoadLevel(int sceneIndex)
    {
        yield return null;
        progressBar.enabled = true;
        progressBarframe.enabled = true;
        transition.SetTrigger("Start");
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);
        op.allowSceneActivation = false;

        loadingScreen.SetActive(false);
        float timer = 0.0f; 
        while (!op.isDone)
        {
            yield return null; 
            timer += Time.deltaTime;
            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if(progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if(progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true; 
                    yield break;
                }
            }
        }

    }
}