using UnityEngine;
using System.Collections;

public class ImpactHitDelay : ImpactBase
{
    public float _HitTime = 0.6f;
    public int _HitEffect = 0;
    public float _DelayTime = 0.1f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        StartCoroutine(ActDelay(senderManager, reciverManager));
    }

    private IEnumerator ActDelay(MotionManager senderManager, MotionManager reciverManager)
    {
        yield return new WaitForSeconds(_DelayTime);

        Hashtable hash = new Hashtable();
        hash.Add("HitTime", _HitTime);
        hash.Add("HitEffect", _HitEffect);
        reciverManager.EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, senderManager, hash);
    }

}
