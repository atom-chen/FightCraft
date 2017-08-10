using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class UIHPPanel : UIBase
    {
        #region static

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/HPPanel/UIHPPanel", UILayer.BaseUI, hash);
        }

        public static void ShowHPItem(MotionManager motionManager)
        {
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", motionManager);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_HP_ITEM, null, hash);
        }

        public static void HideHPItem(UIHPItem hpItem)
        {
            Hashtable hash = new Hashtable();
            hash.Add("HideItem", hpItem);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_HIDE_HP_ITEM, null, hash);
        }

        #endregion

        public UIHPItem _UIHPItemPrefab;

        private List<UIHPItem> _ShowingItems = new List<UIHPItem>();
        private Stack<UIHPItem> _IdleItems = new Stack<UIHPItem>();

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_SHOW_HP_ITEM, ShowItem);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_HIDE_HP_ITEM, HideItem);
        }

        public override void Hide()
        {
            base.Hide();

            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_UI_SHOW_HP_ITEM, ShowItem);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_UI_HIDE_HP_ITEM, HideItem);
        }

        private void ShowItem(object sender, Hashtable args)
        {
            var itemBase = ResourcePool.Instance.GetIdleUIItem<UIHPItem>(_UIHPItemPrefab.gameObject);
            itemBase.Show(args);
            itemBase.transform.SetParent(transform);
            itemBase.transform.localScale = Vector3.one;
        }

        private void HideItem(object sender, Hashtable args)
        {
            UIHPItem hideItem = args["HideItem"] as UIHPItem;
            ResourcePool.Instance.RecvIldeUIItem(hideItem.gameObject);
        }

        
    }
}
