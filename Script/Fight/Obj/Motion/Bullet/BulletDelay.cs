using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletDelay : BulletBase
{
    public float _DelayTime = 1;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        Invoke("BulletFinish", 1);
    }
}
