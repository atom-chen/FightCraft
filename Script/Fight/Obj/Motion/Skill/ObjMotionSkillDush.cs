﻿using UnityEngine;
using System.Collections;

public class ObjMotionSkillDush : ObjMotionSkillBase
{
    void Update()
    {
        if (_CanNextInput)
        {
            if (InputManager.Instance.IsKeyDown(_SkillAttack._ActInput))
            {
                if (_Step == 0)
                {
                    AttackStep();
                }
                else if(_Step == 1)
                {
                    AttackNext();
                }
            }
        }

        //if (_DushPos != Vector3.zero)
        //{
        //    float dis = Vector3.Distance(_DushPos, _MotionManager.transform.position);
        //    if (dis < _DushAttackDis)
        //    {
        //        AttackStep();
        //        _DushPos = Vector3.zero;
        //    }
        //}
    }

    #region override

    public override void Init()
    {
        base.Init();

        _MotionManager.InitAnimation(_AttackAnim);
        _MotionManager.AddAnimationEndEvent(_AttackAnim);

        _SkillAttack = _MotionManager.GetComponentInChildren<ObjMotionSkillAttack>();
    }

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case AnimEventManager.NEXT_INPUT_START:
                _CanNextInput = true;
                NextInputPress();
                break;
            case AnimEventManager.NEXT_INPUT_END:
                _CanNextInput = false;
                break;
        }
    }

    #endregion

    public AnimationClip _AttackAnim;
    public EffectController _AttackEffect;
    public ImpactPushToPos _DushImpact;
    public float _DushAttackDis;

    private ObjMotionSkillAttack _SkillAttack;
    private bool _CanNextInput = false;
    private Vector3 _DushPos;
    private int _Step = 0;

    public override bool ActSkill(Hashtable exhash)
    {
        if (!base.ActSkill())
            return false;

        _CanNextInput = true;
        _Step = 0;
        //Invoke("AttackStep", 0.3f);
        //var targetMotion = SelectTargetCommon.GetNearMotions(_MotionManager, _DushImpact._Time * _DushImpact._Speed);
        //if (targetMotion.Count > 0)
        //{
        //    _DushPos = targetMotion[0].transform.position;
        //    _DushImpact.DestPos = _DushPos;
        //    _MotionManager.SetLookAt(_DushPos);
        //}
        return true;
    }

    public override void FinishSkillImmediately()
    {
        base.FinishSkillImmediately();
        _CanNextInput = false;
        _DushPos = Vector3.zero;
    }

    private void NextInputPress()
    {
        if (InputManager.Instance.IsKeyHold(_SkillAttack._ActInput))
        {
            AttackNext();
        }
    }

    private void AttackStep()
    {
        _Step = 1;
        _MotionManager.ResetMove();

        PlayAnimation(_AttackAnim);
        PlaySkillEffect(_AttackEffect);
        StopSkillEffect(_Effect);
    }

    private void AttackNext()
    {
        _MotionManager.FinishSkill(this);
        _MotionManager.ActSkill(_SkillAttack);
        _SkillAttack.DushAttack();
    }


}

