using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectCollider : SelectBase
{
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

    public override void ColliderStart()
    {
        if (_Collider == null)
        {
            _Collider = gameObject.GetComponent<Collider>();
        }
        _Collider.enabled = true;
        base.ColliderStart();
    }

    public override void ColliderFinish()
    {
        base.ColliderFinish();
        _Collider.enabled = false;
        _TrigMotions.Clear();
    }

    void OnTriggerStay(Collider other)
    {
        var motion = other.gameObject.GetComponentInParent<MotionManager>();
        if (motion == null)
            return;

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
