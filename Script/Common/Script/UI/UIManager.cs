using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using System.IO;
using System;
using System.Reflection;
using UnityEngine.UI;

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
        private Dictionary<UILayer, RectTransform> _UILayers = new Dictionary<UILayer, RectTransform>();

        public void ShowOrCreateUI(string uiPath, UILayer uilayer, Hashtable hashtable)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                _UIObjs[uiPath].Show(hashtable);
            }
            else
            {
                if (!_UILayers.ContainsKey(uilayer))
                {
                    GameObject layer = new GameObject(uilayer.ToString());
                    layer.transform.SetParent(ScreenCanvas.transform);
                    RectTransform rectTransform = layer.AddComponent<RectTransform>();
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    rectTransform.localScale = Vector3.one;
                    _UILayers.Add(uilayer, rectTransform);
                }
                var obj = InitUI(uiPath);
                obj.transform.localScale = new Vector3(1,1,1);
                obj.transform.SetParent(_UILayers[uilayer]);
                var script = obj.GetComponent<UIBase>();
                obj.name = script.GetType().Name;
                script.Show(hashtable);
                script.UIPath = uiPath;
                _UIObjs.Add(uiPath, script);
            }

            UIShowed(uiPath, uilayer, _UIObjs[uiPath]);
        }

        //异步显示
        public void ShowUI(string uiPath, UILayer uilayer, Hashtable hashtable = null)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                ShowOrCreateUI(uiPath, uilayer, hashtable);
            }
            else
            {
                if (hashtable == null)
                {
                    hashtable = new Hashtable();
                }
                hashtable.Add("UIPath", uiPath);
                hashtable.Add("UILayer", uilayer);
                GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_CREATE, this, hashtable);
            }
        }

        public void HideUI(string uiPath)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                _UIObjs[uiPath].Hide();
            }
        }

        public void DestoryUI(string uiPath)
        {
            if (_UIObjs.ContainsKey(uiPath))
            {
                _UIObjs.Remove(uiPath);
            }
        }

        public void DestoryUI(UIBase uiBase)
        {
            if (_UIObjs.ContainsValue(uiBase))
            {
                string uiKey = "";
                foreach (var ui in _UIObjs)
                {
                    if (ui.Value == uiBase)
                    {
                        uiKey = ui.Key;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(uiKey))
                {
                    _UIObjs.Remove(uiKey);
                }
            }
            GameObject.Destroy(uiBase.gameObject);
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

        #region rayCast

        //防死循环
        bool _RayCasting = false;
        public void RayCastBebind(PointerEventData eventData)
        {
            if (_RayCasting)
                return;

            _RayCasting = true;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            _InputEventSystem.RaycastAll(eventData, raycastResults);
            if (raycastResults.Count > 0)
            {
                var pointClick = raycastResults[0].gameObject.GetComponent<Graphic>();
                //if (ExecuteEvents.CanHandleEvent<IPointerClickHandler>(raycastResults[0].gameObject))
                {
                    ExecuteEvents.ExecuteHierarchy<IPointerClickHandler>(raycastResults[0].gameObject, eventData, (x, y) => x.OnPointerClick(eventData));
                }
            }
            _RayCasting = false;
        }

        #endregion

        #region cameraPos

        private RectTransform _UICanvasRect;

        public Vector3 WorldToScreenPoint(Vector3 worldPos)
        {
            if (_UICanvasRect == null)
            {
                _UICanvasRect = gameObject.GetComponent<RectTransform>();
            }

            Vector3 screenPos = Camera.main.WorldToViewportPoint(worldPos);//1024
            //screenPos -= new Vector3(0.5f, 0.5f, 0.5f);
            return new Vector3(screenPos.x * _UICanvasRect.sizeDelta.x, screenPos.y * _UICanvasRect.sizeDelta.y, screenPos.z * 1);
        }

        #endregion

        #region 初始化事件

        private void InitUIEvents()
        {
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_CREATE, ShowLogicUIEvent);
        }
        
        public void ShowLogicUIEvent(object sender, Hashtable hash)
        {
            string uiPath = (string)hash["UIPath"];
            UILayer uilayer = (UILayer)hash["UILayer"];
            ShowOrCreateUI(uiPath, uilayer, hash);
        }
        #endregion

        #region UIConfilict

        private void UIShowed(string uipath, UILayer uilayer, UIBase showUI)
        {
            if (uilayer == UILayer.PopUI)
            {
                PopUIConflict(showUI);
            }
        }

        private UIBase _ShowingPopUI;
        private void PopUIConflict(UIBase showUI)
        {
            if (_ShowingPopUI != null && _ShowingPopUI.IsShowing())
            {
                _ShowingPopUI.Hide();
            }
            _ShowingPopUI = showUI;
        }

        #endregion
    }
}
