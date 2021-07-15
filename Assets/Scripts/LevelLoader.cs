using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public GameObject loadingScreen;
    public GameObject sliderob;
    public Slider slider;
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(1));
    }
    IEnumerator LoadLevel(int sceneIndex)
    {
        transition.SetTrigger("Start");
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(false);
        sliderob.SetActive(true);
        //Time.timeScale = 0f;
        //Debug.Log(operation.isDone);
        //Debug.Log(operation.progress);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
            //Debug.Log(operation.progress);
        }
        //SceneManager.LoadScene(1);
    }
}