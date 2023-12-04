using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinText : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI timerText; 
    public PlayerData playerData;

    private void Update()
    {

        if (timerText != null)
        {
            float t = playerData.pastTime;
            if (t <= 999){
                timerText.text = "Final time: " + (playerData.pastTime).ToString("F0");
            } else {
                timerText.text = ("Final time: 999+");
            }
        }
    }


}
