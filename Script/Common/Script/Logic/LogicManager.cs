using UnityEngine;
using System.Collections;
using System;

using Tables;
using UnityEngine.SceneManagement;

public class LogicManager
{
    #region 唯一

    private static LogicManager _Instance = null;
    public static LogicManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new LogicManager();
            }
            return _Instance;
        }
    }

    private LogicManager() { }

    #endregion

    #region start logic

    public void StartLoadLogic()
    {
        SceneManager.LoadScene(GameDefine.GAMELOGIC_SCENE_NAME);

        PlayerDataPack.Instance.LoadClass(false);
        PlayerDataPack.Instance.InitPlayerData();
    }

    public void StartLoadRole(int idx)
    {
        PlayerDataPack.Instance.SelectRole(idx);
        BackBagPack.Instance.LoadClass(true);
        BackBagPack.Instance.InitBackPack();

        UIMainFun.ShowAsyn();
    }

    #endregion

    #region

    public void StartLogic()
    {
        if (PlayerDataPack.Instance._SelectedRole == null)
        {
            UIRoleSelect.ShowAsyn();
        }
        else
        {
            UIMainFun.ShowAsyn();
        }
    }

    public void SaveGame()
    {
        PlayerDataPack.Instance.SaveClass(true);
        BackBagPack.Instance.SaveClass(true);
        if (PlayerDataPack.Instance._SelectedRole != null)
        {
            PlayerDataPack.Instance._SelectedRole.SaveClass(true);
        }
        //DataManager.Instance.Save();
    }

    public void QuitGame()
    {
        try
        {
            SaveGame();
            DataLog.StopLog();
            Application.Quit();
        }
        catch (Exception e)
        {
            Application.Quit();
        }
    }

    public void CleanUpSave()
    {

    }
    #endregion

    #region Fight

    private StageInfoRecord _EnterStageInfo;
    public StageInfoRecord EnterStageInfo
    {
        get
        {
            return _EnterStageInfo;
        }
    }

    public void EnterFight(StageInfoRecord enterStage)
    {
        _EnterStageInfo = enterStage;
        //var sceneLoader = GameCore.Instance.SceneManager.ChangeFightScene(_EnterStageInfo.ScenePath);

        GameCore.Instance.UIManager.HideAllUI();

        UILoadingScene.ShowAsyn(_EnterStageInfo.ScenePath);
    }

    public void EnterFightFinish()
    {
        UIControlPanel.ShowAsyn();

        InitFightScene();

        UIJoyStick.ShowAsyn();
        UISkillBar.ShowAsyn();
        UIDropNamePanel.ShowAsyn();
        UIHPPanel.ShowAsyn();
        UIPlayerFrame.ShowAsyn();
        UITargetFrame.ShowAsyn();

        UIDamagePanel.ShowAsyn();
        //DamagePanel.ShowAsyn();
        AimTargetPanel.ShowAsyn();
    }

    public void ExitFight()
    {
        //var sceneLoader = GameCore.Instance.SceneManager.ChangeLogicScene();

        UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);

        GameCore.Instance.UIManager.HideAllUI();

    }

    private void InitFightScene()
    {
        GameObject fightGO = new GameObject("FightManager");
        fightGO.AddComponent<FightManager>();
    }


    #endregion
}

