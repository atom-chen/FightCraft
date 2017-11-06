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

    private void HpUpdate()
    {
        if (!AimTarget.Instance)
            return;

        if (!AimTarget.Instance.LockTarget)
        {
            return;
        }

        _HPText.text = AimTarget.Instance.LockTarget.RoleAttrManager.HP + "/" + AimTarget.Instance.LockTarget.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax);
        _HPProcess.value = AimTarget.Instance.LockTarget.RoleAttrManager.HPPersent;
    }

    #endregion
}
