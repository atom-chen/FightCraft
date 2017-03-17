using UnityEngine;
using System.Collections;

public class AI_HeroDaJi : AI_Base
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
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_RISE, RiseEvent);
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_RISE_FINISH, RiseFinishEvent);
    }

    public void FixedUpdate()
    {
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

    #region rise

    private float _RiseTime = 1f;

    public void RiseEvent(object sender, Hashtable eventArgs)
    {
        _SelfMotion._CanBeSelectByEnemy = false;
        StartCoroutine(RiseFinish());
    }

    public IEnumerator RiseFinish()
    {
        yield return new WaitForSeconds(_RiseTime);

        _SelfMotion._CanBeSelectByEnemy = true;
    }

    public void RiseFinishEvent(object sender, Hashtable eventArgs)
    {

    }

    #endregion
}
