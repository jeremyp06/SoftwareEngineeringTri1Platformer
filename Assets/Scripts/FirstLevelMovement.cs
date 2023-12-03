using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "First Level Movement Option", menuName = "Movement Option/First Level")]
public class FirstLevelMovementOption : MovementOption
{
    public override bool CanJump()
    {
        return true;
    }
}


