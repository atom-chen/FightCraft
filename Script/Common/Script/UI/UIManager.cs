using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using System.IO;
using System;
using System.Reflection;

namespace GameUI
{
    public class UIManager : MonoBehaviour
    {
        #region 固有

        public void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(_InputEventSystem);
            _Instance = this;
        }

        public void Start()
        {
            InitUIEvents();
            PushInitEvent();
        }

        #endregion

        #region 唯一

        private static UIManager _Instance = null;
        public static UIManager Instance
        {
            get
            {
                return _Instance;
            }
        }
        #endregion

        #region

        /// <summary>
        /// 屏幕画布
        /// </summary>
        [SerializeField]
        private Canvas _ScreenCanvas;
        public Canvas ScreenCanvas { get { return _ScreenCanvas; } }

        /// <summary>
        /// 事件系统
        /// </summary>
        [SerializeField]
        private EventSystem _InputEventSystem;

        private GameObject InitUI(string path)
        {
            var tempGO = ResourceManager.Instance.GetUI(path);
            if (tempGO != null)
            {
                var uiGO =  GameObject.Instantiate(tempGO);
                if (_ScreenCanvas != null)
                {
                    uiGO.transform.parent = _ScreenCanvas.transform;
                }
                else
                {
                    var canvas = GameObject.Find("Canvas");
                    if (canvas == null)
                        return null;
                    uiGO.transform.parent = canvas.transform;
                }
                uiGO.transform.position = Vector3.zero;
                var trans = uiGO.GetComponent<RectTransform>();
                trans.anchoredPosition = Vector2.zero;
                trans.sizeDelta = Vector2.zero;
                return uiGO;
            }
            return null;
        }

        private Dictionary<string, UIBase> _UIObjs = new Dictionary<string, UIBase>();
        public void ShowOrCreateUI(string uiPath, Hashtable hashtable)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                _UIObjs[uiPath].Show(hashtable);
            }
            else
            {
                var obj = InitUI(uiPath);
                obj.transform.localScale = new Vector3(1,1,1);
                var script = obj.GetComponent<UIBase>();
                obj.name = script.GetType().Name;
                script.Show(hashtable);
                script.UIPath = uiPath;
                _UIObjs.Add(uiPath, script);
            }
        }

        //异步显示
        public void ShowUI(string uiPath, Hashtable hashtable = null)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                ShowOrCreateUI(uiPath, hashtable);
            }
            else
            {
                if (hashtable == null)
                {
                    hashtable = new Hashtable();
                }
                hashtable.Add("UIPath", uiPath);
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_CREATE, this, hashtable);
            }
        }

        public void DestoryUI(string uiPath)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                _UIObjs.Remove(uiPath);
            }
        }

        public void InitAllUI()
        {
            foreach (var uiEvent in _InitEvent)
            {
                if (!_UIObjs.ContainsKey(uiEvent.UIPath) && uiEvent.UIPath != "SystemUI/UILogin")
                {
                    var obj = InitUI(uiEvent.UIPath);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    var script = obj.GetComponent<UIBase>();
                    obj.name = script.GetType().Name;
                    script.UIPath = uiEvent.UIPath;
                    script.PreLoad();
                    _UIObjs.Add(uiEvent.UIPath, script);
                }
            }
        }

        public void HideAllUI()
        {
            foreach (var uiPair in _UIObjs)
            {
                uiPair.Value.Hide();
            }
            _UIObjs.Clear();
        }

        public void DestoryAllUI()
        {
            foreach (var uiPair in _UIObjs)
            {
                uiPair.Value.Destory();
            }
            _UIObjs.Clear();
        }

        #endregion

        #region 初始化事件

        private class UIInitEvent
        {
            public EVENT_TYPE EventType;
            public string UIPath;
        }

        private List<UIInitEvent> _InitEvent = new List<UIInitEvent>();
        private Dictionary<EVENT_TYPE, List<string>> _InitUIs;

        public const string UI_SYSTEM_SCRIPT_PATH = "\\FightCraft\\Script\\Common\\Script\\UI\\SystemUI";
        public const string UI_LOGIC_SCRIPT_PATH = "\\FightCraft\\Script\\Common\\Script\\UI\\LogicUI";
        public const int PATH_FOLD_CNT = 6;

        private void InitUIEvents()
        {

        }

        private static string GetUIObjPath(string path)
        {

            string uiObjPath = path.Replace(Application.dataPath, "_");
            string[] pathStrs = uiObjPath.Split('\\');
            string uiPath = "";
            for (int i = PATH_FOLD_CNT; i < pathStrs.Length - 1; ++i)
            {
                uiPath += pathStrs[i] + '/';
            }
            uiPath += Path.GetFileNameWithoutExtension(path);
            return uiPath;
        }

        private void PushInitEvent()
        {
            _InitUIs = new Dictionary<EVENT_TYPE, List<string>>();
            foreach (var uievent in _InitEvent)
            {
                if (_InitUIs.ContainsKey(uievent.EventType))
                {
                    _InitUIs[uievent.EventType].Add(uievent.UIPath);
                }
                else
                {
                    List<string> pathList = new List<string>();
                    pathList.Add(uievent.UIPath);
                    _InitUIs.Add(uievent.EventType, pathList);
                }
                GameCore.Instance.EventController.RegisteEvent(uievent.EventType, ShowEvent);
            }

            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_CREATE, ShowLogicUIEvent);
        }

        public void ShowEvent(object sender, Hashtable hash)
        {
            EVENT_TYPE type = (EVENT_TYPE)hash["EVENT_TYPE"];
            if (_InitUIs.ContainsKey(type))
            {
                foreach (var initUI in _InitUIs[type])
                {
                    ShowOrCreateUI(initUI, hash);
                }
            }
        }

        public void ShowLogicUIEvent(object sender, Hashtable hash)
        {
            string uiPath = (string)hash["UIPath"];
            ShowOrCreateUI(uiPath, hash);
        }
        #endregion
    }
}
