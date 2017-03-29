using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;

namespace GameUI
{
    public class UILoadingScene : UIBase
    {
        #region static funs

        public static void ShowAsyn(string sceneName, AsyncOperation operation)
        {
            Hashtable hash = new Hashtable();
            hash.Add("SceneLoader", operation);
            hash.Add("SceneName", sceneName);
            GameCore.Instance.UIManager.ShowUI("SystemUI/UILoadingScene", hash);
        }

        public static List<EVENT_TYPE> GetShowEvent()
        {
            List<EVENT_TYPE> showEvents = new List<EVENT_TYPE>();

            showEvents.Add(EVENT_TYPE.EVENT_LOGIC_LOAD);

            return showEvents;
        }

        #endregion

        #region 

        public RawImage BG;

        private AsyncOperation _LoadSceneOperation;
        private string _LoadSceneName;

        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            _LoadSceneOperation = (AsyncOperation)hash["SceneLoader"];
            _LoadSceneName = (string)hash["SceneName"];

            ShowBG();
        }

        public void Update()
        {
            transform.SetSiblingIndex(10000);

            if (_LoadSceneOperation == null || _LoadSceneOperation.isDone)
            {
                if (_LoadSceneName == GameDefine.GAMELOGIC_SCENE_NAME)
                {
                    LogicManager.Instance.StartLogic();
                    base.Destory();
                }
                else if (LogicSceneManager.Instance.IsFightScene( _LoadSceneName))
                {
                    LogicManager.Instance.EnterFightFinish();
                    base.Destory();
                }
                else
                {
                    base.Destory();
                }
            }
        }

        #endregion

        #region 

        public void ShowBG()
        {
            if (_LoadSceneName == GameDefine.GAMELOGIC_SCENE_NAME)
            {
                BG.texture = ResourceManager.Instance.GetTexture("Loading");
            }
            else if (LogicSceneManager.Instance.IsFightScene(_LoadSceneName))
            {
                BG.texture = ResourceManager.Instance.GetTexture("LoadFight");
            }
        }

        #endregion
    }
}
