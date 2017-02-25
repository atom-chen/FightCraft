using UnityEngine;
using System.Collections;

public class ObjMotionSkillDushHit : ObjMotionSkillBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        if(_AnimLoop != null)
            _MotionManager.InitAnimation(_AnimLoop);

        if (_AnimEnd != null)
            _MotionManager.InitAnimation(_AnimEnd);

        if (_SkillLastTime < 0)
        {
            _SkillLastTime = 0;

            if (_AnimationClip != null)
            {
                _SkillLastTime += _AnimationClip.length;
            }
            if (_AnimLoop != null)
            {
                _SkillLastTime += _AnimLoop.length;
            }
            if (_AnimEnd != null)
            {
                _SkillLastTime += _AnimEnd.length;
            }
        }

        _LoopTime = _SkillLastTime;
        if (_AnimationClip != null)
        {
            _LoopTime -= _AnimationClip.length;
        }
        if (_AnimEnd != null)
        {
            _LoopTime -= _AnimEnd.length;
        }

        _MotionPriority = 100;
    }

    public override void PlayMotion(object go, Hashtable eventArgs)
    {
        base.PlayMotion(go, eventArgs);
    }

    public override bool ActiveInput(InputManager inputManager)
    {
        if (!IsCanActiveMotion())
            return false;

        if (inputManager.IsKeyDown(_ActInput))
        {
            ActSkill();
            return true;
        }
        return false;
    }

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case "AnimationEnd":
                ContinueAttack();
                break;
        }
    }

    protected override void InitEvent()
    {
        base.InitEvent();
    }
    #endregion

    public AnimationClip _AnimLoop;
    public AnimationClip _AnimEnd;
    public EffectController _EffectLoop;
    public EffectController _EffectEnd;

    private float _LoopTime = 0;

    public override bool ActSkill()
    {
        _MotionManager.MotionStart(this);
        if (_AnimationClip != null)
        {
            _MotionManager.PlayAnimation(_AnimationClip);
            if (_Effect != null)
                _Effect.PlayEffect(_MotionManager._RoleAttrManager.SkillSpeed);
        }
        else if (_AnimLoop != null)
        {
            ContinueAttack();

        }

        StartCoroutine(FinishSkill());

        return true;
    }

    public void ContinueAttack()
    {
        _MotionManager.PlayAnimation(_AnimLoop);
        if (_EffectLoop != null)
            _EffectLoop.PlayEffect(_MotionManager._RoleAttrManager.SkillSpeed);

        StartCoroutine(SkillLoopEnd());
    }

    public IEnumerator SkillLoopEnd()
    {
        yield return new WaitForSeconds(_LoopTime);

        if (_AnimEnd != null)
        {
            _MotionManager.PlayAnimation(_AnimEnd);
            if (_EffectEnd != null)
                _EffectEnd.PlayEffect(_MotionManager._RoleAttrManager.SkillSpeed);
        }
    }

}
