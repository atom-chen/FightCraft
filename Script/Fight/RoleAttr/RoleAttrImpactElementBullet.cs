using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class RoleAttrImpactElementBullet : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        var attrTab = Tables.TableReader.AttrValue.GetRecord(args[0].ToString());
        _ImpactName = attrTab.StrParam[0];
        _SkillInput = attrTab.StrParam[1];
        _Damage = GameDataValue.ConfigIntToFloat(attrTab.AttrParams[0] + attrTab.AttrParams[1] * args[1]);
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Emitter\\Element\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

        var bulletEmitterEle = impactGO.GetComponentsInChildren<BulletEmitterElement>();
        foreach (var bulletEmitter in bulletEmitterEle)
        {
            bulletEmitter._Damage = _Damage;
        }
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        List<int> copyAttrs = new List<int>(attrParams);
        int attrDescID = copyAttrs[0];
        var attrTab = Tables.TableReader.AttrValue.GetRecord(attrDescID.ToString());
        var damage = attrTab.AttrParams[0] + attrTab.AttrParams[1] * attrParams[1];
        var strFormat = StrDictionary.GetFormatStr(attrDescID, GameDataValue.ConfigIntToPersent(damage));
        return strFormat;
    }

    #region 

    public float _Damage;
    public string _ImpactName;

    
    #endregion
}
