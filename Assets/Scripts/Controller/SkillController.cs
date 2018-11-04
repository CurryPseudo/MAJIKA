﻿using UnityEngine;
using System.Collections;

public class SkillController : EntityBehaviour<Player>
{
    public const string EventSkillEnd = "SkillEnd";
    public Skill[] Skills;
    public Skill ActiveSkill = null;

    private Coroutine movementCoroutine;

    void Update()
    {
        Skills = GetComponentsInChildren<Skill>();
    }

    public bool Activate(int idx)
    {
        if (idx >= Skills.Length)
            return false;
        if (ActiveSkill && !ActiveSkill.Abort())
            return false;
        if (!Skills[idx].Activate())
            return false;
        ActiveSkill = Skills[idx];
        return true;
    }

    public bool IsLocked()
    {
        if (!ActiveSkill)
            return false;
        return ActiveSkill.Locked;
    }

    public bool Abort()
    {
        if (!ActiveSkill)
            return true;
        return ActiveSkill.Abort();
    }

    void OnReleaseLock()
    {
        if (!ActiveSkill)
            return;
        ActiveSkill.Locked = false;
    }

    void OnSkillEnd()
    {
        OnReleaseLock();
        GetComponent<EventBus>().Dispatch(EventSkillEnd);
        ActiveSkill = null;
        movementCoroutine = null;
    }

    void OnImpactStart()
    {
        ActiveSkill?.StartImpact();
    }

    void OnImpactEnd()
    {
        ActiveSkill?.EndImpact();
    }

    IEnumerator MoveCoroutine(MovementSkill movement)
    {
        var endTime = Time.time + movement.Duration;
        var dir = movement.Direction;
        dir.x *= GetComponent<MovableEntity>().FaceDirection;
        while (Time.time <= endTime)
        {
            yield return null;
            GetComponent<MovableEntity>().ForceMove(dir.normalized * movement.Speed);
        }
    }

    void onSkillMovementStart()
    {
        if (!this.ActiveSkill)
            return;
        var movement = ActiveSkill.GetComponent<MovementSkill>();
        if (!movement)
            return;
        movementCoroutine = StartCoroutine(MoveCoroutine(movement));
    }

    void onSkillMovementEnd()
    {
        StopCoroutine(movementCoroutine);
        movementCoroutine = null;
    }
}
