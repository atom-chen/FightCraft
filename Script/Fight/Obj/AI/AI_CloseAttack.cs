using UnityEngine;
using System.Collections;

public class AI_CloseAttack : AI_Base
{
    public float _AlertRange = 15;
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

        float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        if (distance > _AlertRange)
            return;

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_SelfMotion.ActingSkill!= null)
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
}
