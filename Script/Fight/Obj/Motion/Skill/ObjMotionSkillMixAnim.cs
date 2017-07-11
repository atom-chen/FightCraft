﻿using UnityEngine;
using System.Collections;

public class ObjMotionSkillMixAnim : ObjMotionSkillBase
{
    #region override

    public override void Init()
    {
        base.Init();

        foreach (var anim in _NextAnim)
        {
            if (anim != null)
            {
                _MotionManager.InitAnimation(anim);
                _MotionManager.AddAnimationEndEvent(anim);
            }
        }
    }
    
    public override void AnimEvent(string function, object param)
    {
        switch (function)
        {
            case AnimEventManager.ANIMATION_END:
                PlayerNextAnim();
                break;
            case AnimEventManager.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimEventManager.COLLIDER_END:
                ColliderEnd(param);
                break;
        }
    }

    public override void FinishSkillImmediately()
    {
        base.FinishSkillImmediately();

        if (_NextEffect[_NextEffect.Length - 1] != null)
            _MotionManager.StopSkillEffect(_NextEffect[_NextEffect.Length - 1]);

        if (_Effect != null)
            _MotionManager.StopSkillEffect(_Effect);
    }

    protected override float GetTotalAnimLength()
    {
        float totleTime = 0;
        foreach (var anim in _NextAnim)
        {
            totleTime += anim.length / SkillActSpeed;
        }
        return totleTime;
    }
    #endregion

    public AnimationClip[] _NextAnim;
    public EffectController[] _NextEffect;

    private int _CurStep;

    public override bool ActSkill(Hashtable exhash)
    {
        base.ActSkill();

        _CurStep = -1;
        PlayerNextAnim();

        if(_Effect != null)
            PlaySkillEffect(_Effect);

        return true;
    }

    public void PlayerNextAnim()
    {
        if (_CurStep + 1 == _NextAnim.Length)
        {
            FinishSkill();
        }
        else
        {
            ++_CurStep;
            PlayAnimation(_NextAnim[_CurStep]);

            if (_CurStep - 1 >= 0 && _NextEffect[_CurStep - 1] != null)
            {
                StopSkillEffect(_NextEffect[_CurStep - 1]);
            }

            if (_NextEffect.Length > _CurStep && _NextEffect[_CurStep] != null)
            {
                PlaySkillEffect(_NextEffect[_CurStep]);
            }

        }
    }
}
