﻿using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Input;

public class InputManager : Singleton<InputManager>
{
    [Tooltip("Input action assets")]
    public MAJIKAInput Controller;

    public MAJIKAInput.ActionsActions Actions => Controller.Actions;

    public MAJIKAInput.GamePlayActions GamePlay => Controller.GamePlay;
}
