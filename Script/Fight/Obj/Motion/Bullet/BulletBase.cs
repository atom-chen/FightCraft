using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletBase : MonoBehaviour
{
    protected ImpactBase[] _ImpactList;
    protected MotionManager _SkillMotion;

    public virtual void Init(MotionManager senderMotion)
    {
        _SkillMotion = senderMotion;
        _ImpactList = gameObject.GetComponents<ImpactBase>();
    }

    protected virtual void BulletHit(MotionManager hitMotion)
    {
        foreach (var impact in _ImpactList)
        {
            impact.ActImpact(_SkillMotion, hitMotion);
        }
    }

    protected virtual void BulletFinish()
    {
        ResourcePool.Instance.RecvIldeBullet(this);
    }
}
