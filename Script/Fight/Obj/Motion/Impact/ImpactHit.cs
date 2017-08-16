using UnityEngine;
using System.Collections;

public class ImpactHit : ImpactDamage
{
    public float _HitTime = 0.6f;
    public int _HitEffect = 0;
    public bool _IsBulletHit = false;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        HitMotion(senderManager, reciverManager);
    }

    protected virtual void HitMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        reciverManager.BaseMotionManager.HitEvent(_HitTime, _HitEffect, senderManager, this);
    }

}
