using UnityEngine;
using System.Collections;

public class AI_CloseAttack : AI_Base
{
    public float _AlertRange = 6;
    public float _CloseRange = 2;
    public float _CloseInterval = 1;

    private float _CloseWait;
    
    protected override void Init()
    {
        base.Init();
    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_TargetMotion == null)
            return;

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_SelfMotion.ActingSkill != null)
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
            UseSkill();
            _CloseWait = _CloseInterval;
        }
    }

    private void UseSkill()
    {
        base.StartSkill();
    }
}
