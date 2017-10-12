using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class ImpactNoCollider : ImpactBuff
{

    private ObstacleAvoidanceType _OrgAvoidType;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        reciverManager.TriggerCollider.enabled = false;
        _OrgAvoidType = reciverManager.NavAgent.obstacleAvoidanceType;
        reciverManager.NavAgent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        reciverManager.TriggerCollider.enabled = true;
        reciverManager.NavAgent.obstacleAvoidanceType = _OrgAvoidType;
    }
}
