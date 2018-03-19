using UnityEngine;
using System.Collections;

public class AI_EliteBase : AI_HeroBase
{
    public float _AlertRange = 15;
    public float _CloseRange = 2;
    public float _CloseInterval = 1;

    private float _CloseWait;

    protected override void Init()
    {
        base.Init();
        Debug.Log("init AI_StrengthHeroBase");
        InitSkills();
    }

    protected override void AIUpdate()
    {
        base.UpdateCriticalAI();

        if (_TargetMotion == null)
            return;

        if (!_AIAwake)
        {
            float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
            if (distance > _AlertRange)
                return;

            _AIAwake = true;
            AIManager.Instance.GroupAwake();
        }

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_SelfMotion.ActingSkill != null)
            return;

        //specil:do not attack when target lie on floor
        if (_TargetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.RISE_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.FLY_PRIOR
            || _TargetMotion.MotionPrior == BaseMotionManager.HIT_PRIOR)
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
        if (_TargetMotion == null)
            return false;

        if (_TargetMotion._ActionState is StateCatch
            || _TargetMotion._ActionState is StateFly
            || _TargetMotion._ActionState is StateHit
            || _TargetMotion._ActionState is StateLie)
        {
            return false;
        }

        return base.StartSkill();
    }

    #region super armor

    private void InitSkills()
    {
        for (int i = 0; i < _AISkills.Count; ++i)
        {
            InitSuperArmorSkill(_AISkills[i].SkillBase);
            InitReadySkillSpeed(_AISkills[i].SkillBase);
        }
    }

    #endregion


    
}

