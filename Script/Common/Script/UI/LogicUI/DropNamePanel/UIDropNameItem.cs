using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class UIDropNameItem : UIItemBase
    {
        public Text _DropName;

        private RectTransform _RectTransform;
        private DropItem _DropItem;
        private Transform _FollowTransform;
        private Vector3 _HeightDelta;

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            _DropItem = hash["InitObj"] as DropItem;
            _RectTransform = GetComponent<RectTransform>();
            _FollowTransform = _DropItem.transform;

            _HeightDelta.x = 0;
            _HeightDelta.z = 0;
            _HeightDelta.y += 0.2f;

            _DropName.text = _DropItem._DropName;
        }


        public void Update()
        {
            if (_FollowTransform == null)
            {
                UIDropNamePanel.HideDropItem(this);
            }
            else
            {
                _RectTransform.anchoredPosition = UIManager.Instance.WorldToScreenPoint(_FollowTransform.position + _HeightDelta);
            }
        }

        #region 

        public override void OnItemClick()
        {
            base.OnItemClick();

            MonsterDrop.PickItem(_DropItem);
        }

        #endregion
    }
}
