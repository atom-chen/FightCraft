
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;



public class UISkillLevelItem : UIItemSelect
{


    public Text _SkillNameText;
    public Text _SkillLevelText;

    public ItemSkill _SkillItem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        _SkillItem = (ItemSkill)hash["InitObj"];
        if (_SkillItem == null)
            return;

        var skillRecord = Tables.TableReader.SkillInfo.GetRecord(_SkillItem.SkillID);
        string skillName = Tables.StrDictionary.GetFormatStr(skillRecord.NameStrDict);
        _SkillNameText.text = skillName;
        _SkillLevelText.text = "Lv." + _SkillItem.SkillActureLevel + "/" + _SkillItem.SkillRecord.MaxLevel;
    }

    public override void Refresh()
    {
        base.Refresh();

        _SkillLevelText.text = "Lv." + _SkillItem.SkillActureLevel + "/" + _SkillItem.SkillRecord.MaxLevel;
        //ShowEquip(_ShowItem as ItemEquip);
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    #endregion
}

