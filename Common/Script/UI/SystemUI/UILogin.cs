using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using GameBase;
using GameLogic;
using UnityEngine.EventSystems;

namespace GameUI
{

    public class UILogin : UIBase , IPointerClickHandler
    {
        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("SystemUI/UILogin", hash);
        }

        public static List<EVENT_TYPE> GetShowEvent()
        {
            List<EVENT_TYPE> showEvents = new List<EVENT_TYPE>();

            showEvents.Add(EVENT_TYPE.EVENT_SYSTEM_START);

            return showEvents;
        }

        #endregion

        #region params

        public GameObject InitOkTex;

        private AsyncOperation _LoadSceneOperation;
        private bool _InitOK = false;

        #endregion

        #region show funs

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            _LoadSceneOperation = LogicManager.Instance.StartLoadLogic();
        }

        public void Update()
        {
            transform.SetSiblingIndex(10000);

            if (_LoadSceneOperation != null && _LoadSceneOperation.isDone)
            {
                _InitOK = true;
                if (InitOkTex != null)
                {
                    InitOkTex.SetActive(true);
                }
            }
        }

        #endregion

        #region inact

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_InitOK)
            {
                LogicManager.Instance.StartLogic();
                Destory();
            }
        }

        #endregion
    }
}
