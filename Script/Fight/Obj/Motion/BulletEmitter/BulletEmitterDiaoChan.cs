using UnityEngine;
using System.Collections;

public class BulletEmitterDiaoChan : BulletEmitterBase
{
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var target = GameObject.FindGameObjectWithTag("Player");
        var targetMotion = target.GetComponent<MotionManager>();

        var bullet = InitBulletGO<BulletDaiJiHeart>();

        bullet.transform.position = targetMotion.transform.position;
    }
}
