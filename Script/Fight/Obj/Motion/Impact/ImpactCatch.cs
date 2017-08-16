using UnityEngine;
using System.Collections;

public class ImpactCatch : ImpactHit
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        //base.ActImpact(senderManager, reciverManager);

        CatchMotion(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

    protected virtual void CatchMotion(MotionManager senderManager, MotionManager reciverManager)
    {
        reciverManager.BaseMotionManager.CatchEvent(_HitTime, _HitEffect, senderManager, this);
    }

}
