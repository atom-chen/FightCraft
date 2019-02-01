using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffQiLinHitSub : ImpactBuffBeHitSub
{
    public int _ActTimes = 3;

    private EffectQiLinIceBuff _QiLinIceBuff;
    private int _ActedTimes;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _ActedTimes = _ActTimes;
        if (_DynamicEffect != null)
        {
            _QiLinIceBuff = _DynamicEffect.GetComponent<EffectQiLinIceBuff>();
            if (_QiLinIceBuff != null)
            {
                _QiLinIceBuff.PlayEffectCnt(_ActTimes);
            }
        }
    }

    public override void HitAct(MotionManager hitSender, ImpactHit hitImpact)
    {
        base.HitAct(hitSender, hitImpact);

        ++_ActedTimes;
        _QiLinIceBuff.DecEffect();

        if (_ActedTimes == 0)
        {
            _BuffOwner.RemoveBuff(this);
        }
    }

}
