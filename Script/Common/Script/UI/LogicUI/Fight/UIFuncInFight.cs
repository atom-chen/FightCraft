
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIFuncInFight : UIBase
{

    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIFuncInFight, UILayer.BaseUI, hash);

    }

    public static void StopFightTime()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.CancelInvoke("UpdateFightTime");
        Debug.Log("FightTime:" + instance._FightSecond);
    }

    public static void UpdateSkillInfoUI()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.UpdateSkillInfo();
    }

    public static void UpdateKillMonster(int curCnt, int maxCnt)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFuncInFight>(UIConfig.UIFuncInFight);
        if (instance == null)
            return;

        instance.SetMonsterKillCnt(curCnt, maxCnt);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        StartFightTime();
        UpdateSkillInfo();

        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventDelegate);
    }

    public override void Destory()
    {
        base.Destory();

        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, EventDelegate);
    }

    public void OnBtnExit()
    {
        //UIMessageBox.Show(100000, OnExitOk, null);
        //UIFightSetting.ShowAsyn();
        UISystemSetting.ShowAsyn(true);
    }

    private void OnExitOk()
    {
        Debug.Log("exit");
        LogicManager.Instance.ExitFight();
    }

    #region fight time

    public Text _FightTime;

    public int _FightSecond = 0;

    private void StartFightTime()
    {
        InvokeRepeating("UpdateFightTime", 1, 1);
    }

    private void UpdateFightTime()
    {
        ++_FightSecond;
        DateTime dateTime = new DateTime((long)(_FightSecond * 10000000L));
        _FightTime.text = string.Format("{0:mm:ss}", dateTime);
    }

    private void EventDelegate(object go, Hashtable eventArgs)
    {
        Debug.Log("Fight finish time:" + _FightSecond);

    }

    #endregion

    #region  skill info

    public List<UIFightSkillInfo> _UIFightInfos;

    public void UpdateSkillInfo()
    {
        int showIdx = FightSkillManager.Instance.FightSkillDict.Count;
        for (int i = 0; i < _UIFightInfos.Count; ++i)
        {
            --showIdx;
            if (showIdx >= 0)
            {
                if (FightSkillManager.Instance.FightSkillDict[showIdx]._ShowInUI)
                {
                    _UIFightInfos[i].gameObject.SetActive(true);
                    _UIFightInfos[i].InitSkillInfo(FightSkillManager.Instance.FightSkillDict[showIdx]);
                }
                else
                {
                    --showIdx;
                }
            }
            else
            {
                _UIFightInfos[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region kill monster

    public Text _KillMonsterText;

    private void SetMonsterKillCnt(int curCnt, int maxCnt)
    {
        if (maxCnt < 0 && curCnt < 0)
        {
            _KillMonsterText.text = "";
        }
        else if (maxCnt < 0 && curCnt >= 0)
        {
            _KillMonsterText.text = curCnt.ToString();
        }
        else 
        {
            _KillMonsterText.text = string.Format("{0}/{1}", curCnt, maxCnt);
        }
    }

    #endregion
}

