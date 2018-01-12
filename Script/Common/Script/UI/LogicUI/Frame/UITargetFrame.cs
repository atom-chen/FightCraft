using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetFrame : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Frame/UITargetFrame", UILayer.BaseUI, hash);
    }

    #endregion

    void Update()
    {
        HpUpdate();
    }

    #region 

    public GameObject _FrameRoot;
    public Slider _HPProcess;
    public Text _HPText;

    private MotionManager _TargetMotion;

    private void HpUpdate()
    {
        if (!AimTarget.Instance)
            return;

        if (AimTarget.Instance.LockTarget != null)
        {
            _TargetMotion = AimTarget.Instance.LockTarget;
        }

        if (_TargetMotion != null)
        {

            _HPText.text = _TargetMotion.RoleAttrManager.HP + "/" + _TargetMotion.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax);
            _HPProcess.value = _TargetMotion.RoleAttrManager.HPPersent;
        }
    }

    #endregion
}
