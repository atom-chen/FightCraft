using UnityEngine;
using System.Collections;

public class ImpactBuffShield : ImpactBuff
{
    public EffectController _HitEffect;
    public float _ShieldValueFromHPMaxPersent;

    private int _ShieldValue;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _ShieldValue = (int)(reciverManager.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax) * _ShieldValueFromHPMaxPersent);
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);
    }

    public override bool IsBuffCanHit(ImpactHit damageImpact)
    {
        return false;
    }

    public override bool IsBuffCanCatch(ImpactCatch damageImpact)
    {
        return false;
    }

    public override int DamageModify(int orgDamage, ImpactBase damageImpact)
    {
        _ShieldValue -= orgDamage;
        Debug.Log("_ShieldValue:" + _ShieldValue);
        if (_ShieldValue <= 0)
        {
            _BuffOwner.RemoveBuff(this);
        }

        return 0;
    }
}
