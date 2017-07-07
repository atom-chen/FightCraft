using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCollider : SelectBase
{
    private List<Collider> _TrigCollider = new List<Collider>();

    private Collider _SelectCollider;

    public override void ColliderStart()
    {
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
            if (motion != null && motion._CanBeSelectByEnemy)
            {
                foreach (var impact in _ImpactList)
                {
                    impact.ActImpact(_SkillMotion, motion);
                }

                if (_IsRemindSelected)
                {
                    _SkillMotion.ActingSkill._SkillHitMotions.Add(motion);
                }

                if (_SkillMotion._IsRoleHit)
                {
                    GlobalEffect.Instance.Pause(_SkillMotion._RoleHitTime);
                }
            }
        }
    }

}
