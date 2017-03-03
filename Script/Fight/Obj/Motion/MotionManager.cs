using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionManager : MonoBehaviour
{
    void Start()
    {
        _Animaton = GetComponentInChildren<Animation>();
        _AnimationEvent = GetComponentInChildren<AnimationEvent>();
        if (_AnimationEvent == null)
        {
            _AnimationEvent = _Animaton.gameObject.AddComponent<AnimationEvent>();
        }
        _AnimationEvent.Init();

        _BaseMotionManager = GetComponent<BaseMotionManager>();
        _BaseMotionManager.Init();

        InitSkills();
    }

    void FixedUpdate()
    {
        UpdateMove();
    }

    #region motion

    public bool _IsRoleHit = false;

    private int _MotionPrior;
    public int MotionPrior
    {
        get
        {
            return _MotionPrior;
        }
        set
        {
            _MotionPrior = value;
        }
    }

    private BaseMotionManager _BaseMotionManager;
    public BaseMotionManager BaseMotionManager
    {
        get
        {
            return _BaseMotionManager;
        }
    }

    public void NotifyAnimEvent(string function, object param)
    {
        if (_ActingSkill != null)
        {
            _ActingSkill.AnimEvent(function, param);
        }
        else
        {
            _BaseMotionManager.DispatchAnimEvent(function, param);
        }
    }
    #endregion

    #region Animation

    private Animation _Animaton;
    private AnimationEvent _AnimationEvent;
    public AnimationEvent AnimationEvent
    {
        get
        {
            return _AnimationEvent;
        }
    }

    public void InitAnimation(AnimationClip animClip)
    {
        _Animaton.AddClip(animClip, animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip)
    {
        _Animaton[animClip.name].speed = _RoleAttrManager.SkillSpeed;
        _Animaton.Play(animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip, float speed)
    {
        _Animaton[animClip.name].speed = speed;
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip, float speed)
    {
        _Animaton[animClip.name].speed = speed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip)
    {
        _Animaton[animClip.name].speed = _RoleAttrManager.SkillSpeed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    float _OrgSpeed = 1;
    public void PauseAnimation(AnimationClip animClip)
    {
        _OrgSpeed = _Animaton[animClip.name].speed;
        _Animaton[animClip.name].speed = 0;
    }

    public void ResumeAnimation(AnimationClip animClip)
    {
        if (_Animaton.IsPlaying(animClip.name))
        {
            _Animaton[animClip.name].speed = _OrgSpeed;
        }
    }

    public void AddAnimationEndEvent(AnimationClip animClip)
    {
        if (animClip == null)
            return;

        UnityEngine.AnimationEvent animEvent = new UnityEngine.AnimationEvent();
        animEvent.time = animClip.length;
        animEvent.functionName = "AnimationEnd";
        animClip.AddEvent(animEvent);
    }

    #endregion

    #region skill

    public Dictionary<string, ObjMotionSkillBase> _SkillMotions = new Dictionary<string, ObjMotionSkillBase>();

    private ObjMotionSkillBase _ActingSkill;
    public ObjMotionSkillBase ActingSkill
    {
        get
        {
            return _ActingSkill;
        }
    }

    private void InitSkills()
    {
        var skillList = gameObject.GetComponentsInChildren<ObjMotionSkillBase>();
        foreach (var skill in skillList)
        {
            _SkillMotions.Add(skill._ActInput, skill);
            skill.Init();
        }
    }

    public void ActSkill(ObjMotionSkillBase skillMotion)
    {
        if (!skillMotion.IsCanActSkill())
            return;

        if (_ActingSkill != null)
            _ActingSkill.FinishSkill();

        skillMotion.ActSkill();
        _ActingSkill = skillMotion;
        MotionPrior = _ActingSkill._SkillMotionPrior;
    }

    public void FinishSkill(ObjMotionSkillBase skillMotion)
    {
        _ActingSkill = null;
        skillMotion.FinishSkillImmediately();
        BaseMotionManager.MotionIdle();
    }

    #endregion

    #region event

    public GameBase.EventController _EventController;

    #endregion

    #region roleAttr

    public RoleAttrManager _RoleAttrManager;

    #endregion

    #region buff

    List<ImpactBuff> _ImpactBuffs = new List<ImpactBuff>();

    public void AddBuff(ImpactBuff buff)
    {
        _ImpactBuffs.Add(buff);
    }

    public void RemoveBuff(ImpactBuff buff)
    {
        buff.RemoveBuff();
        _ImpactBuffs.Remove(buff);
    }

    #endregion

    #region effect

    private Dictionary<string, Transform> _BindTransform = new Dictionary<string, Transform>();

    public void PlaySkillEffect(EffectController effect)
    {
        effect.PlayEffect(_RoleAttrManager.SkillSpeed);
    }

    public void StopSkillEffect(EffectController effect)
    {
        effect.HideEffect();
    }

    public EffectController PlayDynamicEffect(EffectController effect)
    {
        if (!_BindTransform.ContainsKey(effect._BindPos))
        {
            var bindTran = transform.FindChild(_Animaton.name + "/" + effect._BindPos);
            _BindTransform.Add(effect._BindPos, bindTran);
        }

        var idleEffect = EffectController.GetIdleEffect(effect);
        idleEffect.transform.SetParent(_BindTransform[effect._BindPos]);
        idleEffect.transform.localPosition = Vector3.zero;
        idleEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
        idleEffect._EffectLastTime = effect._EffectLastTime;
        idleEffect.PlayEffect();
        if (idleEffect._EffectLastTime > 0)
        {
            StartCoroutine(StopDynamicEffect(idleEffect));
        }
        return idleEffect;
    }

    public IEnumerator StopDynamicEffect(EffectController effct)
    {
        yield return new WaitForSeconds( effct._EffectLastTime);
        StopDynamicEffectImmediately(effct);
    }

    public void StopDynamicEffectImmediately(EffectController effct)
    {
        effct.HideEffect();
        EffectController.RecvIldeEffect(effct);
    }

    #endregion

    #region move

    private NavMeshAgent _NavAgent;
    private Vector3 _TargetVec;
    private float _LastTime;
    private float _Speed;

    public void SetRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.LookRotation(rotate);
    }

    public void SetMove(Vector3 moveVec, float lastTime)
    {
        if (_NavAgent == null)
        {
            _NavAgent = GetComponent<NavMeshAgent>();
        }
        
        _TargetVec += moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
    }

    public void UpdateMove()
    {
        if (_TargetVec == Vector3.zero)
            return;

        Vector3 moveVec = _TargetVec.normalized * _Speed * Time.fixedDeltaTime;

        _TargetVec -= moveVec;
        _LastTime -= Time.fixedDeltaTime;
        if (_LastTime < 0)
        {
            _LastTime = 0;
            _TargetVec = Vector3.zero;
        }

        _NavAgent.Warp(transform.position + moveVec);
    }

    #endregion
}
