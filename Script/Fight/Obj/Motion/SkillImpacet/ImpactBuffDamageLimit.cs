using UnityEngine;
using System.Collections;

public class ImpactBuffDamageLimit : ImpactBuff
{
    public float _LimitHPPersent = 0.33f;

    public override int DamageModify(int orgDamage, ImpactBase damageImpact)
    {
        int hpPersent = (int)(_BuffOwner.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax) * _LimitHPPersent);
        if (orgDamage > hpPersent)
            return hpPersent;

        return base.DamageModify(orgDamage, damageImpact);
    }
}
