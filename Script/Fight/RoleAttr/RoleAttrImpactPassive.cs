using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactPassive : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ImpactName = attrTab.StrParam[0];
        _SkillInput = attrTab.StrParam[1];
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
            buff.ActImpact(roleMotion, roleMotion);
        }
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        var attrValue = TableReader.AttrValue.GetRecord(attrParams[0].ToString());
        List<int> copyAttrs = new List<int>(attrParams);
        int legendaryId = copyAttrs[0];
        copyAttrs.RemoveAt(0);
        var strFormat = StrDictionary.GetFormatStr(legendaryId, copyAttrs);
        return strFormat;
    }

    #region 

    public string _ImpactName;
    
    #endregion
}
