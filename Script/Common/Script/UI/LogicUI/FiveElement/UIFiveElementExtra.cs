using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElementExtra : UIBase
{

    #region static funs

    public static void ShowAsyn(ItemFiveElement extraItem)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ExtraItem", extraItem);
        GameCore.Instance.UIManager.ShowUI("LogicUI/FiveElement/UIFiveElementExtra", UILayer.SubPopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>("LogicUI/FiveElement/UIFiveElementExtra");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public Image _Icon;
    public Image _Quality;
    public Text _Name;
    public List<Text> _Attrs;

    public UIContainerSelect _ElementItems;

    private ItemFiveElement _ElementItem;
    private ItemFiveElement _SelectedMatItem;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var elementItem = hash["ExtraItem"] as ItemFiveElement;
        ShowItemInfo(elementItem);
        ShowMaterialItems();
        _NeedRefresh = false;
    }

    public override void Hide()
    {
        base.Hide();

        UIFiveElement.RefreshPack();
    }

    private void RefreshInfo()
    {
        ShowItemInfo(_ElementItem);
        ShowMaterialItems();
    }

    private void ShowItemInfo(ItemFiveElement extraItem)
    {
        _ElementItem = extraItem;
        if (_ElementItem == null)
        {
            _ElementItem = FiveElementData.Instance._UsingElements[0];
        }

        _Icon.gameObject.SetActive(true);
        _Quality.gameObject.SetActive(true);
        _Name.text = _ElementItem.GetElementNameWithColor();

        for (int i = 0; i < _Attrs.Count; ++i)
        {
            if (_ElementItem.EquipExAttrs.Count > i)
            {
                _Attrs[i].text = _ElementItem.EquipExAttrs[i].GetAttrStr(false);
            }
            else
            {
                var addRate = FiveElementData.Instance.GetAddExAttrRate(i);
                _Attrs[i].text = GameDataValue.ConfigFloatToPersent(addRate).ToString() + "%";
            }
        }
    }

    private void ShowMaterialItems()
    {
        List<ItemFiveElement> matItems = new List<ItemFiveElement>();
        foreach (var itemElement in FiveElementData.Instance._PackElements)
        {
            if (itemElement == null || !itemElement.IsVolid())
            {
                continue;
            }

            if (itemElement.FiveElementRecord.EvelemtType == _ElementItem.FiveElementRecord.EvelemtType)
            {
                matItems.Add(itemElement);
            }
        }

        _ElementItems.InitSelectContent(matItems, null, OnMatItemClick);
    }

    private void OnMatItemClick(object itemObj)
    {
        ItemFiveElement itemElement = itemObj as ItemFiveElement;

        _SelectedMatItem = itemElement;
    }

    #endregion

    #region opt

    private bool _NeedRefresh = false;

    public void OnBtnExtraOK()
    {
        if (_ElementItem == null)
            return;

        if (_SelectedMatItem == null)
        {
            UIMessageTip.ShowMessageTip(1300001);
            return;
        }

        _NeedRefresh = FiveElementData.Instance.Extract(_SelectedMatItem);
        if (_NeedRefresh)
        {
            RefreshInfo();
        }
    }

    #endregion

}

