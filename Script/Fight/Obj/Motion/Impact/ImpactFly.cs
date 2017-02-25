using UnityEngine;
using System.Collections;

public class ImpactFly : ImpactBase
{
    public float _FlyHeight = 0.6f;
    public int _HitEffect = 0;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        Hashtable hash = new Hashtable();
        hash.Add("FlyHeight", _FlyHeight);
        hash.Add("HitEffect", _HitEffect);
        reciverManager._EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, senderManager, hash);
    }

}
