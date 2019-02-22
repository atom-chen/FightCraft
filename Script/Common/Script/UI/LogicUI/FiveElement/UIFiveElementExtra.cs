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
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElementExtra>("LogicUI/FiveElement/UIFiveElementExtra");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshInfo();
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

        InitAttrFilter();
    }

    public override void Hide()
    {
        base.Hide();

        UIFiveElement.RefreshPack();
    }

    public void RefreshInfo()
    {
        ShowItemInfo(_ElementItem);
        ShowMaterialItems();

        RefreshFilter();
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
        foreach (var itemElement in FiveElementData.Instance._PackElements._PackItems)
        {
            if (itemElement == null || !itemElement.IsVolid())
            {
                continue;
            }

            if (itemElement.FiveElementRecord.EvelemtType != _ElementItem.FiveElementRecord.EvelemtType)
            {
                continue;
            }

            if (!IsFilterAttr(itemElement.EquipExAttrs[0].AttrParams[0]))
            {
                continue;
            }

            matItems.Add(itemElement);
        }
        matItems.Sort((element1, element2) =>
        {
            if (element1.EquipExAttrs[0].AttrParams[0] > element2.EquipExAttrs[0].AttrParams[0])
            {
                return 1;
            }
            else if (element1.EquipExAttrs[0].AttrParams[0] < element2.EquipExAttrs[0].AttrParams[0])
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });

        if (matItems.Count > 0)
        {
            _ElementItems.InitSelectContent(matItems, new List<ItemFiveElement>() { matItems[0] }, OnMatItemClick);
        }
        else
        {
            _ElementItems.InitSelectContent(matItems, null, OnMatItemClick);
        }
    }

    private void OnMatItemClick(object itemObj)
    {
        ItemFiveElement itemElement = itemObj as ItemFiveElement;

        _SelectedMatItem = itemElement;
    }

    #endregion

    #region filter

    public GameObject _FilterPanel;
    public UIContainerBase _FilterContainer;
    public Toggle _FilterUnusedAttr;

    private List<int> _FilterAttrs = null;
    private List<int> _FilterUnusedAttrs = null;

    public void InitAttrFilter()
    {
        _FilterContainer.InitContentItem(GameDataValue._FiveElementAttrs);
        _FilterAttrs = null;
    }

    public void OnBtnShowFilter()
    {
        _FilterPanel.gameObject.SetActive(true);
    }

    public void OnBtnHideFilter()
    {
        _FilterPanel.gameObject.SetActive(false);

        if (_FilterAttrs == null)
        {
            _FilterAttrs = new List<int>();
        }
        _FilterAttrs.Clear();

        _FilterContainer.ForeachActiveItem<UIFiveElementExtraFilterItem>((filterItem) =>
        {
            if (filterItem._Toggle.isOn)
            {
                _FilterAttrs.Add(filterItem.AttrID);
            }
        });

        ShowMaterialItems();

        _FilterUnusedAttr.isOn = false;
    }

    public void OnToggleFilter(bool isOn)
    {
        if (isOn)
        {
            RefreshFilter();
        }
    }

    private void RefreshFilter()
    {
        if (_FilterUnusedAttr.isOn)
        {
            FilterUnuseAttr();
        }
    }

    private void FilterUnuseAttr()
    {
        if (_FilterUnusedAttrs == null)
        {
            _FilterUnusedAttrs = new List<int>();
        }
        _FilterUnusedAttrs.Clear();

        foreach (var elementAttr in GameDataValue._FiveElementAttrs)
        {
            bool containsAttr = false;
            foreach (var exAttr in _ElementItem.EquipExAttrs)
            {
                if (exAttr.AttrParams[0] == (int)elementAttr.AttrID)
                {
                    containsAttr = true;
                    break;
                }
            }

            if (!containsAttr)
            {
                _FilterUnusedAttrs.Add((int)elementAttr.AttrID);
            }
        }

        ShowMaterialItems();
    }

    private bool IsFilterAttr(int attrID)
    {
        bool isFilter = true;
        if (_FilterUnusedAttr.isOn)
        {
            isFilter &= _FilterUnusedAttrs.Contains(attrID);
        }

        if (_FilterAttrs != null)
        {
            isFilter &= _FilterAttrs.Contains(attrID);
        }

        return isFilter;
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

    public void ShowCoreTooltips()
    {
        var elementCore = FiveElementData.Instance._UsingCores[(int)_ElementItem.FiveElementRecord.EvelemtType];
        if (elementCore == null)
            return;

        UIFiveElementCoreTooltip.ShowAsynInType(elementCore, TooltipType.Single);
    }

    public void SwitchElement(int direct)
    {
        int switchPos = (int)_ElementItem.FiveElementRecord.EvelemtType + direct;
        if (switchPos < 0)
        {
            switchPos = 4;
        }
        if (switchPos > 4)
        {
            switchPos = 0;
        }

        var itemUsing = FiveElementData.Instance._UsingElements[switchPos];
        ShowItemInfo(itemUsing);
    }

    #endregion

}

