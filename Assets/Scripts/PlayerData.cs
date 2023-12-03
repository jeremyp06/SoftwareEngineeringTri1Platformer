using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public int level = 0;
    [HideInInspector] public float pastTime = 0;
    [HideInInspector] public float currentTime = 0;

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

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

    public void RestartTimer(){
        currentTime = 0;
    }
}