using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;
using GameLogic;
namespace GameUI
{
    public class UIBagPack : UIBase
    {

        #region static funs

        public static void ShowAsyn()
        {
            Hashtable hash = new Hashtable();
            GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIBagPack", UILayer.PopUI, hash);
        }

        public static void RefreshBagItems()
        {
            var instance = GameCore.Instance.UIManager.GetUIInstance<UIBagPack>("LogicUI/BagPack/UIBagPack");
            if (instance == null)
                return;

            if (!instance.isActiveAndEnabled)
                return;

            instance.RefreshItems();
        }

        #endregion

        #region 

        public UIContainerBase _EquipContainer;
        public UIContainerSelect _ItemsContainer;
        public UIEquipItem _DragItem;

        #endregion

        #region 

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_UI_ITEMS_REFRESH, ItemRefresh);

            ShowPackItems();
        }

        public override void Hide()
        {
            base.Hide();

            GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_UI_ITEMS_REFRESH, ItemRefresh);
        }

        private void ShowPackItems()
        {
            Hashtable exHash = new Hashtable();
            exHash.Add("UIBagPack", this);

            _EquipContainer.InitContentItem(PlayerDataPack.Instance._SelectedRole._EquipList, ShowEquipPackTooltips, exHash);
            _ItemsContainer.InitSelectContent(PlayerDataPack.Instance._SelectedRole._BackPackItems, null, ShowBackPackTooltips, null, exHash);

        }

        public void RefreshItems()
        {
            _EquipContainer.RefreshItems();
            _ItemsContainer.RefreshItems();
        }

        private void ShowBackPackTooltips(object equipObj)
        {
            ItemEquip equipItem = equipObj as ItemEquip;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInBackPack);
        }

        private void ShowEquipPackTooltips(object equipObj)
        {
            ItemEquip equipItem = equipObj as ItemEquip;
            if (equipItem == null || !equipItem.IsVolid())
                return;

            UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
        }
        #endregion

        #region 

        private void ItemRefresh(object sender, Hashtable args)
        {
            RefreshItems();
        }

        #endregion

        #region Drag

        public ItemBase GetDragItem()
        {
            if (IsDraging())
            {
                return _DragItem.ShowItem;
            }

            return null;
        }

        public bool IsDraging()
        {
            return _DragItem.gameObject.activeSelf;
        }

        public void SetDragInfo(ItemBase dragItem)
        {
            ItemEquip dragEquip = dragItem as ItemEquip;
            if (dragEquip == null)
            {
                _DragItem.Hide();
            }
            else
            {
                _DragItem.ShowEquip(dragEquip);
                _DragItem.Show();
            }
        }

        public void SetDragItemPos(Vector3 position)
        {
            _DragItem.transform.position = position;
        }

        #endregion

    }
}
