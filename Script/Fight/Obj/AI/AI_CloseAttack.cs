using UnityEngine;
using System.Collections;

public class AI_CloseAttack : AI_Base
{

    public MotionManager _TargetMotion;
    public float _AlertRange;
    public float _CloseRange;
    public float _CloseInterval;
    public float _SkillInterval;
    public ObjMotionSkillBase[] _Skills;

    private float _SkillWait;
    private float _CloseWait;
    private MotionManager _SelfMotion;

    protected override void Init()
    {
        base.Init();

        _SelfMotion = GetComponent<MotionManager>();
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

    private bool UseSkill()
    {
        if (_Skills.Length == 0)
            return false;

        _SelfMotion.transform.LookAt(_TargetMotion.transform.position);
        if (_SkillWait > 0)
        {
            _SkillWait -= Time.fixedDeltaTime;
            return false;
        }
        
        _SkillWait = _SkillInterval;
        int rand = Random.Range(0, _Skills.Length);
        _SelfMotion.ActSkill(_Skills[rand]);
        return true;
    }
}
