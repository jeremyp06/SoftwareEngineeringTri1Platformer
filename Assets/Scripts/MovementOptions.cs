using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class MovementOption : ScriptableObject
{
    public virtual bool CanJump()
    {
        return false;
    }

    public virtual bool CanWallJump()
    {
        return false;
    }

    public virtual bool CanDash()
    {
        return false;
    }
}

[CreateAssetMenu(fileName = "First Level Movement Option", menuName = "Movement Option/First Level")]
public class FirstLevelMovementOption : MovementOption
{
    public override bool CanJump()
    {
        return true;
    }
}


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
