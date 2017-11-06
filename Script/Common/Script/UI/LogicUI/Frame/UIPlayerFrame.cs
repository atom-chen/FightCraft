using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerFrame : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Frame/UIPlayerFrame", UILayer.BaseUI, hash);
    }

    #endregion

    void Update()
    {
        HpUpdate();
    }

    #region 

    public Slider _HPProcess;
    public Text _HPText;

    private void HpUpdate()
    {
        if (!FightManager.Instance)
            return;

        if (!FightManager.Instance.MainChatMotion)
            return;

        _HPText.text = FightManager.Instance.MainChatMotion.RoleAttrManager.HP + "/" + FightManager.Instance.MainChatMotion.RoleAttrManager.GetBaseAttr(RoleAttrEnum.HPMax);
        _HPProcess.value = FightManager.Instance.MainChatMotion.RoleAttrManager.HPPersent;
    }

    #endregion
}
