using UnityEngine;
using System.Collections;

public class ImpactHitForwardFixed : ImpactHitForward
{
    public Vector3 _Offset;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        if (reciverManager.ActingSkill is ObjMotionSkillDefence)
            return;

        var destPos = senderManager.transform.position + _Offset.x * senderManager.transform.forward + _Offset.y * senderManager.transform.up + _Offset.z * senderManager.transform.right;
        Vector3 destMove = destPos - reciverManager.transform.position;
        var time = destMove.magnitude / _Speed;
        reciverManager.SetMove(destMove, time);
    }

}
