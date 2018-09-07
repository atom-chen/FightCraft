using UnityEngine;
using System.Collections;

public class ImpactBuffBlockCD : ImpactBuffCD
{
    public override bool IsBuffCanCatch(ImpactCatch damageImpact)
    {
        if (IsInCD())
            return true;
        else
            return false;
    }

    public override bool IsBuffCanHit(MotionManager impactSender, ImpactHit damageImpact)
    {
        if (IsInCD())
            return true;
        else
            return false;
    }

    public override void DamageModify(RoleAttrManager.DamageClass orgDamage, ImpactBase damageImpact)
    {
        if (IsInCD())
        {
            base.DamageModify(orgDamage, damageImpact);
        }
        else
        {
            SetCD();
        }

    }
}
