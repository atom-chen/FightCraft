using UnityEngine;
using System.Collections;

public class ObjMotionSkillRoll : ObjMotionSkillBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        foreach (var anim in _NextAnim)
        {
            _MotionManager.InitAnimation(anim);
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


}
