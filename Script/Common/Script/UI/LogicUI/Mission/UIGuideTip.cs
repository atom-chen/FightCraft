
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIGuideTip : UIBase
{

    #region static funs

    public static void ShowGuideStep(int guideStep)
    {
        if (_ShowingGuideID == guideStep)
            return;

        Hashtable hash = new Hashtable();
        hash.Add("GuideStep", guideStep);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIGuideTip, UILayer.BaseUI, hash);
    }

    public static void CloseGuide()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGuideTip>(UIConfig.UIGuideTip);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Hide();
    }

    public static bool IsFightGuide(int guideID)
    {
        if (guideID < 1000 && guideID > 0)
        {
            return true;
        }
        return false;
    }

    public static bool IsUIGuide(int guideID)
    {
        if (guideID >= 1000)
        {
            return true;
        }
        return false;
    }

    public static int ShowingGuide()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGuideTip>(UIConfig.UIGuideTip);
        if (instance == null)
            return 0;

        if (!instance.isActiveAndEnabled)
            return 0;

        return _ShowingGuideID;
    }

    #endregion

    private static int _ShowingGuideID = 0;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _ShowingGuideID = 0;
        if (hash.ContainsKey("GuideStep"))
        {
            int guideStep = (int)hash["GuideStep"];
            _ShowingGuideID = guideStep;
            if (guideStep < 1000)
            {
                ShowSkillAnim(guideStep);
            }
            else
            {
                ShowPointUIAnim(guideStep);
            }
        }
    }

    private void Update()
    {
        AnimUpdate();
    }

    #region guide skill

    public GameObject _SkillTipGO;
    public Animator _AnimBtnJ1;
    public Animator _AnimBtnJ2;
    public Animator _AnimBtnJ3;
    public Animator _AnimBtnJ4;
    public Animator _AnimBtnU;
    public Animator _AnimBtnK;
    public float _AnimInterval;

    private List<Animator> _ActAnims = new List<Animator>();

    private void ShowSkillAnim(int guideStep)
    {
        _SkillTipGO.SetActive(true);
        _PointGO.SetActive(false);
        _ActAnims.Clear();
        switch (guideStep)
        {
            case 1:
                _ActAnims.Add(_AnimBtnJ1);
                _ActAnims.Add(_AnimBtnJ2);
                _ActAnims.Add(_AnimBtnK);
                break;
            case 2:
                _ActAnims.Add(_AnimBtnJ1);
                _ActAnims.Add(_AnimBtnJ2);
                _ActAnims.Add(_AnimBtnJ3);
                _ActAnims.Add(_AnimBtnK);
                break;
            case 3:
                _ActAnims.Add(_AnimBtnJ1);
                _ActAnims.Add(_AnimBtnJ2);
                _ActAnims.Add(_AnimBtnJ3);
                _ActAnims.Add(_AnimBtnJ4);
                _ActAnims.Add(_AnimBtnK);
                break;
            case 4:
                _ActAnims.Add(_AnimBtnJ1);
                _ActAnims.Add(_AnimBtnJ2);
                _ActAnims.Add(_AnimBtnJ3);
                _ActAnims.Add(_AnimBtnU);
                break;
            case 5:
                _ActAnims.Add(_AnimBtnU);
                break;
            case 6:
                _ActAnims.Add(_AnimBtnJ1);
                _ActAnims.Add(_AnimBtnJ2);
                _ActAnims.Add(_AnimBtnJ3);
                _ActAnims.Add(_AnimBtnU);
                _ActAnims.Add(_AnimBtnK);
                break;
            case 7:
                _ActAnims.Add(_AnimBtnJ1);
                _ActAnims.Add(_AnimBtnJ2);
                _ActAnims.Add(_AnimBtnK);
                _ActAnims.Add(_AnimBtnU);
                _ActAnims.Add(_AnimBtnJ3);
                break;
        }

        StartActSkillAnim();
    }

    private void StartActSkillAnim()
    {
        _AnimBtnJ1.gameObject.SetActive(false);
        _AnimBtnJ2.gameObject.SetActive(false);
        _AnimBtnJ3.gameObject.SetActive(false);
        _AnimBtnJ4.gameObject.SetActive(false);
        _AnimBtnU.gameObject.SetActive(false);
        _AnimBtnK.gameObject.SetActive(false);

        for (int i = 0; i < _ActAnims.Count; ++i)
        {
            _ActAnims[i].gameObject.SetActive(true);
            _ActAnims[i].transform.SetAsLastSibling();
        }
        _AnimStartTime = Time.time - _AnimInterval;
        _AnimIdx = 0;
    }

    private float _AnimStartTime;
    private int _AnimIdx = 0;
    private void AnimUpdate()
    {
        if (!_SkillTipGO.activeSelf)
            return;

        if (Time.time - _AnimStartTime > _AnimInterval)
        {
            _ActAnims[_AnimIdx].Play("Press");
            _AnimStartTime = Time.time;

            ++_AnimIdx;
            if (_AnimIdx == _ActAnims.Count)
            {
                _AnimIdx = 0;
            }
        }
    }
    #endregion

    #region point UI

    public enum PointDirect
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    [System.Serializable]
    public class PointUIInfo
    {
        public int GuideID;
        public string PointPath;
        public PointDirect PointDirect;
    }

    [SerializeField]
    public List<PointUIInfo> _PointUIInfos = new List<PointUIInfo>();

    public GameObject _PointGO;

    public void ShowPointUIAnim(int guideID)
    {
        _SkillTipGO.SetActive(false);
        _PointGO.SetActive(true);

        for (int i = 0; i < _PointUIInfos.Count; ++i)
        {
            if (_PointUIInfos[i].GuideID == guideID)
            {
                
                StartPointUIAnim(_PointUIInfos[i]);
            }
        }
    }

    public void StartPointUIAnim(PointUIInfo pointInfo)
    {
        if (pointInfo.PointPath.Contains("LargeBtn"))
        {
            if (UIMainFun.IsShowInFight())
            {
                pointInfo.PointPath = pointInfo.PointPath.Replace("LargeBtn", "SmallBtn");
            }
        }

        if (pointInfo.PointPath.Contains("SmallBtn"))
        {
            if (!UIMainFun.IsShowInFight())
            {
                pointInfo.PointPath = pointInfo.PointPath.Replace("SmallBtn", "LargeBtn");
            }
        }

        var pointTarget = GameObject.Find(pointInfo.PointPath);
        if (pointTarget == null)
        {
            Debug.LogError("Not find point info:" + pointInfo.PointPath);
        }

        _PointGO.transform.position = pointTarget.transform.position;
        switch (pointInfo.PointDirect)
        {
            case PointDirect.RIGHT:
                _PointGO.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case PointDirect.UP:
                _PointGO.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case PointDirect.LEFT:
                _PointGO.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case PointDirect.DOWN:
                _PointGO.transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }

    #endregion

}

