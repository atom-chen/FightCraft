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

    public override bool IsBuffCanHit(ImpactHit damageImpact)
    {
        if (IsInCD())
            return true;
        else
            return false;
    }

    public override int DamageModify(int orgDamage, ImpactBase damageImpact)
    {
        if (IsInCD())
        {
            return base.DamageModify(orgDamage, damageImpact);
        }
        else
        {
            SetCD();
            return 0;
        }

    }
}
