using UnityEngine;
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
            case AnimationEvent.ANIMATION_END:
                PlayerNextAnim();
                break;
            case AnimationEvent.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimationEvent.COLLIDER_END:
                ColliderEnd(param);
                break;
        }
    }

    public override void FinishSkillImmediately()
    {
        base.FinishSkillImmediately();

        if (_NextEffect[_NextEffect.Length - 1] != null)
            _MotionManager.StopSkillEffect(_NextEffect[_NextEffect.Length - 1]);
    }
    #endregion

    public AnimationClip[] _NextAnim;
    public EffectController[] _NextEffect;

    private int _CurStep;

    public override bool ActSkill()
    {

        bool isActSkill = base.ActSkill();
        if (!isActSkill)
            return false;

        _CurStep = -1;
        PlayerNextAnim();

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
            _MotionManager.RePlayAnimation(_NextAnim[_CurStep]);

            if (_CurStep - 1 >= 0 && _NextEffect[_CurStep - 1] != null)
            {
                _MotionManager.StopSkillEffect(_NextEffect[_CurStep - 1]);
            }

            if (_NextEffect.Length > _CurStep && _NextEffect[_CurStep] != null)
            {
                _MotionManager.PlaySkillEffect(_NextEffect[_CurStep]);
            }

        }
    }
}
