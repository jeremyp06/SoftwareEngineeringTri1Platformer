using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

    public float delay;
    public void RestartSceneWithDelay()
    {
        StartCoroutine(RestartWithDelay(delay));
    }

    private IEnumerator RestartWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);
    }

    public void NextSceneWithDelay()
    {
        StartCoroutine(NextSceneWithDelay(delay));
    }

    private IEnumerator NextSceneWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex+1);
    }


}