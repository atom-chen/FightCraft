using UnityEngine;
using System.Collections;

public class ObjMotionSkillMixAnim : ObjMotionSkillBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        AddAnimationEndEvent(_AnimationClip);

        foreach (var anim in _NextAnim)
        {
            _MotionManager.InitAnimation(anim);
            AddAnimationEndEvent(anim);
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
                PlayerNextAnim();
                break;
        }
    }

    #endregion

    public AnimationClip[] _NextAnim;
    public EffectController[] _NextEffect;
    public SelectBase[] _Collider;

    private int _CurStep;

    public override bool ActSkill()
    {
        
        if (!IsCanActiveMotion())
            return false;

        _SkillLastTime = _AnimationClip.length;
        foreach (var anim in _NextAnim)
        {
            _SkillLastTime += anim.length;
        }
        _MotionManager.MotionStart(this);
        _MotionManager.PlayAnimation(_AnimationClip);
        if (_Effect != null)
            _Effect.PlayEffect(_MotionManager._RoleAttrManager.SkillSpeed);

        _CurStep = -1;

        return true;
    }

    public void PlayerNextAnim()
    {
        if (_CurStep + 1 == _NextAnim.Length)
        {
            FinishSkillImmediately();
        }
        else
        {
            ++_CurStep;
            _MotionManager.PlayAnimation(_NextAnim[_CurStep]);
            if (_NextEffect.Length > _CurStep && _NextEffect[_CurStep] != null)
            {
                _NextEffect[_CurStep].PlayEffect(_MotionManager._RoleAttrManager.SkillSpeed);
            }

        }
    }
}
