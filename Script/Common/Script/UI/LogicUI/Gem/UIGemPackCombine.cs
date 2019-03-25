using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIGemPackCombine : UIBase
{

    #region 
    

    #endregion

    #region 

    public void OnEnable()
    {
        ShowPackItems();
        InitCopyPack();
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        for (int i = 0; i < _CombinePack.Count; ++i)
        {
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", null);
            hash.Add("DragPack", this);
            _CombinePack[i].Show(hash);
            _CombinePack[i]._InitInfo = null;
            _CombinePack[i]._PanelClickEvent += ShowGemTooltipsLeft;
        }
        //_BackPack.Show(null);
    }

    public void RefreshItems()
    {
        UIGemPack.RefreshPack();
    }

    private void ShowGemTooltipsLeft(UIItemBase uiItem)
    {
        UIGemItem uiGemItem = uiItem as UIGemItem;
        int idx = _CombinePack.IndexOf(uiGemItem);
        _CopyPack[idx].SetTempNum(_CopyPack[idx].TempNum + 1);
        uiGemItem.ShowGem(null);
    }

    public void ShowGemTooltipsRight(UIGemItem gemItem)
    {
        int emptyPos = -1;
        for (int i = 0; i < _CombinePack.Count; ++i)
        {
            if (_CombinePack[i].ItemGem == null)
            {
                emptyPos = i;
            }
        }

        if (emptyPos < 0)
            return;

        gemItem.SetTempNum(gemItem.TempNum - 1);
        _CombinePack[emptyPos].ShowGem(gemItem.ItemGem);
        _CombinePack[emptyPos].SetTempNum(-1);
        _CopyPack[emptyPos] = gemItem;
    }

    public void AutoFitCombine(Tables.GemTableRecord resultGemRecord)
    {
        var gemPack = UIGemPack.GetGemPack();
        if (gemPack == null)
            return;

        ResetPacket();
        for (int i = 0; i < resultGemRecord.Combine.Count; ++i)
        {
            if (resultGemRecord.Combine[i] > 0)
            {
                string matGemData = resultGemRecord.Combine[i].ToString();
                gemPack.ForeachActiveItem<UIGemItem>((uiGemItem) =>
                {
                    if (uiGemItem.ItemGem.ItemDataID == matGemData)
                    {
                        ShowGemTooltipsRight(uiGemItem);
                        return;
                    }
                });
            }
        }
    }

    #endregion

    #region 

    public List<UIGemItem> _CombinePack;

    private List<UIGemItem> _CopyPack;

    private void InitCopyPack()
    {
        //if (_CopyPack == null)
        {
            _CopyPack = new List<UIGemItem>();
            for (int i = 0; i < _CombinePack.Count; ++i)
            {
                _CopyPack.Add(null);
            }
        }
    }

    private void ResetPacket()
    {
        RefreshItems();
        InitCopyPack();

        for (int i = 0; i < _CombinePack.Count; ++i)
        {
            _CombinePack[i].ShowGem(null);
        }
    }

    public void OnBtnCombine()
    {
        List<ItemGem> combines = new List<ItemGem>();
        for (int i = 0; i < _CopyPack.Count; ++i)
        {
            if (_CopyPack[i] != null)
            {
                combines.Add(_CopyPack[i].ItemGem);
            }
        }

        if (GemData.Instance.GemCombine(combines))
        {
            ResetPacket();
        }
    }

    public void OnBtnShowFormulas()
    {
        UIGemCombineSet.ShowAsyn();
    }

    public void OnBtnCombineAll()
    {
        if (_CopyPack[0].ItemGem != null && _CopyPack[0].ItemGem.IsVolid()
            && _CopyPack[0].ItemGem.ItemDataID == _CopyPack[1].ItemGem.ItemDataID
            && _CopyPack[0].ItemGem.ItemDataID == _CopyPack[2].ItemGem.ItemDataID
            )
        {
            string dictStr = Tables.StrDictionary.GetFormatStr(30007, Tables.StrDictionary.GetFormatStr(_CopyPack[0].ItemGem.CommonItemRecord.NameStrDict));
            UIMessageBox.Show(dictStr, () =>
            {
                GemData.Instance.GemCombineSameAll(_CopyPack[0].ItemGem);
                ResetPacket();
            }, null);
            
        }
        else
        {
            UIMessageTip.ShowMessageTip(30008);
        }
    }
    #endregion

}

