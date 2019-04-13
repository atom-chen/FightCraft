
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;

public class UIGemPackItem : UIItemSelect
{
    public Image _Icon;
    public Image _Quality;
    public Text _Name;
    public Text _Attr;
    public Text _Level;
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

        ShowGem(_ItemGem);
    }

    public void ShowGem(ItemGem showItem)
    {
        _ItemGem = showItem;

        _Icon.gameObject.SetActive(true);
        _Quality.gameObject.SetActive(false);
        _Name.text = showItem.GetName();
        _Attr.text = showItem.GemAttr.GetAttrStr();
        _Level.text = showItem.Level.ToString();

        _UsingGO.SetActive(GemData.Instance.IsEquipedGem(_ItemGem));
    }
    
}

