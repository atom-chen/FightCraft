using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBase : MonoBehaviour
{
    public virtual void Init()
    {
        InitCollider();
        _MotionManager = gameObject.GetComponentInParent<MotionManager>();

        if (_Anim != null)
        {
            _MotionManager.InitAnimation(_Anim);
            _MotionManager.AddAnimationEndEvent(_Anim);
        }   
    }

    public AnimationClip _Anim;
    public EffectController _Effect;
    public string _ActInput;
    public int _SkillMotionPrior = 100;

    protected MotionManager _MotionManager;
    protected float _SkillLastTime;
    private Dictionary<int, List<SelectBase>> _ColliderControl = new Dictionary<int, List<SelectBase>>();

    private void InitCollider()
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
        }

    }

    public virtual bool IsCanActSkill()
    {
        if (_MotionManager.MotionPrior < _SkillMotionPrior)
            return true;

        return false;
    }

    public virtual bool ActSkill()
    {
        if (!IsCanActSkill())
            return false;

        if(_Anim != null)
            _MotionManager.PlayAnimation(_Anim);
        if(_Effect != null)
            _MotionManager.PlaySkillEffect(_Effect);

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

    #region element

    private static string _EleImpactBaseStr = "EleImpact";
    private static string _EleImpactFireStr = "EleImpactFire";
    private static string _EleImpactColdStr = "EleImpactCold";
    private static string _EleImpactLightingStr = "EleImpactLighting";
    private static string _EleImpactWindStr = "EleImpactWind";

    private string _CurEleImpact = "";
    public void SetImpactElement(ElementType eleType)
    {
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
