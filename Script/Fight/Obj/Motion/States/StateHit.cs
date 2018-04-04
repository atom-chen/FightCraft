using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHit : StateBase
{
    protected override string GetAnimName()
    {
        return "Act_Hit_01";
    }

    public override void InitState(MotionManager motionManager)
    {
        base.InitState(motionManager);

        _MotionManager.AddAnimationEndEvent(_Animation);
    }

    public override bool CanStartState(params object[] args)
    {
        return IsBuffCanBeHit((MotionManager)args[2], (ImpactHit)args[3]);
    }

    public override void StartState(params object[] args)
    {
        //base.StartState(args);
        if (args.Length > 6)
        {
            if ((bool)args[6])
            {
                _MotionManager.PauseAnimation();
            }
        }
        else
        {
            MotionHit((float)args[0], (int)args[1], (MotionManager)args[2]);
            SetHitMove((Vector3)args[4], (float)args[5]);
        }

        if (_MotionManager._BehitAudio != null)
        {
            _MotionManager.PlayAudio(_MotionManager._BehitAudio);
        }
    }

    public override void StateOpt(MotionOpt opt, params object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Pause_State:
                _MotionManager.PauseAnimation(_Animation, (float)args[0]);
                break;
            case MotionOpt.Act_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Input_Skill:
                _MotionManager.TryEnterState(_MotionManager._StateSkill, args);
                break;
            case MotionOpt.Hit:
                //_MotionManager.TryEnterState(_MotionManager._StateHit, args);
                MotionHit((float)args[0], (int)args[1], (MotionManager)args[2]);
                SetHitMove((Vector3)args[4], (float)args[5]);
                break;
            case MotionOpt.Fly:
                _MotionManager.TryEnterState(_MotionManager._StateFly, args);
                break;
            case MotionOpt.Catch:
                _MotionManager.TryEnterState(_MotionManager._StateCatch, args);
                break;
            case MotionOpt.Anim_Event:
                DispatchHitEvent(args[0] as string, args[1]);
                break;

            default:
                break;
        }
    }

    #region 

    Hashtable _BuffArg = new Hashtable();
    public bool IsBuffCanBeHit(MotionManager impactSender, ImpactHit impactHit)
    {
        _BuffArg.Clear();
        _BuffArg.Add(ImpactBuff.BuffModifyType.IsCanHit, true);
        _MotionManager.ForeachBuffModify(ImpactBuff.BuffModifyType.IsCanHit, _BuffArg, impactSender, impactHit);
        return (bool)_BuffArg[ImpactBuff.BuffModifyType.IsCanHit];
    }

    private float _StopKeyFrameTime = 0.0f;
    private void DispatchHitEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimEventManager.KEY_FRAME:
                HitKeyframe(param);
                break;
            case AnimEventManager.ANIMATION_END:
                _MotionManager.TryEnterState(_MotionManager._StateIdle);
                break;
        }
    }

    public void SetHitMove(Vector3 moveDirect, float moveTime)
    {
        if (moveTime <= 0)
            return;

        _MotionManager.SetMove(moveDirect, moveTime);
    }

    public void MotionHit(float hitTime, int hitEffect, MotionManager impactSender)
    {
        _MotionManager.PlayHitEffect(impactSender, hitEffect);
        if (hitTime <= 0)
            return;

        float speed = 1;
        if (hitTime > _Animation.length)
        {
            _StopKeyFrameTime = hitTime - _Animation.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
            speed = (_Animation.length / hitTime);
        }

        _MotionManager.StopCoroutine("ComsumeAnim");
        _MotionManager.RePlayAnimation(_Animation, speed);
        //_MotionManager.SetLookAt(impactSender.transform.position);
    }

    public void HitKeyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_Animation, -1);
            _MotionManager.StartCoroutine(ComsumeAnim());
        }
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);

        _MotionManager.ResumeAnimation(_Animation);
    }

    #endregion
}
