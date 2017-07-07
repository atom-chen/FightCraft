using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterHittedTarget : BulletEmitterBase
{
    public float _DelayTime;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var skillBase = transform.parent.GetComponent<ObjMotionSkillBase>();
        if (skillBase == null)
            return;

        foreach (var hitMotion in skillBase._SkillHitMotions)
        {
            var bullet = InitBulletGO<BulletBase>();
            bullet.transform.position = hitMotion.transform.position;
        }

        skillBase._SkillHitMotions.Clear();
    }

}
