using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIStageDiffTips : UIPopBase
{
    #region static
    
    
    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIStageDiffTips, UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public UIContainerBase _TipsContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        List<int> strTips = new List<int>();
        for (int i = 0; i <= 16; ++i)
        {
            strTips.Add(i);
        }

        _TipsContainer.InitContentItem(strTips);
    }

    #endregion
}

