using UnityEngine;
using System.Collections;

public class ImpactNoCollider : ImpactBuff
{


    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

    }

    public override void RemoveBuff()
    {
        base.RemoveBuff();

    }
}
