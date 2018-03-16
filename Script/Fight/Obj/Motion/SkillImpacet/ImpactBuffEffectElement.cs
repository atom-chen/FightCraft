using UnityEngine;
using System.Collections;

public class ImpactBuffEffectElement : ImpactBuff
{
    public ElementType _EffectElement = ElementType.None;
    

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        if (_EffectElement == ElementType.None)
            return;

        foreach (var skillInfo in reciverManager._StateSkill._SkillMotions)
        {
            for (int i = 0; i < skillInfo.Value._NextEffect.Count; ++i)
            {
                if (skillInfo.Value._NextEffect[i] != null)
                {
                    skillInfo.Value._NextEffect[i].SetEffectColor(_EffectElement);
                }
            }
        }
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        foreach (var skillInfo in reciverManager._StateSkill._SkillMotions)
        {
            for (int i = 0; i < skillInfo.Value._NextEffect.Count; ++i)
            {
                if (skillInfo.Value._NextEffect[i] != null)
                    skillInfo.Value._NextEffect[i].SetEffectColor(ElementType.None);
            }
        }
    }
    
}
