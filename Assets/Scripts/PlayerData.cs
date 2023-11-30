using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [HideInInspector] public Vector3 currentPosition;
    [HideInInspector] public int level;

    public void ResetData()
    {
        currentPosition = Vector3.zero;
        level = 0;
    }
}
