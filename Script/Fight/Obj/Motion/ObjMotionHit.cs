using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionHit : ObjMotionBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        _MotionPriority = 1000;
    }

    public override void PlayMotion(object go, Hashtable eventArgs)
    {
        float hitTime = _AnimationClip.length;
        int hitEffect = 0;
        if (eventArgs.ContainsKey("HitTime"))
        {
            hitTime = (float)eventArgs["HitTime"];
        }

        if (eventArgs.ContainsKey("HitEffect"))
        {
            hitEffect = (int)eventArgs["HitEffect"];
        }
        _MotionManager.MotionStart(this);
        MotionManager impactSender = go as MotionManager;
        MotionHit(hitTime, hitEffect, impactSender);
    }

    public override bool ActiveInput(InputManager inputManager)
    {
        return base.ActiveInput(inputManager);
    }

    public override bool ContinueInput(InputManager inputManager)
    {
        return base.ContinueInput(inputManager);
    }

    protected override void InitEvent()
    {
        base.InitEvent();

        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, PlayMotion);
    }

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case "KeyFrame":
                Keyframe(param);
                break;
            case "AnimationEnd":
                AnimationEnd();
                break;
        }
    }
    #endregion

    public EffectController[] _HitEffect;

    private float _StopKeyFrameTime = 0.0f;

    public void MotionHit(float hitTime, int hitEffect, MotionManager impactSender)
    {
        PlayHitEffect(hitEffect, impactSender);

        if (hitTime > _AnimationClip.length)
        {
            _StopKeyFrameTime = hitTime - _AnimationClip.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
        }
        _MotionManager.RePlayAnimation(_AnimationClip);

    }

    public void Keyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_AnimationClip);
            StartCoroutine(ComsumeAnim());
        }
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);
        _MotionManager.ResumeAnimation(_AnimationClip);
    }

    public void AnimationEnd()
    {
        _MotionManager.MotionFinish(this);
    }

    protected void PlayHitEffect(int effectIdx, MotionManager impactSender)
    {
        if (_HitEffect.Length > effectIdx && effectIdx >= 0)
        {
            _MotionManager.PlayDynamicEffect(_HitEffect[effectIdx]);
        }
    }
}
