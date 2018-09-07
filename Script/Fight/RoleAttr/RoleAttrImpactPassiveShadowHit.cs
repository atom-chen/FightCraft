using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassiveShadowHit : RoleAttrImpactPassive
{
    public override void InitImpact(string skillInput, List<int> args)
    {
        //base.InitImpact(skillInput, args);

        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ShadowHitCnt = GetValueFromTab(attrTab, args[1]);
        _HitDamage = GetValue2FromTab(attrTab, args[1]);

        //_ShadowHitCnt = args[0];
        //_HitDamage = GameDataValue.ConfigIntToFloat(args[1]);
       
    }

    public override void ModifySkillAfterInit(MotionManager roleMotion)
    {
        //if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
        //    return;

        var buffGO = ResourceManager.Instance.GetInstanceGameObject("SkillMotion/CommonImpact/ShadowHit");
        buffGO.transform.SetParent(roleMotion.BuffBindPos.transform);
        var buffs = buffGO.GetComponents<ImpactBuff>(); 
        foreach (var buff in buffs)
        {
            var subBuffs = buffGO.GetComponentsInChildren<ImpactBuffShadowHit>();
            foreach (var subBuff in subBuffs)
            {
                if (subBuff.gameObject == buffGO)
                    continue;
                subBuff._ShadowCnt = _ShadowHitCnt;
                subBuff._DamageRate = _HitDamage;
            }

            buff.ActImpact(roleMotion, roleMotion);
        }

    }

    public new static string GetAttrDesc(List<int> attrParams)
    {
        //List<int> copyAttrs = new List<int>(attrParams);
        //int attrDescID = copyAttrs[0];
        //var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        //var value1 = GetValueFromTab(attrTab, attrParams[1]);
        //var value2 = GetValue2FromTab(attrTab, attrParams[1]);
        //var strFormat = StrDictionary.GetFormatStr(attrDescID, GameDataValue.ConfigFloatToPersent(value1), value2);
        //return strFormat;
        return "";
    }

    #region 

    private int _ShadowHitCnt;
    private float _HitDamage;

    private static int GetValueFromTab(AttrValueRecord attrRecord, int level)
    {
        int theValue = (level) / 2 + 1;
        return theValue;
    }

    private static float GetValue2FromTab(AttrValueRecord attrRecord, int level)
    {
        int stepLv = (level) / 3 + 1;
        var theValue = GameDataValue.ConfigIntToFloat(attrRecord.AttrParams[0] + attrRecord.AttrParams[1] * stepLv);
        return theValue;
    }

    #endregion
}
