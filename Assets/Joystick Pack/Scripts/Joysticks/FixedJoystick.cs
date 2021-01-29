using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    public float horizontal;
    private void Update()
    {
        horizontal = base.Horizontal;
    }

}