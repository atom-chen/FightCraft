
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;

namespace GameUI
{
    public class UISkillLevelClassItem : UIItemSelect
    {
        public Text _SkillClassName;

        public override void Show(Hashtable hash)
        {
            base.Show();

            var skillClass = (Tables.SKILL_CLASS)hash["InitObj"];
            _SkillClassName.text = skillClass.ToString();
        }

    }
}
