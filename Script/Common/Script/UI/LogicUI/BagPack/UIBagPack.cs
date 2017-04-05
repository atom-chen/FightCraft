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
            GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIBagPack", hash);
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

            ShowPackItems();
        }

        private void ShowPackItems()
        {
            Hashtable exHash = new Hashtable();
            exHash.Add("UIBagPack", this);

            _EquipContainer.InitContentItem(PlayerDataPack.Instance._SelectedRole._EquipList, null, exHash);
            _ItemsContainer.InitSelectContent(PlayerDataPack.Instance._SelectedRole._BackPackItems, null, null, null, exHash);

        }

        public void RefreshItems()
        {
            _EquipContainer.RefreshItems();
            _ItemsContainer.RefreshItems();
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
