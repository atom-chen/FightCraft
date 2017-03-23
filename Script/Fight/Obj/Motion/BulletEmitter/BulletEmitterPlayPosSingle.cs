using UnityEngine;
using System.Collections;

public class BulletEmitterPlayPosSingle : BulletEmitterBase
{
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var target = GameObject.FindGameObjectWithTag("Player");
        var targetMotion = target.GetComponent<MotionManager>();

        var bullet = InitBulletGO<BulletBase>();

        bullet.transform.position = targetMotion.transform.position;
    }
}
