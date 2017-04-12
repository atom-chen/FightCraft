using UnityEngine;
using System.Collections;

public class ImpactPlayEffect : ImpactBase
{
    public EffectController _EffectController;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        reciverManager.PlaySkillEffect(_EffectController);
    }

}
