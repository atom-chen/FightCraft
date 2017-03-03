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

    public void Start()
    {
        _SelfMotion = GetComponent<MotionManager>();
    }

    public void FixedUpdate()
    {
        if (_TargetMotion == null)
            return;

        CloseUpdate();
    }

    private void CloseUpdate()
    {
        if (_TargetMotion.ActingSkill != null)
            return;

        if (Vector3.Distance(transform.position, _TargetMotion.transform.position) > _CloseRange)
        {
            if (_CloseWait > 0)
            {
                _CloseWait -= Time.fixedDeltaTime;
                return;
            }
            _SelfMotion.BaseMotionManager.MoveDirect(_TargetMotion.transform.position - transform.position);
        }
        else
        {
            UseSkill();
            _CloseWait = _CloseInterval;
        }
    }

    private void UseSkill()
    {
        if (_SkillWait > 0)
        {
            _SkillWait -= Time.fixedDeltaTime;
            return;
        }

        _SkillWait = _SkillInterval;
        int rand = Random.Range(0, _Skills.Length);
        _SelfMotion.ActSkill(_Skills[rand]);
    }
}
