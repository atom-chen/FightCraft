using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICommonAwardItem : UIItemBase
{

    #region 

    public Image _CurrencyIcon;
    public Text _CurrencyValue;
    public Sprite[] _CurrencySprite;

    #endregion

    #region 

    public void SetValue(int value)
    {
        _CurrencyValue.text = value.ToString();
    }

    public void ShowAward(MONEYTYPE currencyType, int currencyValue)
    {
        _CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];

        _CurrencyValue.text = currencyValue.ToString();
    }

    public void ShowAward(MONEYTYPE currencyType, long currencyValue)
    {
        _CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];

        _CurrencyValue.text = currencyValue.ToString();
    }

    public void ShowAward(string itemID, long currencyValue)
    {
        //_CurrencyIcon.sprite = _CurrencySprite[(int)currencyType];

        _CurrencyValue.text = currencyValue.ToString();
    }
    
    #endregion

}

