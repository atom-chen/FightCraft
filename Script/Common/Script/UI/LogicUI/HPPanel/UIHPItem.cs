using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIHPItem : UIItemBase
{
    public Slider _HPProcess;
    public Slider _MPProcess;

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
        var transform = _FollowTransform.Find("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck");

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

        if (_ObjMotion._SkillProcessing > 0 && _ObjMotion._SkillProcessing < 1)
        {
            _MPProcess.gameObject.SetActive(true);
            _MPProcess.value = _ObjMotion._SkillProcessing;
        }
        else
        {
            _MPProcess.gameObject.SetActive(false);
        }

        _RectTransform.anchoredPosition = UIManager.Instance.WorldToScreenPoint(_FollowTransform.position + _HeightDelta);
    }

}

