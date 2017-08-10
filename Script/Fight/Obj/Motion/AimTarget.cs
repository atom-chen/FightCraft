using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimTarget : InstanceBase<AimTarget>
{

    void Start()
    {
        SetInstance(this);
        GameUI.UIControlPanel.AddClickEvent(OnTargetPointClick);
        GameUI.UISkillBar.SetAimTypeStatic(_AimType);
    }

    void OnDestory()
    {
        SetInstance(null);
    }

    void Update()
    {
        FreeAimUpdate();
    }

    public enum AimTargetType
    {
        None,
        Free,
        Lock,
    }

    public AimTargetType _AimType = AimTargetType.Free;

    public void SwitchAimType(int aimType)
    {
        _AimType = (AimTargetType)aimType;
        if (_AimType == AimTargetType.None)
        {
            GameUI.AimTargetPanel.HideAimTarget();
        }
    }

    #region lock target

    private MotionManager _LockTarget;
    public MotionManager LockTarget
    {
        get
        {
            return _LockTarget;
        }
    }

    private void OnTargetPointClick(PointerEventData eventData)
    {
        if (_AimType == AimTargetType.None)
            return;

        Debug.Log("OnTargetPointClick");
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, 1 << FightLayerCommon.CAMP_2))
        {
            var targetMotion = hit.collider.GetComponentInParent<MotionManager>();
            if (targetMotion != null && !targetMotion.IsMotionDie)
            {
                _AimType = AimTargetType.Lock;
                _LockTarget = targetMotion;
            }
        }

        if (_LockTarget == null)
            return;

        GameUI.UISkillBar.SetAimTypeStatic(_AimType);
        GameUI.AimTargetPanel.ShowAimTarget(_LockTarget, AimType.Lock);

    }

    #endregion

    #region free aim

    private void FreeAimUpdate()
    {
        if (_AimType != AimTargetType.Free)
            return;

        var selecteds = SelectTargetCommon.GetFrontMotions(FightManager.Instance.MainChatMotion, 5, 60, SelectTargetCommon.SelectSortType.Angel);
        if (selecteds.Count > 0)
        {
            _LockTarget = selecteds[0]._SelectedMotion;
        }
        else
        {
            _LockTarget = null;
        }

        if (_LockTarget == null)
        {
            GameUI.AimTargetPanel.HideAimTarget();
            return;
        }

        GameUI.AimTargetPanel.ShowAimTarget(_LockTarget, AimType.Free);
    }

    #endregion
}
