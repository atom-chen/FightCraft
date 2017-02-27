using UnityEngine;
using System.Collections;

public class ObjMotionSkillAttack : ObjMotionSkillBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        foreach (var anim in _NextAnim)
        {
            _MotionManager.InitAnimation(anim);
        }
        _MotionPriority = 50;
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

    public override bool ContinueInput(InputManager inputManager)
    {
        if (!_CanNextInput)
            return false;

        if (inputManager.IsKeyDown(_ActInput))
        {
            ContinueAttack();
            return true;
        }
        return false;
    }

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case "NextInputStart":
                _CanNextInput = true;
                break;
            case "NextInputEnd":
                _CanNextInput = false;
                FinishSkillImmediately();
                break;
        }
    }

    protected override void InitEvent()
    {
        base.InitEvent();
    }

    protected override void FinishSkillImmediately()
    {
        base.FinishSkillImmediately();

        if (_Effect != null)
            _MotionManager.StopSkillEffect(_Effect);

        foreach (var effect in _NextEffect)
        {
            if (effect != null)
            {
                _MotionManager.StopSkillEffect(effect);
            }
        }
    }
    #endregion

    public AnimationClip[] _NextAnim;
    public EffectController[] _NextEffect;
    public SelectBase[] _Collider;

    private bool _CanNextInput = false;
    private int _CurStep;

    public override bool ActSkill()
    {
        bool isActSkill = base.ActSkill();
        if (!isActSkill)
            return false;

        _SkillLastTime = _AnimationClip.length;
        _CanNextInput = false;
        _CurStep = -1;

        return true;
    }

    public void ContinueAttack()
    {
        if (_CurStep + 1 < _NextAnim.Length)
        {
            ++_CurStep;
            _MotionManager.PlayAnimation(_NextAnim[_CurStep]);
            _SkillLastTime = _NextAnim[_CurStep].length;
            if (_NextEffect.Length > _CurStep && _NextEffect[_CurStep] != null)
            {
                _MotionManager.PlaySkillEffect(_NextEffect[_CurStep]);
            }

            if (_CurStep - 1 >= 0 && _NextEffect[_CurStep - 1] != null)
            {
                _MotionManager.StopSkillEffect(_NextEffect[_CurStep - 1]);
            }
            else
            {
                _MotionManager.StopSkillEffect(_Effect);
            }

            StopAllCoroutines();
            StartCoroutine(FinishSkill());

            _CanNextInput = false;
        }
    }



}
