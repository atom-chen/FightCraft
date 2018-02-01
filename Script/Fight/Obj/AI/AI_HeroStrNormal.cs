using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeroStrNormal : AI_StrengthHeroBase
{
    protected override void Init()
    {
        base.Init();

    }

    #region

    public float _AlertRange = 15;
    public float _CloseRange = 2;
    public float _CloseInterval = 1;

    private float _CloseWait;
    private int _NextForceSkill;

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_SelfMotion.ActingSkill!= null)
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
            _SelfMotion.StartMoveState(_TargetMotion.transform.position);
        }
        else
        {
            _SelfMotion.StopMoveState();
            StartSkill();
            _CloseWait = _CloseInterval;
        }
    }

    protected override bool StartSkill()
    {
        if (_NextForceSkill > 0)
        {
            StartSkill(_AISkills[_NextForceSkill]);
            _NextForceSkill = 0;
        }

        if (!IsRandomActSkill())
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        List<int> skillIdxs = null;

        for (int i = skillIdxs.Count - 1; i >= 0; --i)
        {
            if (_AISkills[skillIdxs[i]].SkillRange < dis)
                continue;

            if (!_AISkills[skillIdxs[i]].IsSkillCD())
                continue;

            {
                if(skillIdxs[i] == 2)
                {
                    _NextForceSkill = 3;
                }
                StartSkill(_AISkills[skillIdxs[i]]);
                return true;
            }
        }

        return false;
    }

    #endregion
}

