using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIStageDiffTipsItem : UIItemBase
{

    #region 

    public Text _Text;
    public GameObject _SelectedGO;
    public GameObject _LockGO;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var tipsID = (int)hash["InitObj"];
        int curDiff = ActData.Instance.GetNormalDiff();
        if (tipsID == curDiff)
        {
            _SelectedGO.SetActive(true);
        }
        else
        {
            _SelectedGO.SetActive(false);
            if (tipsID > curDiff)
            {
                _LockGO.SetActive(true);
            }
            else
            {
                _LockGO.SetActive(false);
            }
        }
        _Text.text = Tables.StrDictionary.GetFormatStr(tipsID + 2410000);
    }

    #endregion
}

