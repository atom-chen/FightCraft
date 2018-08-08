using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHPPanel : UIInstanceBase<UIHPPanel>
{
    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/HPPanel/UIHPPanel", UILayer.BaseUI, hash);
    }

    public static void ShowHPItem(MotionManager motionManager)
    {
        Hashtable hash = new Hashtable();
        hash.Add("InitObj", motionManager);
        Instance.ShowItem(hash);
    }

    public static void HideHPItem(UIHPItem hpItem)
    {
        Hashtable hash = new Hashtable();
        hash.Add("HideItem", hpItem);
        Instance.HideItem(hash);
        
    }

    #endregion

    public UIHPItem _UIHPItemPrefab;

    private void ShowItem(Hashtable args)
    {

        var itemBase = ResourcePool.Instance.GetIdleUIItem<UIHPItem>(_UIHPItemPrefab.gameObject);
        itemBase.Show(args);
        itemBase.transform.SetParent(transform);
        itemBase.transform.localScale = Vector3.one;

    }

    private void HideItem(Hashtable args)
    {
        UIHPItem hideItem = args["HideItem"] as UIHPItem;
        ResourcePool.Instance.RecvIldeUIItem(hideItem.gameObject);

    }


}

