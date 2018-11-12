using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class UISummonSkillItem : UIItemSelect
{
    public GameObject _InfoPanel;
    public Text _Name;
    public Image _Quality;
    public Image _Icon;
    public Text _Level;
    public GameObject[] _Stars;
    public GameObject _ArraySelect;
    public GameObject _ExpSelect;

    private SummonMotionData _SummonMotionData;
    public SummonMotionData SummonMotionData
    {
        get
        {
            return _SummonMotionData;
        }
    }

    public override void Show(Hashtable hash)
    {
        base.Show();

        var summonData = (SummonMotionData)hash["InitObj"];
        if (summonData == null)
            return;

        ShowSummonData(summonData);

        _ExpSelect.gameObject.SetActive(false);
        if (hash.Contains("ShowSelect"))
        {
            bool isShowSelect = (bool)hash["ShowSelect"];
            if (isShowSelect)
            {
                _ExpSelect.gameObject.SetActive(true);
            }
        }
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowSummonData(_SummonMotionData);
    }

    public void ShowSummonData(SummonMotionData summonData)
    {
        //if (_ArraySelect != null)
        //{
        //    _ArraySelect.SetActive(false);
        //}

        _SummonMotionData = summonData;

        if (_SummonMotionData == null)
        {
            _InfoPanel.SetActive(false);
            return;
        }

        _InfoPanel.SetActive(true);
        _Name.text = CommonDefine.GetQualityColorStr(_SummonMotionData.SummonRecord.Quality) + _SummonMotionData.SummonRecord.Name + "</color>";
        _Level.text = "Lv." + _SummonMotionData.Level;
        for (int i = 0; i < _Stars.Length; ++i)
        {
            if (i < _SummonMotionData.StarLevel)
            {
                _Stars[i].gameObject.SetActive(true);
            }
            else
            {
                _Stars[i].gameObject.SetActive(false);
            }
        }
    }

    #region 

    public override void OnItemClick()
    {
        base.OnItemClick();
    }

    public void SetArraySelected(bool isSelected)
    {
        _ArraySelect.SetActive(isSelected);
    }

    #endregion
}

