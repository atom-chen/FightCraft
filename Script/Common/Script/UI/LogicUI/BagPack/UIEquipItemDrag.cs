
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using GameLogic;
using UnityEngine.EventSystems;
using System;

namespace GameUI
{
    public class UIEquipItemDrag : UIEquipItem, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
       
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

            _DropEnable.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_BagPack == null)
                return;

            if (!_BagPack.IsDraging())
                return;

            if (_DropEnable == null)
                return;

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

            _DropEnable.gameObject.SetActive(false);

            ShowItem.ExchangeInfo(_BagPack.GetDragItem());
            _BagPack.RefreshItems();
        }

        #endregion

    }
}
