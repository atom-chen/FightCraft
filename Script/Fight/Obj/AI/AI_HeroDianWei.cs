﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeroDianWei : AI_StrengthHeroBase
{
    protected override void Init()
    {
        base.Init();

    }

    #region

    public List<int> _Stage1Skills;
    public List<int> _Stage2Skills;

    public float _AlertRange = 15;
    public float _CloseRange = 2;
    public float _CloseInterval = 1;

    private float _CloseWait;

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

        //float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        //if (distance > _AlertRange)
        //    return;

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_SelfMotion.ActingSkill != null)
            return;

        if (StartSkill())
            return;

        float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        if (distance > _CloseRange)
        {
            if (_CloseWait > 0)
            {
                _CloseWait -= Time.fixedDeltaTime;
                return;
            }
            _SelfMotion.BaseMotionManager.MoveTarget(_TargetMotion.transform.position);
        }
        else
        {
            if (_SelfMotion.BaseMotionManager.IsMoving())
            {
                _SelfMotion.BaseMotionManager.StopMove();
            }
            StartSkill();
            _CloseWait = _CloseInterval;
        }
    }

    protected override bool StartSkill()
    {
        if (!IsRandomActSkill())
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        List<int> skillIdxs = null;
        if (_Stage2Started)
        {
            skillIdxs = _Stage2Skills;
        }
        else
        {
            skillIdxs = _Stage1Skills;
        }

        for (int i = skillIdxs.Count - 1; i >= 0; --i)
        {
            if (_AISkills[skillIdxs[i]].SkillRange < dis)
                continue;

            if (!_AISkills[skillIdxs[i]].IsSkillCD())
                continue;

            {
                StartSkill(_AISkills[skillIdxs[i]]);
                return true;
            }
        }

        return false;
    }

    #endregion
}

