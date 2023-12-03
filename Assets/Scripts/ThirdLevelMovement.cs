using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Third Level Movement Option", menuName = "Movement Option/Third Level")]
public class ThirdLevelMovementOption : MovementOption
{
    public override bool CanJump()
    {
        return true;
    }

    public override bool CanWallJump()
    {
        return true;
    }

    public override bool CanDash()
    {
        return true;
    }
}
