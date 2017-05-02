using UnityEngine;
using System.Collections;
using System;

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

            GameUI.UIRoleSelect.ShowAsyn();
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

        public void EnterFight(string sceneName)
        {

            var sceneLoader = GameCore.Instance.SceneManager.ChangeFightScene(sceneName);

            GameCore.Instance.UIManager.HideAllUI();

            GameUI.UILoadingScene.ShowAsyn(GameCore.Instance.SceneManager.GetFightSceneName(), sceneLoader);

        }

        public void EnterFightFinish()
        {
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_FIGHT_START, this, new Hashtable());

            InitFightScene();
            GameUI.UIJoyStick.ShowAsyn();
            GameUI.UISkillBar.ShowAsyn();
            GameUI.UIHPPanel.ShowAsyn();
            GameUI.UIDropNamePanel.ShowAsyn();
        }

        public void ExitFight()
        {
            var sceneLoader = GameCore.Instance.SceneManager.ChangeLogicScene();


            GameCore.Instance.UIManager.HideAllUI();

            GameUI.UILoadingScene.ShowAsyn(GameDefine.GAMELOGIC_SCENE_NAME, sceneLoader);
        }

        private void InitFightScene()
        {
            GameObject fightGO = new GameObject("FightManager");
            fightGO.AddComponent<FightManager>();
        }


        #endregion
    }
}
