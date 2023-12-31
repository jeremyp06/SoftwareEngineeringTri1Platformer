using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Second Level Movement Option", menuName = "Movement Option/Second Level")]
public class SecondLevelMovementOption : MovementOption
{
    public override bool CanJump()
    {
        return true;
    }

    public override bool CanWallJump()
    {
        return true;
    }
}
