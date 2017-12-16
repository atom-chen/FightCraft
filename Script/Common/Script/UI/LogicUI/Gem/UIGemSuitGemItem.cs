
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemSuitGemItem : UIPackItemBase
{
    public GameObject _UnClearItem; 

    private string _GemDataID;
    private int _MinGemLv;
    private bool _ClearGem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemInfo = (Tables.GemTableRecord)hash["InitObj"];
        _MinGemLv = (int)hash["MinLevel"];
        _ClearGem = (bool)hash["IsClearGem"];

        ShowGem(gemInfo.Id);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_GemDataID);
    }


    public void ShowGem(string gemDataID)
    {

        var gemData = GemData.Instance.GetGemInfo(gemDataID);
        if (gemData == null || !gemData.IsVolid())
        {
            ClearItem();
            return;
        }

        _GemDataID = gemDataID;

        if (_Num != null)
        {
            {
                _Num.text = gemData.Level.ToString();
            }
        }
        _Icon.gameObject.SetActive(true);

        if (_DisableGO != null)
        {
            if (gemData.Level >= _MinGemLv)
            {
                _DisableGO.SetActive(false);
            }
            else
            {
                _DisableGO.SetActive(true);
            }
        }
        
        if (!_ClearGem)
        {
            if (gemData.Level >= _MinGemLv)
            {
                _UnClearItem.SetActive(false);
            }
            else
            {
                _UnClearItem.SetActive(true);
            }
        }
    }

    protected override void ClearItem()
    {
        base.ClearItem();

        if (_DisableGO != null)
        {

            _DisableGO.SetActive(false);
        }
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    #endregion
}

