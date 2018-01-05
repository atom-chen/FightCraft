using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

 
 

    public enum BtnType
    {
        None,
        OKBTN,
        YESNOBTN
    }

public class UIMessageBox : UIPopBase
{
    #region 

    public static void Show(int dictIdx, Action okAction, Action cancelAction, BtnType btnType = BtnType.YESNOBTN)
    {
        Show(Tables.StrDictionary.GetFormatStr(dictIdx), okAction, cancelAction, btnType);
    }

    public static void Show(string message, Action okAction, Action cancelAction, BtnType btnType = BtnType.YESNOBTN)
    {
        Hashtable hash = new Hashtable();
        hash.Add("Message", message);
        hash.Add("OkAction", (Action)okAction);
        hash.Add("BtnType", btnType);
        //GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_UI_SHOW_MESSAGEBOX, null, hash);
        GameCore.Instance.UIManager.ShowUI("SystemUI/UIMessageBox", UILayer.MessageUI, hash);
    }

    #endregion

    #region 

    public Text _MsgText;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _OkAction = null;
        if (hash.ContainsKey("OkAction"))
        {
            _OkAction = (Action)hash["OkAction"];
        }

        _CancelAction = null;
        if (hash.ContainsKey("CancelAction"))
        {
            _CancelAction = (Action)hash["CancelAction"];
        }

        if (hash.ContainsKey("Message"))
        {
            _MsgText.text = (string)hash["Message"];
        }
        if (hash.ContainsKey("BtnType"))
        {
            HideBtn((BtnType)hash["BtnType"]);
        }

        transform.SetAsLastSibling();
    }

    public void HideBtn(BtnType btnType)
    {
        if (m_btnOK != null)
        {
            m_btnOK.SetActive(btnType == BtnType.OKBTN);
        }
        if (m_btnYesNo != null)
        {
            m_btnYesNo.SetActive(btnType == BtnType.YESNOBTN);
        }
    }

    #endregion

    #region 

    private Action _OkAction;
    private Action _CancelAction;

    public GameObject m_btnYesNo;
    public GameObject m_btnOK;
    public void BtnOkEvent()
    {
        if (_OkAction != null)
            _OkAction();

        Hide();
    }

    public void BtnCancelEvent()
    {
        if (_CancelAction != null)
            _CancelAction();

        Hide();
    }


    #endregion
}

