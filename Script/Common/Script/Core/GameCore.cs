using UnityEngine;
using System.Collections;
using System;

using GameLogic;
using GameUI;

namespace GameBase
{
    /// <summary>
    /// 游戏核心
    /// </summary>
    public class GameCore : MonoBehaviour
    {
        #region 固有
        
        public void Awake()
        {
            DontDestroyOnLoad(this);
            _Instance = this;
        }

        public void Start()
        {
            //Application.targetFrameRate = 60;

            TimeController.Load();
            StartEvent();
            DataLog.StartLog();
        }

        public void Update()
        {
            if ((Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WindowsEditor) && (Input.GetKeyDown(KeyCode.Escape)))
            {
                TimeController.Save();
                LogicManager.Instance.QuitGame();
                Debug.Log("save data");
            }

#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.C))
            {
                LogicManager.Instance.CleanUpSave();
            }

#endif
        }

        void OnApplicationQuit()
        {
            TimeController.Save();
            LogicManager.Instance.QuitGame();
        }
        #endregion

        #region 唯一

        private static GameCore _Instance = null;
        public static GameCore Instance
        {
            get
            {
                return _Instance;
            }
        }

        #endregion

        #region 管理者

        /// <summary>
        /// 事件
        /// </summary>
        [SerializeField]
        private EventController _EventController;
        public EventController EventController { get { return _EventController; } }

        /// <summary>
        /// 主UI画布
        /// </summary>
        [SerializeField]
        private GameUI.UIManager _UIManager;
        public GameUI.UIManager UIManager { get { return _UIManager; } }

        /// <summary>
        /// 时间控制
        /// </summary>
        [SerializeField]
        private TimeController _TimeController;
        public TimeController TimeController { get { return _TimeController; } }

        /// <summary>
        /// 场景管理器
        /// </summary>
        public LogicSceneManager SceneManager { get { return LogicSceneManager.Instance; } }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public ResourceManager ResourceManager { get { return ResourceManager.Instance; } }

        /// <summary>
        /// 游戏对象管理器
        /// </summary>
        public GameObjectManager GameObjectManager { get { return GameObjectManager.Instance; } }

        

        #endregion

        #region 接口

        public void StartEvent()
        {
            var hashTable = new Hashtable();
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_SYSTEM_START, this, hashTable);

            UILogin.ShowAsyn();
        }

#endregion
    }
}
