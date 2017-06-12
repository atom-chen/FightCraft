using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;

public class MotionManager : MonoBehaviour
{
    public void InitMotion()
    {
        _EventController = GetComponent<GameBase.EventController>();
        if (_EventController == null)
        {
            _EventController = gameObject.AddComponent<GameBase.EventController>();
        }

        InitRoleAttr();

        _Animaton = GetComponentInChildren<Animation>();
        _AnimationEvent = GetComponentInChildren<AnimEventManager>();
        if (_AnimationEvent == null)
        {
            _AnimationEvent = _Animaton.gameObject.AddComponent<AnimEventManager>();
        }
        _AnimationEvent.Init();

        _BaseMotionManager = GetComponent<BaseMotionManager>();
        _BaseMotionManager.Init();
        _BaseMotionManager.MotionIdle();

        if (_NavAgent == null)
        {
            _NavAgent = GetComponent<NavMeshAgent>();
        }

        InitSkills();
    }

    void FixedUpdate()
    {
        UpdateMove();
        
    }

    public void Reset()
    {
        _IsMotionDie = false;
    }

    #region motion

    public bool _IsRoleHit = false;
    public float _RoleHitTime = 0.01f;

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
    private AnimEventManager _AnimationEvent;
    public AnimEventManager AnimationEvent
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
        RemoveBuff(typeof(ImpactBlock));
        _Animaton[animClip.name].speed = RoleAttrManager.SkillSpeed;
        _Animaton.Play(animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip, float speed)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton[animClip.name].speed = speed;
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip, float speed)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton[animClip.name].speed = speed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip)
    {
        RemoveBuff(typeof(ImpactBlock));
        _Animaton[animClip.name].speed = RoleAttrManager.SkillSpeed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    float _OrgSpeed = 1;
    int _PauseCnt = 0;
    public void PauseAnimation(AnimationClip animClip)
    {
        if (_Animaton[animClip.name].speed == 0)
        {
            ++_PauseCnt;
            return;
        }
        else
        {
            _PauseCnt = 1;
        }

        _OrgSpeed = _Animaton[animClip.name].speed;
        _Animaton[animClip.name].speed = 0;
    }

    public void PauseAnimation()
    {
        foreach (AnimationState state in _Animaton)
        {
            if (_Animaton.IsPlaying(state.name))
            {
                PauseAnimation(state.clip);
            }
        }
        
    }

    public void ResumeAnimation(AnimationClip animClip)
    {
        --_PauseCnt;
        if (_PauseCnt > 0)
            return;

        if (_Animaton.IsPlaying(animClip.name))
        {
            _Animaton[animClip.name].speed = _OrgSpeed;
        }
    }

    public void ResumeAnimation()
    {
        foreach (AnimationState state in _Animaton)
        {
            if (_Animaton.IsPlaying(state.name))
            {
                ResumeAnimation(state.clip);
            }
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
            //skill.SetImpactElement(ElementType.Cold);
            skill.Init();
        }
    }

    public void ActSkill(ObjMotionSkillBase skillMotion)
    {
        if (!skillMotion.IsCanActSkill())
            return;

        if (_ActingSkill != null)
            _ActingSkill.FinishSkill();

        if (BaseMotionManager.IsMoving())
        {
            BaseMotionManager.StopMove();
        }

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

    private GameBase.EventController _EventController;
    public GameBase.EventController EventController
    {
        get
        {
            return _EventController;
        }
    }

    #endregion

    #region roleAttr

    private Tables.MonsterBaseRecord _MonsterBase;

    public Tables.MonsterBaseRecord MonsterBase
    {
        get
        {
            return _MonsterBase;
        }
    }

    private bool _IsMotionDie = false;
    public bool IsMotionDie
    {
        get
        {
            return _IsMotionDie;
        }
    }

    private RoleAttrManager _RoleAttrManager;
    public RoleAttrManager RoleAttrManager
    {
        get
        {
            return _RoleAttrManager;
        }
    }

    public void InitRoleAttr(Tables.MonsterBaseRecord monsterBase)
    {
        _MonsterBase = monsterBase;
    }

    private void InitRoleAttr()
    {
        _IsMotionDie = false;
        _RoleAttrManager = GetComponent<RoleAttrManager>();
        if (_RoleAttrManager == null)
        {
            _RoleAttrManager = gameObject.AddComponent<RoleAttrManager>();
        }
        _RoleAttrManager._MotionManager = this;

        if (_RoleAttrManager.MotionType == MotionType.MainChar)
            _RoleAttrManager.InitMainRoleAttr();
        else if (_MonsterBase != null)
            _RoleAttrManager.InitEnemyAttr(_MonsterBase);
        else
            Debug.LogError("MonsterBase is Null");
    }

    public void MotionDie()
    {
        _IsMotionDie = true;
        Hashtable hash = new Hashtable();
        hash.Add("HitEffect", -1);
        _EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, this, new Hashtable());
    }

    public void MotionDisappear()
    {
        if (FightManager.Instance != null)
        {
            FightManager.Instance.ObjDie(this);
        }
    }

    #endregion

    #region buff

    private List<ImpactBuff> _ImpactBuffs = new List<ImpactBuff>();
    private GameObject _BuffBindPos;
    private List<ImpactBuff> _RemoveTemp = new List<ImpactBuff>();

    public ImpactBuff AddBuff(ImpactBuff buff)
    {
        if (_BuffBindPos == null)
        {
            _BuffBindPos = new GameObject("BuffBind");
            _BuffBindPos.transform.SetParent(transform);
            _BuffBindPos.transform.localPosition = Vector3.zero;
            _BuffBindPos.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }

        var buffGO = new GameObject("Buff-" + buff.ToString());
        buffGO.transform.SetParent(_BuffBindPos.transform);
        buffGO.transform.localPosition = Vector3.zero;
        buffGO.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var newBuff = buffGO.AddComponent(buff.GetType()) as ImpactBuff;
        CopyComponent(buff, newBuff);
        _ImpactBuffs.Add(newBuff);
        newBuff.ActBuff(this);

        return newBuff;
    }

    public void RemoveBuff(ImpactBuff buff)
    {
        buff.RemoveBuff(this);
        _ImpactBuffs.Remove(buff);
        GameObject.Destroy(buff.gameObject);
    }

    public void RemoveBuff(Type buffType)
    {
        _RemoveTemp.Clear();
        foreach (var buff in _ImpactBuffs)
        {
            if (buff.GetType() == buffType)
            {
                _RemoveTemp.Add(buff);
            }
        }

        foreach (var buff in _RemoveTemp)
        {
            RemoveBuff(buff);
        }
    }

    void CopyComponent(object original, object destination)
    {
        System.Type type = original.GetType();
        System.Reflection.FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(destination, field.GetValue(original));
        }
    }

    #endregion

    #region effect

    private Dictionary<string, EffectController> _SkillEffects = new Dictionary<string, EffectController>();
    private Dictionary<string, Transform> _BindTransform = new Dictionary<string, Transform>();
    private EffectController _PlayingEffect;

    public void PlaySkillEffect(EffectController effect, ElementType elementType = ElementType.None)
    {
        if (!_SkillEffects.ContainsKey(effect.name))
        {
            var idleEffect = GameObject.Instantiate(effect);
            idleEffect.transform.SetParent(GetBindTransform(effect._BindPos));
            idleEffect.transform.localPosition = Vector3.zero;
            idleEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
            CopyComponent(effect, idleEffect);
            _SkillEffects.Add(effect.name, idleEffect);
        }
        _PlayingEffect = _SkillEffects[effect.name];
        _PlayingEffect.SetEffectColor(elementType);
        _PlayingEffect.PlayEffect(RoleAttrManager.SkillSpeed);
    }

    public void StopSkillEffect(EffectController effect)
    {
        if (_SkillEffects.ContainsKey(effect.name))
        {
            _SkillEffects[effect.name].HideEffect();
        }
    }

    public void StopSkillEffect()
    {
        StopSkillEffect(_PlayingEffect);
    }

    public void PauseSkillEffect()
    {
        _PlayingEffect.PauseEffect();
        //foreach (var skillEffect in _SkillEffects)
        //{
        //    skillEffect.Value.PauseEffect();
        //}
    }

    public void ResumeSkillEffect()
    {
        _PlayingEffect.ResumeEffect();
        //foreach (var skillEffect in _SkillEffects)
        //{
        //    skillEffect.Value.ResumeEffect();
        //}
    }



    public Transform GetBindTransform(string bindName)
    {
        if (string.IsNullOrEmpty(bindName))
        {
            if (!_BindTransform.ContainsKey(_Animaton.name))
            {
                var bindTran = transform.FindChild(_Animaton.name);
                _BindTransform.Add(_Animaton.name, bindTran);
            }
        }

        if (!_BindTransform.ContainsKey(bindName))
        {
            var bindTran = transform.FindChild(_Animaton.name + "/" + bindName);
            _BindTransform.Add(bindName, bindTran);
        }

        return _BindTransform[bindName];
    }

    public EffectController PlayDynamicEffect(EffectController effect, Hashtable hashParam = null)
    {
        var idleEffect = ResourcePool.Instance.GetIdleEffect(effect);
        idleEffect.transform.SetParent(GetBindTransform(effect._BindPos));
        idleEffect.transform.localPosition = Vector3.zero;
        idleEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
        if (hashParam != null && hashParam.ContainsKey("WorldPos"))
        {
            idleEffect.transform.position = (Vector3)hashParam["WorldPos"];
        }
        idleEffect._EffectLastTime = effect._EffectLastTime;
        if(hashParam == null)
            idleEffect.PlayEffect();
        else
            idleEffect.PlayEffect(hashParam);
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
        ResourcePool.Instance.RecvIldeEffect(effct);
    }

    #endregion

    #region move

    private static int _NormalNavPrior = 50;
    private static int _NormalCorpsePrior = 49;
    private static int _EliteNavPrior = 40;
    private static int _EliteCorpsePrior = 39;
    private static int _PlayerNavPrior = 30;
    private static int _PlayerCorpsePrior = 29;
    private static int _HeroNavPrior = 20;
    private static int _HeroCorpsePrior = 19;

    private NavMeshAgent _NavAgent;
    public NavMeshAgent NavAgent
    {
        get
        {
            if (_NavAgent == null)
            {
                _NavAgent = GetComponent<NavMeshAgent>();
            }
            return _NavAgent;
        }
    }
    private Vector3 _TargetVec;
    private float _LastTime;
    private float _Speed;

    public void SetPosition(Vector3 position)
    {
        NavMeshHit navHit = new NavMeshHit();
        if (!NavMesh.SamplePosition(position, out navHit, 5, NavMesh.AllAreas))
        {
            return;
        }
        //transform.position = navHit.position;
        NavAgent.Warp(navHit.position);
    }

    public void SetLookRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.LookRotation(rotate);
    }

    public void SetRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.Euler(rotate);
    }

    public void SetLookAt(Vector3 target)
    {
        transform.LookAt(target);
    }

    public void ResetMove()
    {
        _TargetVec = Vector3.zero;
        _LastTime = 0;
    }

    public void SetMove(Vector3 moveVec, float lastTime)
    {
        _TargetVec = moveVec;
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

        var destPoint = transform.position + moveVec;
        NavMeshHit navHit = new NavMeshHit();
        if (!NavMesh.SamplePosition(destPoint, out navHit, 5, NavMesh.AllAreas))
        {
            return;
        }
        _NavAgent.Warp(navHit.position);
    }

    public void SetCorpsePrior()
    {
        int corpsePrior = 99;
        switch (RoleAttrManager.MotionType)
        {
            case MotionType.Normal:
                corpsePrior = _NormalCorpsePrior;
                break;
            case MotionType.Elite:
                corpsePrior = _EliteCorpsePrior;
                break;
            case MotionType.Hero:
                corpsePrior = _HeroCorpsePrior;
                break;
            case MotionType.MainChar:
                corpsePrior = _PlayerCorpsePrior;
                break;
        }

        _NavAgent.avoidancePriority = corpsePrior;
    }

    public void ResumeCorpsePrior()
    {
        int corpsePrior = 99;
        switch (RoleAttrManager.MotionType)
        {
            case MotionType.Normal:
                corpsePrior = _NormalNavPrior;
                break;
            case MotionType.Elite:
                corpsePrior = _EliteNavPrior;
                break;
            case MotionType.Hero:
                corpsePrior = _HeroNavPrior;
                break;
            case MotionType.MainChar:
                corpsePrior = _PlayerNavPrior;
                break;
        }

        _NavAgent.avoidancePriority = corpsePrior;
    }

    #endregion

    #region motion pause

    public void SkillPause()
    {
        if (ActingSkill != null)
        {
            ActingSkill.PauseSkill();
            PauseAnimation();
            PauseSkillEffect();
        }
    }

    public void SkillPause(float time)
    {
        SkillPause();
        StartCoroutine(SkillResumeLater(time));
    }

    public void SkillResume()
    {
        if (ActingSkill != null)
        {
            ActingSkill.ResumeSkill();
            ResumeAnimation();
            ResumeSkillEffect();
        }
    }

    public IEnumerator SkillResumeLater(float time)
    {
        yield return new WaitForSeconds(time);
        SkillResume();
    }

    #endregion

    #region target

    public bool _CanBeSelectByEnemy = true;

    #endregion

    #region bullet

    private Transform _BulletBindPos = null;
    public Transform BulletBindPos
    {
        get
        {
            if (_BulletBindPos == null)
            {
                var bulletGO = new GameObject("BulletBind");
                _BulletBindPos = bulletGO.transform;
                _BulletBindPos.transform.SetParent(transform);
                _BulletBindPos.transform.localPosition = Vector3.zero;
                _BulletBindPos.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            return _BulletBindPos;
        }
    }

    #endregion

    #region collider

    private Collider _TriggerCollider;
    public Collider TriggerCollider
    {
        get
        {
            if (_TriggerCollider == null)
            {
                _TriggerCollider = AnimationEvent.GetComponentInChildren<Collider>();
            }
            return _TriggerCollider;
        }
    }

    #endregion
}
