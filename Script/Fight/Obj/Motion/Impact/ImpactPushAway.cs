using UnityEngine;
using System.Collections;

public class ImpactPushAway : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        if (reciverManager.ActingSkill is ObjMotionSkillDefence)
            return;

        Vector3 destMove = (reciverManager.transform.position - senderManager.transform.position).normalized * _Speed * _Time;
        Debug.Log("DestMove:" + destMove);
        reciverManager.SetMove(destMove, _Time / senderManager.RoleAttrManager.AttackSpeed);
    }

}
