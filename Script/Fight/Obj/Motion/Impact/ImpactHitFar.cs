using UnityEngine;
using System.Collections;

public class ImpactHitFar : ImpactHitForward
{
    public float _MaxDis = 2;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        Vector3 direct = reciverManager.transform.position - senderManager.transform.position;
        float length = direct.magnitude - _MaxDis;
        if (length > 0)
            return;

        HitMotion(senderManager, reciverManager);
        if (reciverManager.BaseMotionManager.IsCanBePush())
        {
            Vector3 destMove = senderManager.transform.forward.normalized * _Speed * _Time;

            reciverManager.SetMove(destMove, _Time / senderManager.RoleAttrManager.AttackSpeed);
        }
    }

}
