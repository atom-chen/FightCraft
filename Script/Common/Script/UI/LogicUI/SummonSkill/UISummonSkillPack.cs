using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonSkillPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/SummonSkill/UISummonSkillPack", UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillPack>("LogicUI/SummonSkill/UISummonSkillPack");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region pack

    public UIContainerBase _SummonItemContainer;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowItemPack();
        ShowUsingItems();

        _ArrayMode = false;
    }

    public void RefreshItems()
    {
        ShowItemPack();
        ShowUsingItems();
    }

    private void ShowItemPack()
    {
        List<SummonMotionData> unusedMotions = new List<SummonMotionData>();
        for (int i = 0; i < SummonSkillData.Instance._SummonMotionList.Count; ++i)
        {
            //var sameSummonData = SummonSkillData.Instance._UsingSummon.Find((summonData) =>
            //{
            //    if (summonData.SummonRecordID == SummonSkillData.Instance._SummonMotionList[i].SummonRecordID)
            //    {
            //        return true;
            //    }
            //    return false;
            //});
            //if (sameSummonData == null)
            {
                unusedMotions.Add(SummonSkillData.Instance._SummonMotionList[i]);
            }
        }

        SummonSkillData.Instance.SortSummonMotionsInPack(unusedMotions);

        _SummonItemContainer.InitContentItem(unusedMotions, null, null, OnPackClick);
    }

    private void OnPackClick(UIItemBase itemObj)
    {
        UISummonSkillItem summonItem = itemObj as UISummonSkillItem;

        if (_ArrayMode)
        {
            var sameSummonData = SummonSkillData.Instance._UsingSummon.Find((summonData) =>
            {
                if (summonData!= null && summonData.SummonRecordID == summonItem.SummonMotionData.SummonRecordID)
                {
                    return true;
                }
                return false;
            });

            if (sameSummonData != null)
            {
                UIMessageTip.ShowMessageTip(20007);
                return;
            }

            if (summonItem.SummonMotionData.SummonRecord.Quality == Tables.ITEM_QUALITY.WHITE)
                return;

            SetSelectItem(summonItem.SummonMotionData);

            SelectNextEmpty();
        }
        else
        {
            UISummonSkillToolTips.ShowAsyn(summonItem.SummonMotionData);
        }
    }

    #endregion

    #region using

    public List<UISummonSkillItem> _UsingItems;

    private UISummonSkillItem _SelectingItem;

    private void ShowUsingItems()
    {
        for (int i = 0; i < SummonSkillData.Instance._UsingSummon.Count; ++i)
        {
            _UsingItems[i].ShowSummonData(SummonSkillData.Instance._UsingSummon[i]);
            _UsingItems[i]._PanelClickEvent = OnArrayClick;
        }

        if (!_ArrayMode)
        {
            ClearSelect();
        }
    }

    private void OnArrayClick(UIItemBase itemObj)
    {
        UISummonSkillItem summonItem = itemObj as UISummonSkillItem;
        if (summonItem == null)
            return;

        if (_ArrayMode)
        {
            if (_SelectingItem == summonItem)
            {
                //_SelectingItem.ShowSummonData(null);
                SetSelectItem(null);
            }
            SetSelectedArray(summonItem);
        }
        else
        {
            if (summonItem.SummonMotionData == null)
                return;

            UISummonSkillToolTips.ShowAsyn(summonItem.SummonMotionData);
        }
    }

    private void SetSelectedArray(UISummonSkillItem summonItem)
    {
        for (int i = 0; i < _UsingItems.Count; ++i)
        {
            if (_UsingItems[i] == summonItem)
            {
                _UsingItems[i].SetArraySelected(true);
            }
            else
            {
                _UsingItems[i].SetArraySelected(false);
            }
        }

        _SelectingItem = summonItem;
    }

    private void SetSelectItem(SummonMotionData summonData)
    {
        if (_SelectingItem == null)
            return;

        var idx = _UsingItems.IndexOf(_SelectingItem);
        SummonSkillData.Instance.SetUsingSummon(idx, summonData);
        RefreshItems();
    }

    private void SelectNextEmpty()
    {
        for (int i = 0; i < _UsingItems.Count; ++i)
        {
            if (_UsingItems[i].SummonMotionData == null)
            {
                SetSelectedArray(_UsingItems[i]);
                return;
            }
        }
    }

    private void ClearSelect()
    {
        for (int i = 0; i < _UsingItems.Count; ++i)
        {
            _UsingItems[i].SetArraySelected(false);
        }

        _SelectingItem = null;
    }

    #endregion

    #region interface

    public GameObject _BtnArray;
    public GameObject _BtnArrayOk;

    private bool _ArrayMode = false;

    public void OnBtnLotteryGold()
    {
        UISummonSkillLottery.ShowGoldAsyn();
    }

    public void OnBtnLotteryDiamond()
    {
        UISummonSkillLottery.ShowDiamondAsyn();
    }

    public void OnBtnArray()
    {
        _ArrayMode = true;

        _BtnArray.SetActive(false);
        _BtnArrayOk.SetActive(true);

        SelectNextEmpty();
        if (_SelectingItem == null)
        {
            SetSelectedArray(_UsingItems[0]);
        }
    }

    public void OnBtnArrayOK()
    {
        _ArrayMode = false;

        _BtnArray.SetActive(true);
        _BtnArrayOk.SetActive(false);

        SetSelectedArray(null);
    }

    public void OnBtnCollect()
    {
        UISummonCollections.ShowAsyn();
    }
    #endregion

}

