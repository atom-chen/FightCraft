using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class UIDropNamePanel : UIBase
    {
        #region static

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/DropNamePanel/UIDropNamePanel", UILayer.BaseUI, hash);
        }

        public static void ShowDropItem(DropItem dropItem)
        {
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", dropItem);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_DROP_ITEM, null, hash);
        }

        public static void HideDropItem(UIDropNameItem hpItem)
        {
            Hashtable hash = new Hashtable();
            hash.Add("HideItem", hpItem);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_HIDE_DROP_ITEM, null, hash);
        }

        #endregion

        public UIDropNameItem _UIDropItemPrefab;

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_SHOW_DROP_ITEM, ShowItem);
            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_HIDE_DROP_ITEM, HideItem);
        }

        public override void Hide()
        {
            base.Hide();

            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_UI_SHOW_DROP_ITEM, ShowItem);
            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_UI_HIDE_DROP_ITEM, HideItem);
        }

        private void ShowItem(object sender, Hashtable args)
        {
            var itemBase = ResourcePool.Instance.GetIdleUIItem<UIDropNameItem>(_UIDropItemPrefab);
            itemBase.Show(args);
            itemBase.transform.SetParent(transform);
            itemBase.transform.localScale = Vector3.one;
        }

        private void HideItem(object sender, Hashtable args)
        {
            UIDropNameItem hideItem = args["HideItem"] as UIDropNameItem;
            ResourcePool.Instance.RecvIldeUIItem(hideItem);
        }

        
    }
}
