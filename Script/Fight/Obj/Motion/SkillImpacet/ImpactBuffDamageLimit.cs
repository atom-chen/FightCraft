using UnityEngine;
using System.Collections;

public class ImpactBuffDamageLimit : ImpactBuff
{
    public override int DamageModify(int orgDamage, ImpactBase damageImpact)
    {
        int hpPersent = (int)(_BuffOwner.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax) * 0.333f);
        if (orgDamage > hpPersent)
            return hpPersent;

        return base.DamageModify(orgDamage, damageImpact);
    }
}
