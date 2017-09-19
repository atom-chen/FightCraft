using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAvator : ImpactBase
{
    private EffectAfterAnim _AvatorEffect;

    private int _AvatorCnt;
    private float _AvatorDamage;

    public override void Init(RoleAttrManager.SkillAttr skillAttr, ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillAttr, skillMotion, selector);

        var avatorEffectGO = ResourceManager.Instance.GetEffect("Skill/Effect_Char_AfterAnim");
        _AvatorEffect = avatorEffectGO.GetComponent<EffectAfterAnim>();
        _AvatorCnt = skillAttr.ShadowWarriorCnt;
        _AvatorDamage = skillAttr.ShadowWarriorDamageRate;

        selector._EventAnim = skillMotion._NextAnim[0];
        selector._EventFrame.Add(0);
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        var effectTime = _SkillMotion.GetTotalAnimLength();
        _AvatorEffect._Duration = effectTime;
        _AvatorEffect._Interval = 0.05f;
        _AvatorEffect._FadeOut = 0.05f * _AvatorCnt;
        _SkillMotion.MotionManager.PlaySkillEffect(_AvatorEffect);
    }

    public override void StopImpact()
    {
        base.StopImpact();

        _SkillMotion.MotionManager.StopSkillEffect(_AvatorEffect);
    }
}
