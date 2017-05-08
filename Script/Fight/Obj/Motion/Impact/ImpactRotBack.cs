﻿using UnityEngine;
using System.Collections;

public class ImpactRotBack : ImpactBase
{
    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        if (reciverManager.ActingSkill is ObjMotionSkillDefence)
            return;

        reciverManager.SetRotate(reciverManager.transform.rotation.eulerAngles + new Vector3(0, 180, 0));
        
    }

}