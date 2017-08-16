using UnityEngine;
using System.Collections;

public class ImpactCatch : ImpactBase
{
    public float _HitTime = 0.6f;
    public int _HitEffect = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        CatchMotion(senderManager, reciverManager);
    }

    protected virtual void CatchMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        reciverManager.BaseMotionManager.CatchEvent(_HitTime, _HitEffect, senderManager);
    }

}
