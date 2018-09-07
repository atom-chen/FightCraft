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

        instance.RefreshItems();
    }

    #endregion

    #region 

    public List<Sprite> _ResSprites = new List<Sprite>();
    public Text _ResCost;
    public List<Text> _ElementsLv;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);


    }

    public void RefreshItems()
    {

    }

    #endregion

    #region 

    #endregion

}

