using UnityEngine;
using System.Collections;
using System;

using Tables;
using GameBase;
namespace GameLogic
{
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

        #region
       
        #endregion


        #region 接口

        public AsyncOperation StartLoadLogic()
        {
            var sceneLoader = GameCore.Instance.SceneManager.ChangeLogicScene();

            DataManager.Instance.Load();

            PlayerDataPack.Instance.InitPlayerData();

            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_LOAD_FINISH, this, new Hashtable());

            return sceneLoader;
        }

        public void StartLogic()
        {
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_LOGIC_START, this, new Hashtable());

            if (PlayerDataPack.Instance._SelectedRole == null)
            {
                GameUI.UIRoleSelect.ShowAsyn();
            }
            else
            {
                GameUI.UIMainFun.ShowAsyn();
            }
        }

        public void SaveGame()
        {
            DataManager.Instance.Save();
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

            GameUI.UILoadingScene.ShowAsyn(_EnterStageInfo.ScenePath);
            //var sceneLoader = GameCore.Instance.SceneManager.ChangeFightScene(_EnterStageInfo.ScenePath);

            GameCore.Instance.UIManager.HideAllUI();
        }

        public void EnterFightFinish()
        {
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_FIGHT_START, this, new Hashtable());

            GameUI.UIControlPanel.ShowAsyn();

            InitFightScene();
            
            GameUI.UIJoyStick.ShowAsyn();
            GameUI.UISkillBar.ShowAsyn();
            GameUI.UIDropNamePanel.ShowAsyn();
            GameUI.UIHPPanel.ShowAsyn();
            
            //GameUI.UIDamagePanel.ShowAsyn();
            GameUI.DamagePanel.ShowAsyn();
            GameUI.AimTargetPanel.ShowAsyn();
        }

        public void ExitFight()
        {
            //var sceneLoader = GameCore.Instance.SceneManager.ChangeLogicScene();

            GameUI.UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME);

            GameCore.Instance.UIManager.HideAllUI();

        }

        private void InitFightScene()
        {
            GameObject fightGO = new GameObject("FightManager");
            fightGO.AddComponent<FightManager>();
        }


        #endregion
    }
}
