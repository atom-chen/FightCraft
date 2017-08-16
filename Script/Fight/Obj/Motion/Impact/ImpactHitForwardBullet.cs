using UnityEngine;
using System.Collections;

public class ImpactHitForwardBullet : ImpactHit
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        //HitMotion(senderManager, reciverManager);

        if (reciverManager.BaseMotionManager.IsCanBePush())
        {
            Vector3 destMove = transform.forward.normalized * _Speed * _Time;

            reciverManager.SetMove(destMove, _Time);
        }
    }

}
