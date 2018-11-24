using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum MONEYTYPE
{
    GOLD = 0,
    DIAMOND,
    ITEM,
}

public class UICurrencyItem : UIItemBase
{

    #region 

    public Image _CurrencyIcon;
    public Text _CurrencyValue;
    public Sprite[] _CurrencySprite;

    private MONEYTYPE _CurrencyType;
    private int _CurrencyIntValue;
    public int CurrencyIntValue
    {
        get
        {
            return _CurrencyIntValue;
        }
    }

    #endregion

    #region 

    public void ShowCurrency(MONEYTYPE currencyType, int currencyValue)
    {
        _CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = currencyValue;
        _CurrencyType = currencyType;
    }

    public void ShowCurrency(MONEYTYPE currencyType, long currencyValue)
    {
        _CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = (int)currencyValue;
        _CurrencyType = currencyType;
    }

    public void ShowCurrency(string itemID, long currencyValue)
    {
        //_CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];

        _CurrencyValue.text = currencyValue.ToString();
        _CurrencyIntValue = (int)currencyValue;
        _CurrencyType = MONEYTYPE.ITEM;
    }

    public void ShowOwnCurrency(MONEYTYPE currencyType)
    {
        int Ownvalue = 0;
        if (currencyType == MONEYTYPE.GOLD)
        {
            Ownvalue = PlayerDataPack.Instance.Gold;
        }
        else if (currencyType == MONEYTYPE.DIAMOND)
        {
            Ownvalue = PlayerDataPack.Instance.Diamond;
        }
        ShowCurrency(currencyType, Ownvalue);
    }

    public void ShowOwnCurrency(string itemDataID)
    {
        int Ownvalue = BackBagPack.Instance.PageItems.GetItemCnt(itemDataID);
        ShowCurrency(itemDataID, Ownvalue);
    }

    #endregion

    public void OnBtnAddClick()
    {

        PlayerDataPack.Instance.AddGold(50000);
        PlayerDataPack.Instance.AddDiamond(1000);
        UIMainFun.UpdateMoney();

    }
}

