using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterTargetDelay : BulletEmitterBase
{
    public float _DelayTime;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        StartCoroutine(BulletDelay(reciverManager));
    }

    private IEnumerator BulletDelay(MotionManager reciverManager)
    {
        yield return new WaitForSeconds(_DelayTime);

        var bullet = InitBulletGO<BulletBase>();
        bullet.transform.position = reciverManager.transform.position;
    }
}
