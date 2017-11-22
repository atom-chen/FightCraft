﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCollider : SelectBase
{
    #region exImpacts

    protected List<ExImpactBase> _ExHitEnemyImpacts = new List<ExImpactBase>();
    public List<ExImpactBase> ExHitEnemyImpacts
    {
        get
        {
            return _ExHitEnemyImpacts;
        }
        set
        {
            _ExHitEnemyImpacts = value;
        }
    }
    public void ActExHitEnemyImpacts(MotionManager targetMotion)
    {
        foreach (var hitImpact in _ExHitEnemyImpacts)
        {
            hitImpact.ActExImpact(_ObjMotion, targetMotion);
        }
    }

    protected List<ExImpactBase> _ExHitSelfImpacts = new List<ExImpactBase>();
    public List<ExImpactBase> ExHitSelfImpacts
    {
        get
        {
            return _ExHitSelfImpacts;
        }
        set
        {
            _ExHitSelfImpacts = value;
        }
    }

    public void ActExHitSelfImpacts()
    {
        foreach (var hitImpact in _ExHitEnemyImpacts)
        {
            hitImpact.ActExImpact(_ObjMotion, _ObjMotion);
        }
    }

    #endregion

    public List<int> _ColliderFinishFrame;

    protected List<MotionManager> _TrigMotions= new List<MotionManager>();
    public List<MotionManager> TrigMotions
    {
        get
        {
            return _TrigMotions;
        }
    }

    protected Collider _Collider;
    public Collider Collider
    {
        get
        {
            return _Collider;
        }
    }

    public bool _SelectLieObj;

    public override void Init()
    {
        base.Init();
        
    }

    public override void ModifyColliderRange(float rangeModify)
    {
        base.ModifyColliderRange(rangeModify);

        _Collider = gameObject.GetComponent<Collider>();
        if (_Collider is CapsuleCollider)
        {
            var capsuleCollider = _Collider as CapsuleCollider;
            capsuleCollider.radius = capsuleCollider.radius * (1 + rangeModify);
            capsuleCollider.height = capsuleCollider.height * (1 + rangeModify);
            if (capsuleCollider.direction == 1)
            {
                capsuleCollider.center = new Vector3(0, capsuleCollider.height * 0.5f, capsuleCollider.center.z);
            }
            else if (capsuleCollider.direction == 2)
            {
                capsuleCollider.center = new Vector3(0, capsuleCollider.radius * 0.5f, capsuleCollider.center.z * (1 + rangeModify));
            }
        }
    }

    public override void RegisterEvent()
    {
        base.RegisterEvent();

        for (int i = 0; i < _ColliderFinishFrame.Count; ++i)
        {
            var anim = _ObjMotion.Animation.GetClip(_EventAnim.name);
            _ObjMotion.AnimationEvent.AddSelectorFinishEvent(anim, _ColliderFinishFrame[i], _ColliderID);
        }
    }

    public override void ColliderStart()
    {
        if (_Collider == null)
        {
            _Collider = gameObject.GetComponent<Collider>();
        }
        _Collider.enabled = true;
        base.ColliderStart();

        //Debug.Log("ColliderStart:" + _ColliderID);
    }

    public override void ColliderFinish()
    {
        base.ColliderFinish();
        if(_Collider != null)
            _Collider.enabled = false;
        _TrigMotions.Clear();
    }

    public override void ColliderStop()
    {
        ColliderFinish();
    }

    void OnTriggerStay(Collider other)
    {
        var motion = other.gameObject.GetComponentInParent<MotionManager>();
        if (motion == null)
            return;

        TriggerMotion(motion);
    }

    protected virtual void TriggerMotion(MotionManager motion)
    {
        if (!_TrigMotions.Contains(motion))
        {
            _TrigMotions.Add(motion);

            if (motion._StateLie == motion._ActionState && !_SelectLieObj)
                return;

            if (!motion._CanBeSelectByEnemy)
                return;

            {
                foreach (var impact in _ImpactList)
                {
                    impact.ActImpact(_ObjMotion, motion);
                }
                ActExHitSelfImpacts();
                ActExHitEnemyImpacts(motion);

                if (_IsRemindSelected && _ObjMotion.ActingSkill != null)
                {
                    if (!_ObjMotion.ActingSkill._SkillHitMotions.Contains(motion))
                        _ObjMotion.ActingSkill._SkillHitMotions.Add(motion);
                }

                if (_ObjMotion._IsRoleHit)
                {
                    GlobalEffect.Instance.Pause(_ObjMotion._RoleHitTime);
                }
            }
        }
    }

}
