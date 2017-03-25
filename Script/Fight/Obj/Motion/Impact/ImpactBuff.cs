using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuff : ImpactBase
{
    public float _LastTime;
    public EffectController _BuffEffect;

    protected MotionManager _BuffSender;
    protected EffectController _DynamicEffect;

    protected Dictionary<MotionManager, ImpactBuff> _ReciverDict = new Dictionary<MotionManager, ImpactBuff>();

    public override sealed void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _BuffSender = senderManager;
        var dynamicBuff = reciverManager.AddBuff(this);

        _ReciverDict.Add(reciverManager, dynamicBuff);
    }

    public override void RemoveImpact(MotionManager reciverManager)
    {
        base.RemoveImpact(reciverManager);

        if (_ReciverDict.ContainsKey(reciverManager))
        {
            reciverManager.RemoveBuff(_ReciverDict[reciverManager]);
        }
    }

    public void ActBuff(MotionManager reciverManager)
    {
        ActBuff(_BuffSender, reciverManager);
    }

    public virtual void ActBuff(MotionManager senderManager, MotionManager ownerManager)
    {
        if (_BuffEffect != null)
        {
            _DynamicEffect = ownerManager.PlayDynamicEffect(_BuffEffect);
        }
        if(_LastTime > 0)
            StartCoroutine(TimeOut(ownerManager));
    }

    public virtual void RemoveBuff(MotionManager ownerManager)
    {
        if (_DynamicEffect != null)
        {
            ownerManager.StopDynamicEffectImmediately(_DynamicEffect);
        }
    }

    public IEnumerator TimeOut(MotionManager ownerManager)
    {
        yield return new WaitForSeconds(_LastTime);
        ownerManager.RemoveBuff(this);
    }
}
