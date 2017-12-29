using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class RoleAttrImpactBaseAttr : RoleAttrImpactBase
{

    public override void InitImpact(string skillInput, List<int> args)
    {
        
    }

    public override void ModifySkillBeforeInit(MotionManager roleMotion)
    {
        if (!roleMotion._StateSkill._SkillMotions.ContainsKey(_SkillInput))
            return;

        var skillMotion = roleMotion._StateSkill._SkillMotions[_SkillInput];
        var impactGO = ResourceManager.Instance.GetInstanceGameObject("Bullet\\Emitter\\Element\\" + _ImpactName);
        impactGO.transform.SetParent(skillMotion.transform);

        var bulletEmitterEle = impactGO.GetComponent<BulletEmitterElement>();
        bulletEmitterEle._Damage = _Damage;
    }

    public override bool AddData(List<int> attrParam)
    {
        return true;
    }

    public static string GetAttrDesc(List<int> attrParams)
    {
        Debug.Log("attrParams:" + attrParams[0]);
        var attrTab = Tables.TableReader.FightAttr.GetRecord(attrParams[0].ToString());

        return string.Format(attrTab.ShowTip, attrParams[1]);
    }

    public static EquipExAttr GetExAttrByValue(AttrValueRecord attrRecord, int arg)
    {
        int attrValue = attrRecord.AttrParams[1] + attrRecord.AttrParams[2] * arg;

        EquipExAttr exAttr = new EquipExAttr(attrRecord.AttrImpact, 0, attrRecord.AttrParams[0], attrValue);

        return exAttr;
    }

    #region 

    public int _Damage;
    public string _ImpactName;

    
    #endregion
}
