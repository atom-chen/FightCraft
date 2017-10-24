﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillDefence : ObjMotionSkillBase
{
    public float _MinDefenceTime = 0.5f;
    public ImpactBuffDefence _BuffDefence;

    private float _DefenceTime = 0;

    void Update()
    {
        _DefenceTime -= Time.deltaTime;
        if (_DefenceTime <= 0 && !InputManager.Instance.IsKeyHold("7"))
        {
            PlayerNextAnim();
        }
    }

    public override bool ActSkill(Hashtable exhash)
    {
        var actSkill = base.ActSkill(exhash);
        _DefenceTime = _MinDefenceTime;
        _BuffDefence.ActImpact(MotionManager, MotionManager);
        return actSkill;
    }

    public override void FinishSkillImmediately()
    {
        base.FinishSkillImmediately();

        MotionManager.RemoveBuff(_BuffDefence.GetType());
    }

    public override void AnimEvent(string function, object param)
    {
        switch (function)
        {
            //case AnimEventManager.ANIMATION_END:
            //    PlayerNextAnim();
            //    break;
            case AnimEventManager.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimEventManager.COLLIDER_END:
                ColliderEnd(param);
                break;
        }
    }
}