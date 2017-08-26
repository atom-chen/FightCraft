﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCollider : SelectBase
{
    private List<Collider> _TrigCollider = new List<Collider>();

    private Collider _SelectCollider;

    public bool _SelectLieObj;

    public override void ColliderStart()
    {
        Debug.Log("ColliderStart:" + _ColliderID);
        if (_SelectCollider == null)
        {
            _SelectCollider = gameObject.GetComponent<Collider>();
        }
        _SelectCollider.enabled = true;
        base.ColliderStart();
    }

    public override void ColliderFinish()
    {
        base.ColliderFinish();
        _SelectCollider.enabled = false;
        _TrigCollider.Clear();
    }

    void OnTriggerStay(Collider other)
    {
        
        if (!_TrigCollider.Contains(other))
        {
            _TrigCollider.Add(other);
            
            var motion = other.gameObject.GetComponentInParent<MotionManager>();
            if (motion == null)
                return;

            if (motion.MotionPrior == BaseMotionManager.LIE_PRIOR && !_SelectLieObj)
                return;

            if (!motion._CanBeSelectByEnemy)
                return;

            {
                foreach (var impact in _ImpactList)
                {
                    impact.ActImpact(_ObjMotion, motion);
                }

                if (_IsRemindSelected && _ObjMotion.ActingSkill != null)
                {
                    if(!_ObjMotion.ActingSkill._SkillHitMotions.Contains(motion))
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
