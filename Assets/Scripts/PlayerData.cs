using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public int level;
    [HideInInspector] public int pastTime;
    [HideInInspector] public int currentTime;


    public void ResetData()
    {
        pastTime = 0;
        level = 0;
        currentTime = 0;
    }

    public void NextLevel(){
        level += 1;
        pastTime += currentTime;
        currentTime = 0;
    }
}
