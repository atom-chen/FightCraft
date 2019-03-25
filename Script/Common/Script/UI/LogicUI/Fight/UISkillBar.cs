using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 
public class UISkillBar : UIBase
{
    #region static

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UISkillBar", UILayer.BaseUI, hash);
    }

    public static void SetSkillUseTips(string input, float time)
    {
        if (GameCore.Instance == null)
            return;

        var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillBar>("LogicUI/Fight/UISkillBar");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetBtnUseTip(input, time);
    }

    public static void SetAimTypeStatic(AimTarget.AimTargetType aimType)
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISkillBar>("LogicUI/Fight/UISkillBar");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.SetAimType(aimType);
    }

    public void Update()
    {
        UpdateCD();
    }

    #endregion

    public UISkillBarItem _ButtonJ;
    public UISkillBarItem _ButtonK;
    public UISkillBarItem _ButtonL;
    public UISkillBarItem _ButtonU;

    private Dictionary<string, UISkillBarItem> _Buttons = new Dictionary<string, UISkillBarItem>();

    public override void Init()
    {
        base.Init();

        _Buttons.Add("j", _ButtonJ);
        _Buttons.Add("k", _ButtonK);
        _Buttons.Add("l", _ButtonL);
        _Buttons.Add("u", _ButtonU);
    }

    public bool IsKeyDown(string key)
    {
        if (_Buttons.ContainsKey(key))
        {
            return _Buttons[key]._BtnPress.IsPress;
        }
        return false;
    }

    public bool IsKeyDown()
    {
        foreach (var btn in _Buttons)
        {
            if (btn.Value._BtnPress.IsPress)
                return true;
        }
        return false;
    }

    public void SetBtnUseTip(string key, float useTipTime)
    {
        if (_Buttons.ContainsKey(key))
        {
            _Buttons[key].SetUseTips(useTipTime);
        }
    }

    #region cd
    List<string> skillInput = new List<string>() { "j", "k", "l" };

    public void UpdateCD()
    {
        if (InputManager.Instance == null)
            return;

        for (int i = 0; i < skillInput.Count; ++i)
        {
            var skillBase = InputManager.Instance.GetCharSkill(skillInput[i]);
            if (skillBase == null)
            {
                _Buttons[skillInput[i]].SetCDPro(0);
                continue;
            }

            if (skillBase._SkillCD == 0)
            {
                _Buttons[skillInput[i]].SetCDPro(0);
                continue;
            }

            var cd = Time.time - skillBase.LastUseTime;
            float cdPro = (skillBase._SkillCD - cd) / skillBase._SkillCD;
            _Buttons[skillInput[i]].SetCDPro(cdPro);


            _Buttons[skillInput[i]].SetStoreTimes(skillBase.LastUseTimes);
        }
    }

    #endregion

    #region emulate

    public TestFight _TestFight;

    public void OnEmulate()
    {
        if (_TestFight == null)
        {
            _TestFight = FightManager.Instance.MainChatMotion.gameObject.GetComponent<TestFight>();
            if (_TestFight == null)
                FightManager.Instance.MainChatMotion.gameObject.AddComponent<TestFight>();
            return;
        }

        _TestFight.enabled = !_TestFight.enabled;
    }

    #endregion

    #region summon

    public UISkillBarItem _SummonBtn;
    
    public void OnBtnSummon()
    {
        SummonSkill.Instance.UseSummonSkill();    
    }

    #endregion

    #region switch aim

    public Text _AimText;

    private int _AimType = 0;
    public void OnSwitchAimType()
    {
        //++_AimType;
        //if (_AimType >= 3)
        //    _AimType = 0;

        //AimTarget.Instance.SwitchAimType(_AimType);
        //UpdateAim();
        AimTarget.Instance.SwitchAimTarget();
    }

    public void SetAimType(AimTarget.AimTargetType aimType)
    {
        _AimType = (int)aimType;
        UpdateAim();
    }

    private void UpdateAim()
    {
        switch ((AimTarget.AimTargetType)_AimType)
        {
            case AimTarget.AimTargetType.None:
                _AimText.text = "N";
                break;
            case AimTarget.AimTargetType.Free:
                _AimText.text = "F";
                break;
            case AimTarget.AimTargetType.Lock:
                _AimText.text = "L";
                break;
        }
    }

    #endregion
}

