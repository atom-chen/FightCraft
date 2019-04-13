using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElement : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/FiveElement/UIFiveElement", UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>("LogicUI/FiveElement/UIFiveElement");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshUsingCoreItems();

    }

    #endregion

    #region element pack

    public List<UIFiveElementItem> _UsingElement = new List<UIFiveElementItem>();
    public UIFiveElementExtra _UIFiveElementExtra;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshUsingCoreItems();
        OnElementCoreClick(_UsingElement[0]);
    }

    public void RefreshUsingCoreItems()
    {
        for (int i = 0; i < _UsingElement.Count; ++i)
        {
            _UsingElement[i].ShowItem(FiveElementData.Instance._UsingCores[i]);
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", FiveElementData.Instance._UsingCores[i]);
            hash.Add("DragPack", this);
            _UsingElement[i].Show(hash);
            _UsingElement[i]._InitInfo = FiveElementData.Instance._UsingCores[i];
            //_UsingElement[i]._ClickEvent += ShowTooltipsLeft;
            _UsingElement[i]._PanelClickEvent += OnElementCoreClick;
        }
    }
    
    private void OnElementCoreClick(UIItemBase elementItem)
    {
        for (int i = 0; i < _UsingElement.Count; ++i)
        {
            if (_UsingElement[i] == elementItem)
            {
                if (_UIFiveElementExtra.isActiveAndEnabled)
                {
                    _UIFiveElementExtra.ShowItemByIndex(i);
                }
                _UsingElement[i].Selected();
            }
            else
            {
                _UsingElement[i].UnSelected();
            }
        }
    }
    
    
    #endregion
    

}

