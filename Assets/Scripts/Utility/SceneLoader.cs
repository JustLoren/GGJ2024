using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public FadeToBlack curtain;

    // Method to load scene by name asynchronously
    public void LoadSceneByNameAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Method to load scene by build index asynchronously
    public void LoadSceneByIndexAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        asyncLoad.allowSceneActivation = false;

        // Optionally, you can use asyncLoad.progress to get the progress of the loading
        while (asyncLoad.progress < .9f || !curtain.fadeComplete)
        {
            // Here you can add code to show loading progress if desired
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        asyncLoad.allowSceneActivation = false;

        // Optionally, you can use asyncLoad.progress to get the progress of the loading
        while (asyncLoad.progress < .9f || !curtain.fadeComplete)
        {
            // Here you can add code to show loading progress if desired
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}
