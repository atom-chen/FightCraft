using UnityEngine;
using System.Collections;

public class ImpactBuffConceal : ImpactBuff
{
    #region 


    #endregion

    private float _OrgNavRadius = 0;
    
    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        reciverManager.TriggerCollider.enabled = false;
        _OrgNavRadius = reciverManager.NavAgent.radius;
        Debug.Log("_OrgNavRadius:" + _OrgNavRadius);
        reciverManager.NavAgent.radius = 0;
        reciverManager._CanBeSelectByEnemy = false;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        Debug.Log("_OrgNavRadius:" + _OrgNavRadius);
        reciverManager.NavAgent.radius = _OrgNavRadius;
        reciverManager.TriggerCollider.enabled = true;
        reciverManager._CanBeSelectByEnemy = true;
    }
}
