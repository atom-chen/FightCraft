using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBase : ObjMotionBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);
        InitCollider();
        _MotionPriority = 100;
    }

    public override void PlayMotion(object go, Hashtable eventArgs)
    {
        base.PlayMotion(go, eventArgs);
    }

    public override bool ActiveInput(InputManager inputManager)
    {
        if (inputManager.IsKeyDown(_ActInput))
        {
            return ActSkill();
        }
        return false;
    }

    public override bool ContinueInput(InputManager inputManager)
    {
        return base.ContinueInput(inputManager);
    }

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case "ColliderStart":
                ColliderStart(param);
                break;
            case "ColliderFinish":
                ColliderFinish(param);
                break;
        }
    }

    protected override void InitEvent()
    {
        base.InitEvent();
    }

    #endregion

    public EffectController _Effect;
    public string _ActInput;
    public float _SkillLastTime = -1;
    public Dictionary<int, List<SelectBase>> _ColliderControl = new Dictionary<int, List<SelectBase>>();

    private void InitCollider()
    {
        var collidercontrollers = gameObject.GetComponentsInChildren<SelectBase>(true);
        foreach (var collider in collidercontrollers)
        {
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

    public virtual bool ActSkill()
    {
        if (!IsCanActiveMotion())
            return false;

        if (_SkillLastTime < 0)
        {
            _SkillLastTime = _AnimationClip.length;
        }
        _MotionManager.MotionStart(this);
        _MotionManager.PlayAnimation(_AnimationClip);
        if(_Effect != null)
            _Effect.PlayEffect(_MotionManager._RoleAttrManager.SkillSpeed);

        StartCoroutine(FinishSkill());

        return true;
    }

    protected virtual IEnumerator FinishSkill()
    {
        yield return new WaitForSeconds(_SkillLastTime);
        FinishSkillImmediately();
    }

    protected virtual void FinishSkillImmediately()
    {
        _MotionManager.MotionFinish(this);
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

    protected virtual void ColliderFinish(object param)
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

}
