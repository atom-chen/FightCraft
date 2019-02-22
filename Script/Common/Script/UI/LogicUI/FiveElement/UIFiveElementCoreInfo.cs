
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIFiveElementCoreInfo : UIItemInfo
{

    #region 

    public Text _Value;
    public UIContainerBase _ElementCoreContainer;

    #endregion

    #region 

    private ItemFiveElementCore _ShowElementItem;

    public void ShowTips(ItemFiveElementCore itemElement)
    {
        if (itemElement == null || !itemElement.IsVolid())
        {
            _ShowElementItem = null;
            return;
        }

        _ShowElementItem = itemElement;


        _Name.text = _ShowElementItem.GetElementNameWithColor();
        _Level.text = _ShowElementItem.Level.ToString();


        bool isConditionComplate = true;
        List<string> conditionDesc = new List<string>();
        for (int i = 0; i < _ShowElementItem.FiveElementRecord.PosCondition.Count; ++i)
        {
            var conState = _ShowElementItem.ConditionState(i);
            if (conState < 0)
                break;

            if (conState == 0)
            {
                isConditionComplate = false;
            }

            var conDesc = _ShowElementItem.GetConditionDesc(i);
            if (conState > 0 && isConditionComplate)
            {
                conDesc = CommonDefine.GetEnableRedStr(1) + conDesc + "</color>";
            }
            else
            {
                conDesc = CommonDefine.GetEnableGrayStr(0) + conDesc + "</color>";
            }
            conditionDesc.Add(conDesc);
        }
        
        Hashtable hash = new Hashtable();
        _ElementCoreContainer.InitContentItem(conditionDesc, null, hash);
    }
    #endregion



}

