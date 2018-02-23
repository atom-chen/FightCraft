using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_EliteHeroYangJian : AI_HeroStrNormal
{
    protected override void Init()
    {
        base.Init();

    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        UpdateAutoBullet();
    }

    #region 

    public ImpactBase _AutoBullet;
    public float _BulletInterval;

    private float _AutoStartTime;
    private float _AutoInterval;

    private void UpdateAutoBullet()
    {
        if (_TargetMotion == null)
            return;

        if (_SelfMotion._ActionState == _SelfMotion._StateIdle
            || _SelfMotion._ActionState == _SelfMotion._StateMove
            || _SelfMotion._ActionState == _SelfMotion._StateSkill)
        {
            if (_AutoStartTime != 0)
            {
                _AutoInterval = Time.time - _AutoStartTime;
                _TargetMotion._SkillProcessing = _AutoInterval / _BulletInterval;
                if (_AutoInterval > _BulletInterval)
                {
                    ActImpact();
                }
            }
            else
            {
                _TargetMotion._SkillProcessing = 0;
                _AutoStartTime = Time.time;
                _AutoInterval = 0;
            }
        }
        else
        {
            _TargetMotion._SkillProcessing = 0;
            _AutoStartTime = 0;
            _AutoInterval = 0;
        }
    }

    private void ActImpact()
    {
        _AutoBullet.ActImpact(_SelfMotion, _TargetMotion);
        _AutoStartTime = 0;
        _AutoInterval = 0;
    }

    #endregion

}

