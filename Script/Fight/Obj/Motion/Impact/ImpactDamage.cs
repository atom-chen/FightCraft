using UnityEngine;
using System.Collections;

public class ImpactDamage : ImpactBase
{
    public float _DamageRate = 1;


    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        ProcessDamge(senderManager, reciverManager);
    }

    protected virtual void ProcessDamge(MotionManager senderManager, MotionManager reciverManager)
    {
        senderManager.RoleAttrManager.SendDamageEvent(reciverManager, _DamageRate, this);
    }

}
