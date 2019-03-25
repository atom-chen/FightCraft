
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIGemSuitAttrItem : UIItemBase
{
    public Text _AttrText;

    private GemSetRecord _GemSetRecord;
    private EquipExAttr _ShowAttr;
    private int _Idx;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipExAttr)hash["InitObj"];
        _GemSetRecord = (GemSetRecord)hash["GetSetRecord"];
        _Idx = (int)hash["InitIdx"];

        ShowAttr(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowAttr(_ShowAttr);
    }

    public void ShowAttr(EquipExAttr attr)
    {

        _ShowAttr = attr;

        string attrStr = _ShowAttr.GetAttrStr();

        //int actLevel = GemSuit._ActAttrLevel[_Idx];

        //if (_GemSetRecord == GemSuit.Instance.ActSet && _Idx < GemSuit.Instance.ActSetAttrCnt)
        //{
        //    attrStr = CommonDefine.GetEnableGrayStr(1) + attrStr + "</color>";
        //    attrStr += CommonDefine.GetEnableRedStr(1) + StrDictionary.GetFormatStr(30005, actLevel) + "</color>";
        //}
        //else
        //{
        //    attrStr = CommonDefine.GetEnableGrayStr(0) + attrStr + "</color>";
        //    attrStr += CommonDefine.GetEnableRedStr(0) + StrDictionary.GetFormatStr(30005, actLevel) + "</color>";
        //}

        _AttrText.text = attrStr;
    }


}

