using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBase : MonoBehaviour
{
    public virtual void Init()
    {
        _MotionManager = gameObject.GetComponentInParent<MotionManager>();

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        _SkillAttr = _MotionManager.RoleAttrManager.GetSkillAttr(_ActInput);
        InitCollider(_SkillAttr);
        if (_SkillAttr != null)
        {
            SetEffectSize(1 + _SkillAttr.RangeAdd);
        }

        foreach (var anim in _NextAnim)
        {
            if (anim != null)
            {
                _MotionManager.InitAnimation(anim);
                _MotionManager.AddAnimationEndEvent(anim);
            }
        }
    }

    private int _CurStep;
    public List<AnimationClip> _NextAnim;
    public List<EffectController> _NextEffect;
    public string _ActInput;
    public int _SkillMotionPrior = 100;
    public float _SkillBaseSpeed = 1;
    public float SkillSpeed
    {
        get
        {
            return (SkillActSpeed) * _SkillBaseSpeed;
        }
    }

    protected MotionManager _MotionManager;
    public MotionManager MotionManager
    {
        get
        {
            return _MotionManager;
        }
    }

    protected RoleAttrManager.SkillAttr _SkillAttr;

    public virtual bool IsCanActSkill()
    {
        if (_MotionManager._ActionState == _MotionManager._StateIdle)
            return true;

        if (_MotionManager._ActionState == _MotionManager._StateMove)
            return true;

        if (_MotionManager._ActionState == _MotionManager._StateSkill)
        {
            if (_MotionManager.ActingSkill._SkillMotionPrior < _SkillMotionPrior)
                return true;
        }

        return false;
    }

    public bool StartSkill(Hashtable exHash = null)
    {
        if (!IsCanActSkill())
            return false;

        return ActSkill(exHash);
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
        foreach (var effect in _NextEffect)
        {
            _MotionManager.StopSkillEffect(effect);
        }

        this.enabled = false;
        _MotionManager.ResetMove();
        ColliderStop();
    }

    public virtual void AnimEvent(string function, object param)
    {
        switch (function)
        {
            case AnimEventManager.ANIMATION_END:
                PlayerNextAnim();
                break;
            case AnimEventManager.COLLIDER_START:
                ColliderStart(param);
                break;
            case AnimEventManager.COLLIDER_END:
                ColliderEnd(param);
                break;
        }
    }

    public virtual bool ActSkill(Hashtable exhash)
    {
        this.enabled = true;

        _CurStep = -1;
        PlayerNextAnim();

        return true;
    }

    public void PlayerNextAnim()
    {
        if (_CurStep + 1 == _NextAnim.Count)
        {
            FinishSkill();
        }
        else
        {
            ++_CurStep;
            PlayAnimation(_NextAnim[_CurStep]);

            if (_CurStep - 1 >= 0 && _NextEffect[_CurStep - 1] != null)
            {
                StopSkillEffect(_NextEffect[_CurStep - 1]);
            }

            if (_NextEffect.Count > _CurStep && _NextEffect[_CurStep] != null)
            {
                PlaySkillEffect(_NextEffect[_CurStep]);
            }

        }
    }

    #region performance

    protected float _SkillActSpeed = -1;
    protected float SkillActSpeed
    {
        get
        {
            //if (_SkillActSpeed < 0)
            {
                _SkillActSpeed = _MotionManager.RoleAttrManager.AttackSpeed;
                if (_SkillAttr != null)
                {
                    _SkillActSpeed += _SkillAttr.SpeedAdd;
                }
            }
            return _SkillActSpeed;
        }
    }

    protected virtual void SetEffectSize(float size)
    {
        foreach (var effect in _NextEffect)
        {
            if (effect == null)
                continue;

            effect._EffectSizeRate = (size);
        }
    }

    protected void PlayAnimation(AnimationClip anim)
    {
        _MotionManager.RePlayAnimation(anim, (SkillActSpeed) * _SkillBaseSpeed);
    }

    protected void PlaySkillEffect(EffectController effect)
    {
        _MotionManager.PlaySkillEffect(effect, (SkillActSpeed) * _SkillBaseSpeed);
    }

    protected void StopSkillEffect(EffectController effect)
    {
        _MotionManager.StopSkillEffect(effect);
    }

    public virtual float GetTotalAnimLength()
    {
        float totleTime = 0;
        foreach (var anim in _NextAnim)
        {
            totleTime += anim.length / SkillActSpeed;
        }
        return totleTime;
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

    public List<MotionManager> _SkillHitMotions = new List<MotionManager>();

    private Dictionary<int, List<SelectBase>> _ColliderControl = new Dictionary<int, List<SelectBase>>();

    protected void InitCollider(RoleAttrManager.SkillAttr skillAttr)
    {
        var collidercontrollers = gameObject.GetComponentsInChildren<SelectBase>(true);
        foreach (var collider in collidercontrollers)
        {
            collider.Init(skillAttr);
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

        foreach (var collider in collidercontrollers)
        {
            collider.RegisterEvent();
        }
    }

    protected virtual void ColliderStart(object param)
    {
        int index = (int)param;

        if (_ColliderControl != null && _ColliderControl.ContainsKey(index))
        {
            for (int i = 0; i< _ColliderControl[index].Count; ++i)
            {
                _ColliderControl[index][i].ColliderStart();
            }
        }

    }

    protected virtual void ColliderEnd(object param)
    {
        int index = (int)param;
        if (_ColliderControl != null && _ColliderControl.ContainsKey(index))
        {
            for (int i = 0; i < _ColliderControl[index].Count; ++i)
            {
                _ColliderControl[index][i].ColliderFinish();
            }
            
        }
    }

    protected virtual void ColliderStop()
    {
        foreach(var colliderPair in _ColliderControl)
        {
            for (int i = 0; i < colliderPair.Value.Count; ++i)
            {
                colliderPair.Value[i].ColliderStop();
            }

        }
    }

    #endregion

}
