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
            GameCore.Instance.UIManager.ShowUI("SystemUI/UILogin", UILayer.BaseUI, hash);
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

            GuiTextDebug.debug("UILogin show");
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
            GuiTextDebug.debug("UILogin OnPointerClick:");
            if (_LoadSceneOperation == null)
            {
                GuiTextDebug.debug("UILogin _LoadSceneOperation null");
            }
            else
            {
                GuiTextDebug.debug("UILogin _LoadSceneOperation:" + _LoadSceneOperation.progress);
            }
            if (_InitOK)
            {
                LogicManager.Instance.StartLogic();
                Destory();
            }
        }

        #endregion
    }
}
