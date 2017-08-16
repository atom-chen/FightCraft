using UnityEngine;
using System.Collections;

public class ImpactFly : ImpactHit
{
    public float _FlyHeight = 0.6f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        reciverManager.BaseMotionManager.FlyEvent(_FlyHeight, _HitEffect, senderManager, this);

        ProcessDamge(senderManager, reciverManager);
    }

}
