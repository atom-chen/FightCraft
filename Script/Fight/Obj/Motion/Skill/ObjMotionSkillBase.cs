using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBase : MonoBehaviour
{
    public virtual void Init()
    {
        _MotionManager = gameObject.GetComponentInParent<MotionManager>();

        if (_Anim != null)
        {
            _MotionManager.InitAnimation(_Anim);
            _MotionManager.AddAnimationEndEvent(_Anim);
        }

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        _SkillAttr = _MotionManager.RoleAttrManager.GetSkillAttr(_ActInput);
        InitCollider(_SkillAttr);
        if (_SkillAttr != null)
        {
            InitShadowEffect();
        }
    }

    public AnimationClip _Anim;
    public EffectController _Effect;
    public string _ActInput;
    public int _SkillMotionPrior = 100;
    public float _SkillBaseSpeed = 1;

    protected MotionManager _MotionManager;
    protected float _SkillLastTime;
    protected RoleAttrManager.SkillAttr _SkillAttr;

    public virtual bool IsCanActSkill()
    {
        if (_MotionManager.MotionPrior < _SkillMotionPrior)
            return true;

        return false;
    }

    public bool StartSkill()
    {
        if (!IsCanActSkill())
            return false;

        if (IsAccumulateSkill())
        {
            StartSkillAccumulate();
            return true;
        }

        return ActSkill();
    }

    public virtual bool ActSkill()
    {
        if (_Anim != null)
            PlayAnimation(_Anim);
        if(_Effect != null)
            PlaySkillEffect(_Effect);

        if(_ShadowEffect != null)
            PlaySkillEffect(_ShadowEffect);

        this.enabled = true;
        return true;
    }

    public virtual void PauseSkill()
    { }

    public virtual void ResumeSkill()
    { }

    public virtual void FinishSkill()
    {
        _MotionManager.FinishSkill(this);
    }

    public virtual void FinishSkillImmediately()
    {
        if (_Effect != null)
            _MotionManager.StopSkillEffect(_Effect);

        this.enabled = false;
        _MotionManager.ResetMove();
    }

    public virtual void AnimEvent(string function, object param)
    {
        switch (function)
        {
            case AnimEventManager.ANIMATION_END:
                FinishSkill();
                break;
            case AnimEventManager.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimEventManager.COLLIDER_END:
                ColliderEnd(param);
                break;
        }
    }

    #region performance

    private EffectController _ShadowEffect;

    protected float _SkillActSpeed = -1;
    protected float SkillActSpeed
    {
        get
        {
            if (_SkillActSpeed < 0)
            {
                _SkillActSpeed = _MotionManager.RoleAttrManager.SkillSpeed;
                if (_SkillAttr != null)
                {
                    _SkillActSpeed += _SkillAttr.SpeedAdd;
                }
            }
            return _SkillActSpeed;
        }
    }

    protected void PlayAnimation(AnimationClip anim)
    {
        _MotionManager.PlayAnimation(anim, (SkillActSpeed) * _SkillBaseSpeed);
    }

    protected void PlaySkillEffect(EffectController effect)
    {
        _MotionManager.PlaySkillEffect(effect, SkillActSpeed, _EffectElement);
    }

    protected void StopSkillEffect(EffectController effect)
    {
        _MotionManager.StopSkillEffect(effect);
    }

    protected virtual float GetTotalAnimLength()
    {
        return GetAnimNextInputLength(_Anim);
    }

    protected float GetAnimNextInputLength(AnimationClip anim)
    {
        foreach (var animEvent in anim.events)
        {
            if (animEvent.functionName == AnimEventManager.NEXT_INPUT_START)
            {
                return animEvent.time / SkillActSpeed;
            }
        }
        return anim.length;
    }

    

    #endregion  

    #region collider 

    private Dictionary<int, List<SelectBase>> _ColliderControl = new Dictionary<int, List<SelectBase>>();

    private void InitCollider(RoleAttrManager.SkillAttr skillAttr)
    {
        var collidercontrollers = gameObject.GetComponentsInChildren<SelectBase>(true);
        foreach (var collider in collidercontrollers)
        {
            //if (!IsColliderCanAct(collider.gameObject.name))
            //    continue;

            collider.Init();
            if (_ColliderControl.ContainsKey(collider._ColliderID))
            {
                _ColliderControl[collider._ColliderID].Add(collider);
            }
            else
            {
                _ColliderControl.Add(collider._ColliderID, new List<SelectBase>());
                _ColliderControl[collider._ColliderID].Add(collider);
            }

            if (skillAttr != null)
            {
                if (collider is SelectCollider)
                {
                    InitColliderRange(collider);
                }

                InitColliderDamage(collider);

            }
        }
    }

    protected virtual void ColliderStart(object param)
    {
        int index = (int)param;

        if (_ColliderControl != null && _ColliderControl.ContainsKey(index))
        {
            foreach (var collider in _ColliderControl[index])
            {
                collider.ColliderStart();
            }
        }

    }

    protected virtual void ColliderEnd(object param)
    {
        int index = (int)param;
        if (_ColliderControl != null && _ColliderControl.ContainsKey(index))
        {
            foreach (var collider in _ColliderControl[index])
            {
                collider.ColliderFinish();
            }
        }
    }

    #endregion

    #region skill enhance

    private void InitColliderRange(SelectBase select)
    {
        var capsuleCollider = select.GetComponent<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.transform.localPosition = Vector3.zero;
            capsuleCollider.transform.localRotation = Quaternion.Euler(Vector3.zero);

            capsuleCollider.radius = capsuleCollider.radius * (1 + _SkillAttr.RangeAdd);
            capsuleCollider.height = capsuleCollider.height * (1 + _SkillAttr.RangeLengthAdd) + _SkillAttr.BackRangeAdd;
            capsuleCollider.direction = 2;
            float centerZ = capsuleCollider.height * 0.5f;
            if (_SkillAttr.BackRangeAdd > 0)
            {
                centerZ = capsuleCollider.height * 0.5f - _SkillAttr.BackRangeAdd;
            }

            capsuleCollider.center = new Vector3(0, capsuleCollider.radius * 0.5f, centerZ);
        }
    }

    private void InitColliderDamage(SelectBase select)
    {
        var damages = select.GetComponentsInChildren<ImpactDamage>();
        foreach (var damageImpact in damages)
        {
            damageImpact._DamageRate = damageImpact._DamageRate * (1 + _SkillAttr.DamageRateAdd);
        }
    }

    private void InitShadowEffect()
    {
        if (_SkillAttr.ShadowWarriorCnt <= 0)
            return;

        float actSpeed = _MotionManager.RoleAttrManager.SkillSpeed;
        if (_SkillAttr != null)
        {
            actSpeed += _SkillAttr.SpeedAdd;
        }
        var shadowEffect = GameBase.ResourceManager.Instance.GetInstanceGameObject("Effect/Skill/Effect_Char_AfterAnim");
        var shadowScript = shadowEffect.GetComponent<EffectAfterAnim>();
        shadowScript._Duration = GetTotalAnimLength();
        shadowScript._Interval = 0.1f;
        shadowScript._FadeOut = 0.1f * _SkillAttr.ShadowWarriorCnt;
        _ShadowEffect = shadowScript;

        var damages = GetComponentsInChildren<ImpactDamage>();
        foreach (var damageImpact in damages)
        {
            for (int i = 0; i < _SkillAttr.ShadowWarriorCnt; ++i)
            {
                var damageDelay = damageImpact.gameObject.AddComponent<ImpactDamageDelay>();
                damageDelay._DamageRate = damageImpact._DamageRate * _SkillAttr.ShadowWarriorDamageRate;
                damageDelay._DelayTime = 0.1f * (1 + i);

                var hitDelay = damageImpact.gameObject.AddComponent<ImpactHitDelay>();
                hitDelay._HitTime = 0.1f;
                hitDelay._HitEffect = -1;
                hitDelay._DelayTime = 0.1f * (1 + i);
            }
        }
    }

    private static AnimationClip _AccumulateAnim;
    protected bool IsAccumulateSkill()
    {
        if (_SkillAttr == null)
            return false;

        if (_AccumulateAnim == null)
        {
            //if (GameLogic.PlayerDataPack.Instance._SelectedRole.Profession == Tables.PROFESSION.GIRL_DEFENCE
            //    || GameLogic.PlayerDataPack.Instance._SelectedRole.Profession == Tables.PROFESSION.GIRL_DOUGE)
                _AccumulateAnim = GameBase.ResourceManager.Instance.GetAnimationClip("Animation/Girl/Act_S_Skill_Accumulate");
            _MotionManager.InitAnimation(_AccumulateAnim);
        }
        if (_AccumulateAnim == null)
        {
            Debug.LogError("Load AccumulateAnim error!!!");
            return false;
        }

        if (_SkillAttr.AccumulateTime > 0 && InputManager.Instance.IsKeyHold(_ActInput))
        {
            return true;
        }
        return false;
    }

    private float _AccumulateTime;
    private void StartSkillAccumulate()
    {
        PlayAnimation(_AccumulateAnim);
        
        _AccumulateTime = Time.time;

        StartCoroutine(SkillAccumulateUpdate());
    }

    private IEnumerator SkillAccumulateUpdate()
    {
        yield return new WaitForSeconds(0.05f);
        if (!InputManager.Instance.IsKeyHold(_ActInput))
        {
            ActSkill();
            yield break;
        }

        if (Time.time - _AccumulateTime > _SkillAttr.AccumulateTime)
        {
            ActSkill();
            yield break;
        }

        StartCoroutine(SkillAccumulateUpdate());
    }

    #endregion

    #region element

    private static string _EleImpactBaseStr = "EleImpact";
    private static string _EleImpactFireStr = "EleImpactFire";
    private static string _EleImpactColdStr = "EleImpactCold";
    private static string _EleImpactLightingStr = "EleImpactLighting";
    private static string _EleImpactWindStr = "EleImpactWind";

    private string _CurEleImpact = "";
    private ElementType _ImpactElement;
    private ElementType _EffectElement;
    private ElementType _HitElement;

    public void SetImpactElement(ElementType eleType)
    {
        _ImpactElement = eleType;
        _CurEleImpact = "";
        switch (eleType)
        {
            case ElementType.Fire:
                _CurEleImpact = _EleImpactFireStr;
                break;
            case ElementType.Cold:
                _CurEleImpact = _EleImpactColdStr;
                break;
            case ElementType.Lighting:
                _CurEleImpact = _EleImpactLightingStr;
                break;
            case ElementType.Wind:
                _CurEleImpact = _EleImpactWindStr;
                break;
        }
    }

    public void SetEffectElement(ElementType eleType)
    {
        _EffectElement = eleType;
    }

    public void SetHitEffectElement(ElementType eleType)
    {
        _HitElement = eleType;
    }

    private bool IsColliderCanAct(string colliderName)
    {
        if (colliderName.Contains(_EleImpactBaseStr))
        {
            if (string.IsNullOrEmpty(_CurEleImpact))
                return false;

            if (colliderName == _CurEleImpact)
                return true;

            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

}
