using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffBeHitSub : ImpactBuffSub
{
    public float _Interval = 1;

    private CapsuleCollider _Collider;

    public override void BeHit(MotionManager hitSender, ImpactHit hitImpact)
    {
        ActSubImpacts(_BuffSender, _BuffOwner);
    }
    
}
