using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class UIGemSuitAttr : UIBase
{

    #region 

    public Text _GemSuitName;
    public Text _GemSuitLevel;
    public UIContainerBase _AttrContainer;

    #endregion

    public void ClearSuitInfo()
    {
        _GemSuitName.text = "";
        _AttrContainer.InitContentItem(null);
    }

    public void SuitSelect(GemSetRecord gemSet)
    {
        if (gemSet == null)
            return;

        //_GemSuitName.text = GemSuit.Instance.ActLevel.ToString();
        _GemSuitLevel.text = StrDictionary.GetFormatStr(30004, GemSuit.Instance.ActLevel);
        Hashtable hash = new Hashtable();
        hash.Add("GetSetRecord", gemSet);

        List<EquipExAttr> exAttrs = GameDataValue.GetGemSetAttr(gemSet, 0);

        _AttrContainer.InitContentItem(exAttrs, null, hash);
    }
}

