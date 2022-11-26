using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    Action<Vector3> OnMovementDirectionInput {get;set;}
    Action<Vector2> OnMovementInput {get;set;}
}
