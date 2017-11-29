using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIBackPack : UIBase
{
    [Serializable]
    public class OnSelectItem : UnityEvent<ItemBase>
    {
        public OnSelectItem() { }
    }

    [SerializeField]
    public OnSelectItem _OnItemSelectCallBack;

    #region 

    public UIContainerSelect _ItemsContainer;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        OnShowPage(0);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnShowPage(int page)
    {
        if (page == 0)
        {
            _ItemsContainer.InitSelectContent(BackBagPack.Instance.PageEquips, null, ShowBackPackTooltips);
        }
        else
        {
            _ItemsContainer.InitSelectContent(BackBagPack.Instance.PageItems, null, ShowBackPackTooltips);
        }

    }

    public void RefreshItems()
    {
        _ItemsContainer.RefreshItems();
    }

    private void ShowBackPackTooltips(object equipObj)
    {
        ItemBase equipItem = equipObj as ItemBase;
        _OnItemSelectCallBack.Invoke(equipItem);
    }


    #endregion

    #region 

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    #endregion

}

