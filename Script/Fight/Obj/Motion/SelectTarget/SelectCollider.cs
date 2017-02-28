using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCollider : SelectBase
{
    private List<Collider> _TrigCollider = new List<Collider>();

    public override void ColliderFinish()
    {
        base.ColliderFinish();

        _TrigCollider.Clear();
    }

    void OnTriggerStay(Collider other)
    {
        
        if (!_TrigCollider.Contains(other))
        {
            _TrigCollider.Add(other);
            
            var motion = other.gameObject.GetComponentInParent<MotionManager>();
            if (motion != null)
            {
                foreach (var impact in _ImpactList)
                {
                    impact.ActImpact(_SkillMotion, motion);
                }

                if (_SkillMotion._IsRoleHit)
                {
                    GlobalEffect.Instance.Pause(0.05f);
                }
            }
        }
    }

}
