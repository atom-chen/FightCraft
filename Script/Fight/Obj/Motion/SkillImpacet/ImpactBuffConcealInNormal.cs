﻿using UnityEngine;
using System.Collections;

public class ImpactBuffConcealInNormal : ImpactBuff
{
    #region 

    public EffectController _ConcealActEffect;

    private EffectController _DynamicConcealEffect;
    #endregion

    public override void ActBuff(MotionManager senderManager, MotionManager ownerManager)
    {
        base.ActBuff(senderManager, ownerManager);
        if (_ConcealActEffect != null)
        {
            _DynamicConcealEffect = ownerManager.PlayDynamicEffect(_ConcealActEffect);
        }
    }

    public override void UpdateBuff()
    {
        base.UpdateBuff();

        if (_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
        {
            _DynamicConcealEffect.PlayEffect();
        }
        else
        {
            _DynamicConcealEffect.HideEffect();
        }
    }

    public override bool IsBuffCanHit(ImpactHit damageImpact)
    {
        if(_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
        return false;

        return true;
    }

    public override bool IsBuffCanCatch(ImpactCatch damageImpact)
    {
        if (_BuffOwner._ActionState == _BuffOwner._StateIdle
            || _BuffOwner._ActionState == _BuffOwner._StateMove)
            return false;

        return true;
    }
}
