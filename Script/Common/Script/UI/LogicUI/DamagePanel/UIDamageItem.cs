using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameBase;

namespace GameUI
{
    public enum ShowDamageType
    {
        Normal,
        Critical,
        Hurt,
        Heal,
    }

    public class UIDamageItem : UIItemBase
    {
        public Text DamageValue1;
        public Text DamageValue2;
        public RectTransform _RootTransform;
        public RectTransform _TextTransform;

        private const string _NormalColor = "<color=#ffff00dd>";
        private const string _CriticalColor = "<color=#FF5900dd>";
        private const string _HurtColor = "<color=#FF0000dd>";
        private const string _HealColor = "<color=#00FF00dd>";

        public void Show(Vector3 showWorldPos, int showValue1, int showValue2, ShowDamageType showType, int baseSize)
        {
            gameObject.SetActive(true);
            switch (showType)
            {
                case ShowDamageType.Normal:
                    DamageValue1.text = _NormalColor + showValue1.ToString() + "</color>";
                    break;
                case ShowDamageType.Critical:
                    DamageValue1.text = _CriticalColor + showValue1.ToString() + "</color>";
                    break;
                case ShowDamageType.Hurt:
                    DamageValue1.text = _HurtColor + showValue1.ToString() + "</color>";
                    break;
                case ShowDamageType.Heal:
                    DamageValue1.text = _HealColor + showValue1.ToString() + "</color>";
                    break;
            }
            
            if (showValue2 > 0)
            {
                DamageValue2.text = showValue2.ToString();
            }
            else
            {
                DamageValue2.text = "";
            }

            _RootTransform.anchoredPosition = UIManager.Instance.WorldToScreenPoint(showWorldPos);
            _TextTransform.localScale = new Vector3(baseSize, baseSize, baseSize);
        }


    }
}
