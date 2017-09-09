using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCatch : StateBase
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
    public override void StartState(params object[] args)
    {
        //base.StartState(args);
        MotionHit((float)args[0], (int)args[1], (MotionManager)args[2]);
        SetHitMove((Vector3)args[4], (float)args[5]);
    }


    public override void StateOpt(MotionOpt opt, params object[] args)
    {
        switch (opt)
        {
            case MotionOpt.Anim_Event:
                DispatchHitEvent(args[0] as string, args[1]);
                break;
            case MotionOpt.Stop_Catch:
                _MotionManager.TryEnterState(_MotionManager._StateIdle);
                break;
            default:
                break;
        }
    }

    #region MyRegion

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

    public void HitKeyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_Animation, -1);
            _MotionManager.StartCoroutine(ComsumeAnim());
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

        if (hitTime > _Animation.length)
        {
            _StopKeyFrameTime = hitTime - _Animation.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
        }

        _MotionManager.StopCoroutine("ComsumeAnim");
        _MotionManager.RePlayAnimation(_Animation, 1);
        //_MotionManager.SetLookAt(impactSender.transform.position);
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);

        _MotionManager.ResumeAnimation(_Animation);
    }

    #endregion
}
