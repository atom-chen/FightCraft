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
        GemData.Instance.LoadClass(true);
        GemData.Instance.InitGemData();

        LegendaryData.Instance.LoadClass(true);
        LegendaryData.Instance.InitLegendaryEquips();

        FiveElementData.Instance.LoadClass(true);
        FiveElementData.Instance.InitFiveElementData();

        PlayerDataPack.Instance.SelectRole(idx);

        //BackBagPack.Instance.LoadClass(true);
        BackBagPack.Instance.InitBackPack();

        ShopData.Instance.LoadClass(true);
        ShopData.Instance.InitShop();

        SummonSkillData.Instance.LoadClass(true);
        SummonSkillData.Instance.InitSummonSkillData();

        ActData.Instance.LoadClass(true);
        ActData.Instance.InitActData();

        MissionData.Instance.LoadClass(true);
        MissionData.Instance.InitMissionData();

        AchievementData.Instance.LoadClass(true);
        AchievementData.Instance.InitMissionData();

        GlobalBuffData.Instance.LoadClass(true);
        GlobalBuffData.Instance.InitGlobalBuffData();

        ItemPackTest.Instance.LoadClass(true);
        ItemPackTest.Instance.Init();

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
        GameCore.Instance._SoundManager.PlayBGMusic(GameCore.Instance._SoundManager._LogicAudio);
    }

    public void SaveGame()
    {
        PlayerDataPack.Instance.SaveClass(false);
        //BackBagPack.Instance.SaveClass(true);
        //ShopData.Instance.SaveClass(true);
        //GemData.Instance.SaveClass(true);


        if (PlayerDataPack.Instance._SelectedRole != null)
        {
            PlayerDataPack.Instance._SelectedRole.SaveClass(false);
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

        UILoadingScene.ShowAsyn(_EnterStageInfo);
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
        UIFuncInFight.ShowAsyn();

        //UIDamagePanel.ShowAsyn();
        //DamagePanel.ShowAsyn();
        //AimTargetPanel.ShowAsyn();

        GameCore.Instance._SoundManager.PlayBGMusic(EnterStageInfo.Audio);
    }

    public void ExitFight()
    {
        
        //var sceneLoader = GameCore.Instance.SceneManager.ChangeLogicScene();
        GameCore.Instance.UIManager.DestoryAllUI();
        UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);
    }

    private void InitFightScene()
    {
        GameObject fightGO = new GameObject("FightManager");
        fightGO.AddComponent<FightManager>();
    }


    #endregion
}

