using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleAttrImpactPassiveRelive : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ConcealTime = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0]) - GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] * args[1]);
        if (attrTab.AttrParams[1] > 0)
        {
            _ConcealTime = Mathf.Min(_ConcealTime, attrTab.AttrParams[1]);
        }

        _ResumeHP = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[2]) - GameDataValue.ConfigIntToFloat(attrTab.AttrParams[1] * args[1]);
        _ResumeHP = Mathf.Min(_ResumeHP, 1);
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Passive\\" + _ImpactName);
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuff>(); 
        foreach (var buff in buffs)
        {
            var subBuffs = buffGO.GetComponentsInChildren<ImpactBuffConceal>();
            foreach (var subBuff in subBuffs)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._LastTime = _ConcealTime;
            }

            var subBuffs2 = buffGO.GetComponentsInChildren<ImpactResumeHP>();
            foreach (var subBuff in subBuffs2)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._HPPersent = _ResumeHP;
            }


            buff.ActImpact(roleMotion, roleMotion);
        }

    }


    #region 

    private float _ConcealTime;
    private float _ResumeHP;
    
    #endregion
}
