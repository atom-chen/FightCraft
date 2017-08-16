﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public class AimTargetPanel : InstanceBase<AimTargetPanel>
    {
        #region 

        public void Awake()
        {
            SetInstance(this);
            gameObject.SetActive(false);
        }

        void OnDestory()
        {
            SetInstance(null);
        }

        #endregion

        #region static

        public static void ShowAsyn()
        {
            var gameobj = ResourceManager.Instance.GetInstanceGameObject("UI/LogicUI/AimTarget/AimTargetPanel");
        }

        public static void ShowAimTarget(MotionManager target, AimType aimType)
        {

            var instance = AimTargetPanel.Instance;
            if (instance == null)
                return;

            instance.ShowItemInner(target, aimType);
        }

        public static void HideAimTarget()
        {
            var instance = AimTargetPanel.Instance;
            if (instance == null)
                return;

            instance.HideAim();
        }
        #endregion

        public AimTargetItem _AimItem;

        private MotionManager _AimTarget;

        private void ShowItemInner(MotionManager target, AimType aimType)
        {
            _AimTarget = target;

            gameObject.SetActive(true);
            _AimItem.transform.rotation = Quaternion.Euler(-Camera.main.transform.rotation.eulerAngles + new Vector3(0, 180, 0));
            _AimItem.SetAimType(aimType);
        }

        private void HideAim()
        {
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (_AimTarget != null && !_AimTarget.IsMotionDie)
            {
                _AimItem.transform.position = _AimTarget.transform.position + _AimTarget._ColliderInfo + new Vector3(-0.4f, -0.4f, 0);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }
}
