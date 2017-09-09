
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;



public class UISkillLevelItem : UIItemSelect
{


    public Text _SkillNameText;
    public Text _SkillLevelText;

    public SkillInfoItem _SkillItem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        _SkillItem = (SkillInfoItem)hash["InitObj"];
        if (_SkillItem == null)
            return;

        _SkillNameText.text = Tables.TableReader.SkillInfo.GetRecord(_SkillItem._SkillID).Name;
        _SkillLevelText.text = "Lv." + _SkillItem.SkillActureLevel;
    }

    public override void Refresh()
    {
        base.Refresh();

        _SkillLevelText.text = "Lv." + _SkillItem.SkillActureLevel;
        //ShowEquip(_ShowItem as ItemEquip);
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    #endregion
}

