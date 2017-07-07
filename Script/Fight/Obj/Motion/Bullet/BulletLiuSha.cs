using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLiuSha : BulletBase
{
    public float _LastTime = 10;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion,emitterBase);

        StartCoroutine(StartHit());
    }

    IEnumerator StartHit()
    {
        yield return new WaitForSeconds(_LastTime);

        BulletFinish();
    }
    
    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);
    }

    void OnTriggerExit(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        foreach (var impact in _ImpactList)
        {
            impact.RemoveImpact(targetMotion);
        }
    }

}
