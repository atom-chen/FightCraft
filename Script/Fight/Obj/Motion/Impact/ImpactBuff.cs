using UnityEngine;
using System.Collections;

public class ImpactBuff : ImpactBase
{
    public float _LastTime;
    public EffectController _BuffEffect;

    protected MotionManager _BuffSender;
    protected MotionManager _ReciverManager;
    protected EffectController _DynamicEffect;

    public override sealed void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _BuffSender = senderManager;
        _ReciverManager = reciverManager;
        _ReciverManager.AddBuff(this);
    }

    public void ActBuff()
    {
        ActBuff(_BuffSender, _ReciverManager);
    }

    public virtual void ActBuff(MotionManager senderManager, MotionManager ownerManager)
    {
        if (_BuffEffect != null)
        {
            _DynamicEffect = _ReciverManager.PlayDynamicEffect(_BuffEffect);
        }
        StartCoroutine(TimeOut());
    }

    public virtual void RemoveBuff()
    {
        if (_DynamicEffect != null)
        {
            _ReciverManager.StopDynamicEffectImmediately(_DynamicEffect);
        }
    }

    public IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(_LastTime);
        _ReciverManager.RemoveBuff(this);
    }
}
