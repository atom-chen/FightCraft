﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRise : StateBase
{

    protected override string GetAnimName()
    {
        return "Act_Rise_01";
    }

    public override void InitState(MotionManager motionManager)
    {
        base.InitState(motionManager);

        _MotionManager.AddAnimationEndEvent(_Animation);
    }

    public override void StateOpt(MotionOpt opt, params object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Act_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Input_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Hit:
                _MotionManager.TryEnterState(_MotionManager._StateHit, args);
                break;
            case MotionOpt.Fly:
                _MotionManager.TryEnterState(_MotionManager._StateFly, args);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            case MotionOpt.Anim_Event:
                DispatchRiseEvent(args[0] as string, args[1]);
                break;
            default:
                break;
        }
    }

    #region 

    private void DispatchRiseEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.ANIMATION_END:
                _MotionManager.TryEnterState(_MotionManager._StateIdle);
                break;
        }
    }

    #endregion
}