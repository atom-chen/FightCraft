using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class UIBase : MonoBehaviour
    {
        public string UIPath;

        #region fiex fun

        public void Awake()
        {
            Init();
        }
        public virtual void Init()
        {
            InitEvent();
        }

        public void OnDestory()
        {

        }

        #endregion

        #region show

        public virtual void PreLoad()
        {
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        public virtual void Show(Hashtable hash)
        {
            Show();
        }

        public virtual void Hide()
        {
            if (gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void ShowDelay(float time)
        {
            Hide();
            Invoke("Show", time);
        }

        public virtual void ShowLast(float time)
        {
            Show();
            Invoke("Hide", time);
        }

        public virtual void Destory()
        {
            UnRegisteEvents();
            UIManager.Instance.DestoryUI(UIPath);
            GameObjectManager.Instance.DestoryGameObject(gameObject);
        }

        #endregion

        #region event

        protected Dictionary<EVENT_TYPE, EventController.EventDelegate> _HandleList = new Dictionary<EVENT_TYPE, EventController.EventDelegate>();

        protected virtual void InitEvent()
        {

        }

        protected void RegisteEvent(EVENT_TYPE eventType, EventController.EventDelegate handle)
        {
            _HandleList.Add(eventType, handle);
            GameCore.Instance.EventController.RegisteEvent(eventType, handle);
        }

        protected void UnRegisteEvents()
        {
            foreach (var eventHandle in _HandleList)
            {
                GameCore.Instance.EventController.UnRegisteEvent(eventHandle.Key, eventHandle.Value);
            }
        }
        #endregion
    }
}
