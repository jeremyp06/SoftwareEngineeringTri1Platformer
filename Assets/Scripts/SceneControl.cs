using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

    public float delay;
    public PlayerData playerData;

    public void RestartSceneWithDelay()
    {
        StartCoroutine(RestartWithDelay(delay));
    }

    private IEnumerator RestartWithDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (playerData.level == 1){
            playerData.RestartTimer();
        }
        
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void NextSceneWithDelay()
    {
        StartCoroutine(NextSceneWithDelay(delay));
    }

    private IEnumerator NextSceneWithDelay(float delay)
    {
        // Wait for the specified delay
        playerData.NextLevel();

        yield return new WaitForSeconds(delay);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        playerData.RestartTimer();

        SceneManager.LoadScene(currentSceneIndex+1);
    }

    public void ResetPlayerData(){
        playerData.ResetData();
    }
}