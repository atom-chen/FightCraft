using UnityEngine;
using System.Collections;

public class ImpactCatchFinish : ImpactBase
{
    public float _FlyHeight = 0.6f;
    public int _HitEffect = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.BaseMotionManager.FinishCatch();
    }

}
