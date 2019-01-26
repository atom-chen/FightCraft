
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemItem : /*UIDragableItemBase*/ UIPackItemBase
{
    public GameObject _UsingGO;

    private ItemGem _ItemGem;
    public ItemGem ItemGem
    {
        get
        {
            return _ItemGem;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var showItem = (ItemGem)hash["InitObj"];
        ShowGem(showItem);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_ShowedItem as ItemGem);
    }

    public void ShowGem(ItemGem showItem)
    {
        _ItemGem = showItem;

        if (showItem == null)
        {
            ClearItem();
            return;
        }

        _ShowedItem = showItem;
        
        if (!showItem.IsVolid())
        {
            ClearItem();
            return;
        }

        if (_Num != null)
        {
            {
                SetTempNum(showItem.ItemStackNum);
            }
        }

        //if (_DisableGO != null)
        //{
        //    if (showItem.Level > 0)
        //    {
        //        _DisableGO.SetActive(false);
        //    }
        //    else
        //    {
        //        _DisableGO.SetActive(true);
        //    }
        //}

        //if (_UsingGO != null)
        //{
        //    if (GemData.Instance.IsEquipedGem(showItem.ItemDataID))
        //    {
        //        _UsingGO.SetActive(true);
        //    }
        //    else
        //    {
        //        _UsingGO.SetActive(false);
        //    }
        //}
        _Icon.gameObject.SetActive(true);
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

    private int _TempNum = 0;
    public int TempNum
    {
        get
        {
            return _TempNum;
        }
    }

    public void SetTempNum(int num)
    {
        _TempNum = num;
        if (_Num != null)
        {
            if (_TempNum < 0)
            {
                _Num.text = "";
            }
            //else if (_TempNum == 0)
            //{
            //    _Num.text = "";
            //}
            else
            {
                if (_TempNum == _ShowedItem.ItemStackNum)
                {
                    _Num.text = CommonDefine.GetEnableGrayStr(1) + num.ToString() + "</color>";
                }
                else
                {
                    _Num.text = CommonDefine.GetEnableRedStr(0) + num.ToString() + "</color>";
                }
            }

        }
    }

    #endregion
}

