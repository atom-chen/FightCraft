
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;



public class UISkillLevelClassItem : UIItemSelect
{
    public Text _SkillClassName;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var skillClass = (string)hash["InitObj"];
        _SkillClassName.text = skillClass.ToString();
    }

}

