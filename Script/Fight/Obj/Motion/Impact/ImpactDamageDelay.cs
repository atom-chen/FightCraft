using UnityEngine;
using System.Collections;

public class ImpactDamageDelay : ImpactBase
{
    public float _DamageRate = 1;
    public float _DelayTime = 0.1f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        
        StartCoroutine(ActDelay(senderManager, reciverManager));
    }

    private IEnumerator ActDelay(MotionManager senderManager, MotionManager reciverManager)
    {
        yield return new WaitForSeconds(_DelayTime);

        senderManager.RoleAttrManager.SendDamageEvent(reciverManager, _DamageRate, SkillMotion);
    }
}
