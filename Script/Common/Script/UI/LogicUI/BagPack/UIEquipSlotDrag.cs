
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;
using Tables;

namespace GameUI
{
    public class UIEquipSlotDrag : UIEquipItem, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {

        #region 

        private EQUIP_SLOT _EquipSlot;

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            int idx = PlayerDataPack.Instance._SelectedRole._EquipList.IndexOf(ShowItem as ItemEquip);
            _EquipSlot = (EQUIP_SLOT)idx;
        }

        #endregion

        #region drag

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_BagPack == null)
                return;

            if (_DisableGO.activeSelf)
                return;

            _BagPack.SetDragInfo(_ShowItem);

        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_BagPack == null)
                return;

            if (!_BagPack.IsDraging())
                return;

            SetDraggedPosition(eventData);
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            if (_BagPack == null)
                return;

            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_BagPack.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                _BagPack.SetDragItemPos(globalMousePos);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _BagPack.SetDragInfo(null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_BagPack == null)
                return;

            if (!_BagPack.IsDraging())
                return;

            if (_DropEnable == null)
                return;

            if (PlayerDataPack.Instance._SelectedRole.IsCanEquipItem(_EquipSlot, _BagPack.GetDragItem() as ItemEquip))
            {
                _DropEnable.gameObject.SetActive(true);
            }
            else
            {
                _DropDisable.gameObject.SetActive(true);
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("OnPointerExit");
            if (_BagPack == null)
                return;

            if (!_BagPack.IsDraging())
                return;

            if (_DropEnable == null)
                return;

            
            _DropDisable.gameObject.SetActive(false);
            _DropEnable.gameObject.SetActive(false);
        }

        public void OnDrop(PointerEventData data)
        {
            if (_BagPack == null)
                return;

            if (!_BagPack.IsDraging())
                return;

            if (_DropEnable == null)
                return;

            _DropDisable.gameObject.SetActive(false);
            if (_DropEnable.gameObject.activeSelf)
            {
                ShowItem.ExchangeInfo(_BagPack.GetDragItem());
                _BagPack.RefreshItems();
                _DropEnable.gameObject.SetActive(false);
            }
            
        }

        #endregion

    }
}
