using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeroDexNormal : AI_DexHeroBase
{
    protected override void Init()
    {
        base.Init();

    }

    #region

    public float _AlertRange = 15;
    public float _CloseRange = 2;
    protected  float _CloseInterval = 0.5f;

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
        if (_NextSkillIdx > 0)
        {
            StartSkill(_AISkills[_NextSkillIdx]);
            _NextSkillIdx = -1;
            return true;
        }

        if (!IsRandomActSkill())
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].IsSkillCD())
                continue;

            if (_AISkills[i].SkillRange < dis)
                continue;

            StartSkill(_AISkills[i]);
            return true;

        }

        return false;
    }

    #endregion
}

