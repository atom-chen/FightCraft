
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;

namespace GameUI
{
    public class AttrPair
    {
        public string AttrName;
        public string AttrValue;

        public AttrPair(string attrName, string attrValue)
        {
            AttrName = attrName;
            AttrValue = attrValue;
        }
    }

    public class RoleAttrItem : UIItemBase
    {

        public Text _AttrName;
        public Text _AttrValue;

        public override void Show(Hashtable hash)
        {
            base.Show();

            var attrPair = (AttrPair)hash["InitObj"];
            if (attrPair == null)
                return;

            _AttrName.text = attrPair.AttrName;
            _AttrValue.text = attrPair.AttrValue;
        }

        public void Show(string attrName, int value)
        {
            _AttrName.text = attrName.ToString();
            _AttrValue.text = value.ToString();
        }
    }
}
