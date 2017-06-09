using UnityEngine;
using System.Collections;

public class ImpactNoCollider : ImpactBuff
{


    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        reciverManager.TriggerCollider.enabled = false;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        reciverManager.TriggerCollider.enabled = true;
    }
}
