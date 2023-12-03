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