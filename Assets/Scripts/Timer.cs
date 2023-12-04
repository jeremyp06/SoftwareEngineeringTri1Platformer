using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public PlayerData playerData;

    private void Update()
    {

        if (timerText != null)
        {
            float t = playerData.currentTime + playerData.pastTime;
            if (t <= 999){
                timerText.text = (playerData.currentTime + playerData.pastTime).ToString("F0");
            } else {
                timerText.text = "999+";
            }
        }
    }
}
