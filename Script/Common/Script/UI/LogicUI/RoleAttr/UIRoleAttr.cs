using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleAttr : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/RoleAttr/UIRoleAttr", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitRoleAttrs();
    }

    #endregion

    public RoleAttrItem _RoleLevel;
    public RoleAttrItem _AttrLevel;
    public RoleAttrItem _UnDistrubutePoint;
    public RoleAttrItem _StrengthItem;
    public RoleAttrItem _DexterityItem;
    public RoleAttrItem _IntelligenceItem;
    public RoleAttrItem _VitalityItem;
    public GameObject _BtnAddStrength;
    public GameObject _BtnDexterity;
    public GameObject _BtnIntelligence;
    public GameObject _BtnVitality;
    public GameObject _BtnAddStrength10;
    public GameObject _BtnDexterity10;
    public GameObject _BtnIntelligence10;
    public GameObject _BtnVitality10;

    public UIContainerBase _AttrItemContainer;

    private void InitRoleAttrs()
    {
        _RoleLevel.Show("RoleLevel", RoleData.SelectRole._RoleLevel);
        _AttrLevel.Show("AttrLevel", RoleData.SelectRole._AttrLevel);
        _UnDistrubutePoint.Show("UnDistributePoint", RoleData.SelectRole.UnDistrubutePoint);
        _StrengthItem.Show(RoleAttrEnum.Strength.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Strength));
        _DexterityItem.Show(RoleAttrEnum.Dexterity.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Dexterity));
        _IntelligenceItem.Show(RoleAttrEnum.Intelligence.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Intelligence));
        _VitalityItem.Show(RoleAttrEnum.Vitality.ToString(), RoleData.SelectRole._BaseAttr.GetValue(RoleAttrEnum.Vitality));

        if (RoleData.SelectRole.UnDistrubutePoint > 10)
        {
            _BtnAddStrength10.gameObject.SetActive(true);
            _BtnDexterity10.gameObject.SetActive(true);
            _BtnIntelligence10.gameObject.SetActive(true);
            _BtnVitality10.gameObject.SetActive(true);

            _BtnAddStrength.gameObject.SetActive(true);
            _BtnDexterity.gameObject.SetActive(true);
            _BtnIntelligence.gameObject.SetActive(true);
            _BtnVitality.gameObject.SetActive(true);
        }
        else if (RoleData.SelectRole.UnDistrubutePoint > 0)
        {
            _BtnAddStrength10.gameObject.SetActive(false);
            _BtnDexterity10.gameObject.SetActive(false);
            _BtnIntelligence10.gameObject.SetActive(false);
            _BtnVitality10.gameObject.SetActive(false);

            _BtnAddStrength.gameObject.SetActive(true);
            _BtnDexterity.gameObject.SetActive(true);
            _BtnIntelligence.gameObject.SetActive(true);
            _BtnVitality.gameObject.SetActive(true);
        }
        else
        {
            _BtnAddStrength10.gameObject.SetActive(false);
            _BtnDexterity10.gameObject.SetActive(false);
            _BtnIntelligence10.gameObject.SetActive(false);
            _BtnVitality10.gameObject.SetActive(false);

            _BtnAddStrength.gameObject.SetActive(false);
            _BtnDexterity.gameObject.SetActive(false);
            _BtnIntelligence.gameObject.SetActive(false);
            _BtnVitality.gameObject.SetActive(false);
        }

        List<AttrPair> pair = new List<AttrPair>();

        pair.Add(new AttrPair(RoleAttrEnum.Attack));
        pair.Add(new AttrPair(RoleAttrEnum.Defense));
        pair.Add(new AttrPair(RoleAttrEnum.HPMax));
        pair.Add(new AttrPair(RoleAttrEnum.AttackSpeed));
        pair.Add(new AttrPair(RoleAttrEnum.CriticalHitChance));
        pair.Add(new AttrPair(RoleAttrEnum.CriticalHitDamge));
        pair.Add(new AttrPair(RoleAttrEnum.FireAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.ColdAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.LightingAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.WindAttackAdd));
        pair.Add(new AttrPair(RoleAttrEnum.FireResistan));
        pair.Add(new AttrPair(RoleAttrEnum.ColdResistan));
        pair.Add(new AttrPair(RoleAttrEnum.LightingResistan));
        pair.Add(new AttrPair(RoleAttrEnum.WindResistan));
        pair.Add(new AttrPair(RoleAttrEnum.FireEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.ColdEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.LightingEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.WindEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.PhysicDamageEnhance));
        pair.Add(new AttrPair(RoleAttrEnum.IgnoreDefenceAttack));
        pair.Add(new AttrPair(RoleAttrEnum.FinalDamageReduse));

        _AttrItemContainer.InitContentItem(pair);
    }

    public void OnDistrubuteStr(bool isPress)
    {
        if (!isPress)
            return;

        RoleData.SelectRole.DistributePoint(1, 1);
        InitRoleAttrs();
    }

    public void OnDistrubuteDex(bool isPress)
    {
        if (!isPress)
            return;
        RoleData.SelectRole.DistributePoint(2, 1);
        InitRoleAttrs();
    }

    public void OnDistrubuteInt(bool isPress)
    {
        if (!isPress)
            return;

        RoleData.SelectRole.DistributePoint(3, 1);
        InitRoleAttrs();
    }

    public void OnDistrubuteVit(bool isPress)
    {
        if (!isPress)
            return;

        RoleData.SelectRole.DistributePoint(4, 1);
        InitRoleAttrs();
    }

    public void OnDistrubuteStr10(bool isPress)
    {
        if (!isPress)
            return;

        RoleData.SelectRole.DistributePoint(1, 10);
        InitRoleAttrs();
    }

    public void OnDistrubuteDex10(bool isPress)
    {
        if (!isPress)
            return;
        RoleData.SelectRole.DistributePoint(2, 10);
        InitRoleAttrs();
    }

    public void OnDistrubuteInt10(bool isPress)
    {
        if (!isPress)
            return;

        RoleData.SelectRole.DistributePoint(3, 10);
        InitRoleAttrs();
    }

    public void OnDistrubuteVit10(bool isPress)
    {
        if (!isPress)
            return;

        RoleData.SelectRole.DistributePoint(4, 10);
        InitRoleAttrs();
    }

    public void OnResetPoints()
    {
        RoleData.SelectRole.ResetPoints();
        InitRoleAttrs();
    }

    #region show attr tips

    public Vector3[] _ShowPoses;

    public void OnShowBaseAttr(int type)
    {
        RoleAttrEnum attr = (RoleAttrEnum)type;
        string showTips = "";
        int value = RoleData.SelectRole._BaseAttr.GetValue(attr);
        switch (attr)
        {
            case RoleAttrEnum.Strength:
                showTips = Tables.StrDictionary.GetFormatStr(1001000, value * GameDataValue._AttackPerStrength, value * GameDataValue._DmgEnhancePerStrength);
                break;
            case RoleAttrEnum.Dexterity:
                showTips = Tables.StrDictionary.GetFormatStr(1001001, value * GameDataValue._IgnoreAtkPerDex, GameDataValue.ConfigIntToPersent((int)(value * GameDataValue._CriticalRatePerDex)), GameDataValue.ConfigIntToPersent((int)(value * GameDataValue._CriticalDmgPerDex)));
                break;
            case RoleAttrEnum.Intelligence:
                showTips = Tables.StrDictionary.GetFormatStr(1001002, value * GameDataValue._EleAtkPerInt, value * GameDataValue._EleEnhancePerInt);
                break;
            case RoleAttrEnum.Vitality:
                showTips = Tables.StrDictionary.GetFormatStr(1001003, value * GameDataValue._HPPerVit, value * GameDataValue._FinalDmgRedusePerVit);
                break;
        }

        UITextTip.ShowMessageTip(showTips, _ShowPoses[type - 1]);
    }

    #endregion
}

