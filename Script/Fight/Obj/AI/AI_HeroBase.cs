using UnityEngine;
using System.Collections;

public class AI_HeroBase : AI_Base
{
    public float _CloseRange = 2;
    public float _CloseInterval = 1;
    public int _NormalAttackIdx = 0;
    public int _RiseSkillIdx = 1;
    public int _GlobalSkillIdx = 2;

    private float _SkillWait;
    private float _CloseWait;

    protected override void Init()
    {
        base.Init();

        if (_TargetMotion == null)
        {
            _TargetMotion = SelectTargetCommon.GetMainPlayer();
        }

        InitAttackBlock();
        InitRise();
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
        if (!_SelfMotion.BaseMotionManager.IsMoving() && !_SelfMotion.BaseMotionManager.IsMotionIdle())
            return;

        if (StartSkill())
        {
            return;
        }

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
            CloseEnough();
            _CloseWait = _CloseInterval;
        }
    }

    protected virtual void CloseEnough()
    {
        StartSkill();
    }

    #region attackBlock

    private EffectController _HitEffect;

    private void InitAttackBlock()
    {
        float attackConlliderTime = _SelfMotion.AnimationEvent.GetAnimFirstColliderEventTime(_AISkills[_NormalAttackIdx].SkillBase._Anim);
        if (attackConlliderTime < 0)
            return;

        var effectObj = GameBase.ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Char_OutLineBlock");
        _HitEffect = effectObj.GetComponent<EffectController>();

        _SelfMotion.AnimationEvent.AddEvent(_AISkills[_NormalAttackIdx].SkillBase._Anim, 0, AttackStart);
        _SelfMotion.AnimationEvent.AddEvent(_AISkills[_NormalAttackIdx].SkillBase._Anim, attackConlliderTime + 0.05f, AttackCollider);
    }

    private void AttackStart()
    {
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, HitEvent, 99);
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, FlyEvent, 99);
    }

    private void AttackCollider()
    {
        _SelfMotion.EventController.UnRegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, HitEvent);
        _SelfMotion.EventController.UnRegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, FlyEvent);
    }

    private void HitEvent(object sender, Hashtable eventArgs)
    {
        eventArgs.Add("StopEvent", true);
        //GlobalEffect.Instance.Pause(0.1f);
        _SelfMotion.ResetMove();
        _SelfMotion.SkillPause(0.1f);
        _SelfMotion.PlaySkillEffect(_HitEffect);
    }

    private void FlyEvent(object sender, Hashtable eventArgs)
    {
        eventArgs.Add("StopEvent", true);
        //GlobalEffect.Instance.Pause(0.1f);
        _SelfMotion.ResetMove();
        _SelfMotion.SkillPause(0.1f);
        _SelfMotion.PlaySkillEffect(_HitEffect);

    }

    #endregion

    #region rise

    private float _RiseTime = 1f;

    private void InitRise()
    {
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_RISE, RiseEvent);
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_RISE_FINISH, RiseFinishEvent);
    }

    public void RiseEvent(object sender, Hashtable eventArgs)
    {
        _SelfMotion._CanBeSelectByEnemy = false;
        StartCoroutine(RiseFinish());
        _SkillWait = _RiseTime;
    }

    public IEnumerator RiseFinish()
    {
        yield return new WaitForSeconds(_RiseTime);

        _SelfMotion._CanBeSelectByEnemy = true;
    }

    public void RiseFinishEvent(object sender, Hashtable eventArgs)
    {
        if (_RiseSkillIdx < 0)
            return;

        float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        if (distance < _CloseRange)
        {
            _SelfMotion.transform.LookAt(_TargetMotion.transform.position);
            Debug.Log("use rise skill " + Time.time);
            _SelfMotion.ActSkill(_AISkills[_RiseSkillIdx].SkillBase);
        }
    }

    #endregion

}
