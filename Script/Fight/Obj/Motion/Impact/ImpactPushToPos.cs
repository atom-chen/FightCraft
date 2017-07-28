using UnityEngine;
using System.Collections;

public class ImpactPushToPos : ImpactBase
{
    public float _Time = 0.6f;
    public float _Speed = 10.0f;

    private Vector3 _DestPos;
    public Vector3 DestPos
    {
        get
        {
            return _DestPos;
        }
        set
        {
            _DestPos = value;
        }
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        if (reciverManager.ActingSkill is ObjMotionSkillDefence)
            return;

        if (_DestPos != Vector3.zero)
        {
            reciverManager.SetMove(_DestPos - reciverManager.transform.position, _Time / senderManager.RoleAttrManager.AttackSpeed);
        }
        else
        {
            Vector3 destMove = senderManager.transform.forward.normalized * _Speed * _Time;
            reciverManager.SetMove(destMove, _Time / senderManager.RoleAttrManager.AttackSpeed);
        }
        _DestPos = Vector3.zero;
    }

}
