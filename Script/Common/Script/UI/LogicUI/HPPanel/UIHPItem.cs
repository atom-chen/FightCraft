﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class UIHPItem : UIItemBase
    {
        public Slider _HPProcess;

        private RectTransform _RectTransform;
        private MotionManager _ObjMotion;
        private Transform _FollowTransform;
        private Vector3 _HeightDelta;

        public override void Show(Hashtable hash)
        {
            base.Show(hash);

            _ObjMotion = hash["InitObj"] as MotionManager;
            _RectTransform = GetComponent<RectTransform>();
            _FollowTransform = _ObjMotion.AnimationEvent.transform;
            var transform = _FollowTransform.FindChild("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/head/Bip001 HeadNub");

            _HeightDelta = transform.position - _FollowTransform.position;
            _HeightDelta.x = 0;
            _HeightDelta.z = 0;
            _HeightDelta.y += 0.2f;
        }


        public void Update()
        {
            if (_ObjMotion == null || _ObjMotion.IsMotionDie)
            {
                UIHPPanel.HideHPItem(this);
                gameObject.SetActive(false);
                return;
            }

            _HPProcess.value = _ObjMotion.RoleAttrManager.HPPersent;
            _RectTransform.anchoredPosition = UIManager.Instance.WorldToScreenPoint(_FollowTransform.position + _HeightDelta);
        }

    }
}