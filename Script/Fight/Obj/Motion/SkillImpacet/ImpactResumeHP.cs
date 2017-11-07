using UnityEngine;
using System.Collections;

public class ImpactResumeHP: ImpactBase
{
    public int _HPValue = 0;
    public float _HPPersent = 0;


    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.RoleAttrManager.AddHP(_HPValue);
        reciverManager.RoleAttrManager.AddHPPersent(_HPPersent);
    }

}
