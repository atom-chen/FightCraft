using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterSingle : BulletEmitterBase
{

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var bullet = InitBulletGO<BulletBase>();
    }
    
}
