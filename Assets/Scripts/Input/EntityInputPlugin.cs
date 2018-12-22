﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="InputPlugin",menuName ="ControllerPlugin/Input")]
public class EntityInputPlugin : ControllerPlugin
{
    public override void OnUpdate(EntityController controller)
    {
        controller.Movement = InputManager.Instance.Movement;
        controller.Jumped = InputManager.Instance.Jumped;
        controller.Climbed = InputManager.Instance.Climbed;
        controller.SkillIndex = InputManager.Instance.GetSkillIndex();
    }
}
