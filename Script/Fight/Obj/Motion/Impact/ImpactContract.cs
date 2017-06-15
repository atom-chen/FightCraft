using UnityEngine;
using System.Collections;

public class ImpactContract : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        Vector3 destMove = (senderManager.transform.position - reciverManager.transform.position).normalized * _Speed * _Time;
        Debug.Log("DestMove:" + destMove);
        reciverManager.SetMove(destMove, _Time);
    }

}
