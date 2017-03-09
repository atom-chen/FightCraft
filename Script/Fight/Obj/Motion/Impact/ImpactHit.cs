using UnityEngine;
using System.Collections;

public class ImpactHit : ImpactBase
{
    public float _HitTime = 0.6f;
    public int _HitEffect = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        Hashtable hash = new Hashtable();
        hash.Add("HitTime", _HitTime);
        hash.Add("HitEffect", _HitEffect);
        reciverManager.EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, senderManager, hash);
    }

}
