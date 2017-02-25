using UnityEngine;
using System.Collections;

public class ImpactBuff : ImpactBase
{
    public float _LastTime;
    public EffectController _BuffEffect;

    protected MotionManager _ReciverManager;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _ReciverManager = reciverManager;
        _ReciverManager.AddBuff(this);
        _BuffEffect.PlayEffect();
        StartCoroutine(TimeOut());
    }

    public virtual void RemoveBuff()
    {
        _BuffEffect.HideEffect();
    }

    public IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(_LastTime);
        _ReciverManager.RemoveBuff(this);
    }
}
