using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
using UnityEngine.EventSystems;
using System;

public class UISkillBarItem : UIItemSelect
{
    public UIPressBtn _BtnPress;
    public Image _SkillIcon;
    public Image _CDImage;
    public Image _UseTipImage;
    public string _SkillInput;

    public bool IsSkillAct
    {
        get; set;
    }

    void Update()
    {
        if (_UseTipImage.gameObject.activeInHierarchy)
        {
            _UseTipImage.fillAmount -= _Step * Time.deltaTime;
        }
    }

    public void InitSkillIcon()
    {
        _CDImage.fillAmount = 0;
        _UseTipImage.fillAmount = 0;
    }

    public void BtnSkill()
    {

    }

    #region use tips

    private float _Step;

    public void SetUseTips(float second)
    {
        if (second > 0)
        {
            _UseTipImage.gameObject.SetActive(true);
            _Step = 1 / second;
            _UseTipImage.fillAmount = 1;
        }
        else
        {
            _UseTipImage.gameObject.SetActive(false);
        }
    }

    #endregion

    #region cd

    public void SetCD(float cdProcess)
    {
        if (cdProcess > 0 && cdProcess < 1)
        {
            _CDImage.gameObject.SetActive(true);
            _CDImage.fillAmount = cdProcess;
        }
        else
        {
            _CDImage.gameObject.SetActive(false);
        }
    }

    #endregion
}

