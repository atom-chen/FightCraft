using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 

public class InputManager : InstanceBase<InputManager>
{

	// Use this for initialization
	void Start ()
    {
        SetInstance(this);

        if (_NormalAttack == null)
        {
            _NormalAttack = _InputMotion.GetComponentInChildren<ObjMotionSkillAttack>();
        }
        InitReuseSkill();
    }

    void OnDestory()
    {
        SetInstance(null);
    }
	
	// Update is called once per frame
	void Update ()
    {
#if UNITY_EDITOR
        Axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif

#if UNITY_EDITOR
        if (!_EmulateMode)
#endif
        {
            _InputMotion.InputDirect(CameraAxis);
        }

        //if (_InputMotion.ActingSkill == null)
        {
            foreach (var skill in _InputMotion._StateSkill._SkillMotions)
            {
                if (IsKeyHold(skill.Key))
                {
                    //_InputMotion.ActSkill(skill.Value);
                    _InputMotion.InputSkill(skill.Key);
                }
            }
        }
        CharSkill();
    }

    public MotionManager _InputMotion;

    #region input 
    private Vector2 _LastAxis;
    private Vector2 _Axis;
    public Vector2 Axis
    {
        get
        {
            return _Axis;
        }

        set
        {
            _LastAxis = _Axis;
            _Axis = value;
        }
    }

    public Vector2 CameraAxis
    {
        get
        {
            var direct = transform.forward * Axis.y + transform.right * Axis.x;
            return new Vector2(direct.x, direct.z);
        }
    }

    private UISkillBar _UISkillBar;
    public UISkillBar UISkillBar
    {
        get
        {
            if (_UISkillBar == null)
            {
                _UISkillBar = GameObject.FindObjectOfType<UISkillBar>();
            }
            return _UISkillBar;
        }
    }

    public bool IsKeyDown(string key)
    {

        if (key.Length != 1)
            return false;
#if UNITY_EDITOR
        if (_EmulateMode)
        {
            return IsEmulateKeyDown(key);
        }
        return Input.GetKeyDown(key);
#else
        return UISkillBar.IsKeyDown(key);
#endif
    }

    public bool IsKeyHold(string key)
    {
        string realKey = key;
        if (key.Contains("k"))
        {
            realKey = "k";
        }
        if (key.Contains("u"))
        {
            realKey = "u";
        }
#if UNITY_EDITOR
        if (_EmulateMode)
        {
            return IsEmulateKeyDown(key);
        }
        return Input.GetKey(realKey);
#else
        return UISkillBar.IsKeyDown(key);
#endif
    }

    public bool IsAnyHold()
    {
#if UNITY_EDITOR
        return Input.anyKey;
#else
        return UISkillBar.IsKeyDown();
#endif
    }

    #endregion

    #region emulate key

    public bool _EmulateMode = false;
    private string _EmulatePress = "";

    public void SetEmulatePress(string key)
    {
        _EmulateMode = true;
        _EmulatePress = key;
    }

    public void ReleasePress()
    {
        _EmulatePress = "";
    }

    public void FinishEmulateMode()
    {
        _EmulateMode = false;
    }

    public bool IsEmulateKeyDown(string key)
    {
        return _EmulatePress == key;
    }

    #endregion

    #region normal skill

    private ObjMotionSkillAttack _NormalAttack;
    public void CharSkill()
    {
        if (_NormalAttack == null)
            return;

        if (IsKeyHold("j"))
        {
            _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions["j"]);
        }

        if (IsKeyHold("k"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = (_NormalAttack.CurStep).ToString();
                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    AutoRotate();
                    _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
            else if (_InputMotion.ActingSkill == null)
            {
                if (CanReuseSkill())
                {
                    _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[_ReuseSkillInput]);
                    return;
                }
                
                {
                    string inputKey = "k";
                    if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                    {
                        AutoRotate();
                        _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[inputKey]);
                    }
                }
            }
        }

        if (IsKeyHold("u"))
        {
            if (_InputMotion.ActingSkill== _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "6";
                Hashtable hash = new Hashtable();
                hash.Add("AttackStep", _NormalAttack.CurStep);

                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[inputKey], hash);
                }
            }
            else if (_InputMotion.ActingSkill== null)
            {
                string inputKey = "5";
                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
        }

        if (IsKeyHold("l"))
        {
            string inputKey = "4";
            if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
            {
                _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[inputKey]);
            }
            else
            {
                inputKey = "7";
                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
        }
    }

    public void AutoRotate()
    {
        if (AimTarget.Instance == null)
            return;

        if (AimTarget.Instance.LockTarget != null)
        {
            _InputMotion.SetLookAt(AimTarget.Instance.LockTarget.transform.position);
        }
    }

    public ObjMotionSkillBase GetCharSkill(string input)
    {
        if (_NormalAttack == null)
            return null;

        if (input == ("j"))
        {
            return _InputMotion._StateSkill._SkillMotions["j"];
        }

        if (input == ("k"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = (_NormalAttack.CurStep).ToString();
                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return _InputMotion._StateSkill._SkillMotions[inputKey];
                }
            }
            else if (_InputMotion.ActingSkill == null)
            {
                if (CanReuseSkill())
                {
                    return _InputMotion._StateSkill._SkillMotions[_ReuseSkillInput];
                }

                {
                    string inputKey = "k0";
                    if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                    {
                        return  (_InputMotion._StateSkill._SkillMotions[inputKey]);
                    }
                }
            }
        }

        if (input == ("u"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "6";
                Hashtable hash = new Hashtable();
                hash.Add("AttackStep", _NormalAttack.CurStep);

                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return  (_InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
            else if (_InputMotion.ActingSkill == null)
            {
                string inputKey = "5";
                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return  (_InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
        }

        if (input == ("l"))
        {
            string inputKey = "4";
            if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
            {
                return  (_InputMotion._StateSkill._SkillMotions[inputKey]);
            }
            else
            {
                inputKey = "7";
                if (_InputMotion._StateSkill._SkillMotions.ContainsKey(inputKey))
                {
                    return  (_InputMotion._StateSkill._SkillMotions[inputKey]);
                }
            }
        }

        return null;
    }

    #endregion

    #region use skill again

    private string _ReuseSkillInput;
    private int _ReuseTimes = 0;
    private float _ReuseStartTime = 0;
    private float _ReuseLast = 1;

    public void InitReuseSkill()
    {
        foreach (var skillInfo in RoleData.SelectRole.ProfessionSkills)
        {
            if (skillInfo.SkillRecord.SkillAttr == "RoleAttrImpactAnotherUse")
            {
                _ReuseSkillInput = skillInfo.SkillRecord.SkillInput;
                break;
            }
        }
    }

    public void SkillFinish(ObjMotionSkillBase motionSkill)
    {
        if (_ReuseTimes > 0)
        {
            if (motionSkill._ActInput == "j"
                || motionSkill._ActInput == "1"
                || motionSkill._ActInput == "2"
                || motionSkill._ActInput == "3")
            {
                _ReuseTimes = 0;
                UISkillBar.SetSkillUseTips("k", 0);
                return;
            }
        }
        if (_InputMotion == FightManager.Instance.MainChatMotion)
        {
            if (motionSkill._ActInput == _ReuseSkillInput)
            {
                _ReuseTimes = 1;
                _ReuseStartTime = Time.time;
                UISkillBar.SetSkillUseTips("k", _ReuseTimes);
            }
        }
    }

    private bool CanReuseSkill()
    {
        if (_ReuseTimes > 0 && Time.time - _ReuseStartTime < _ReuseLast)
        {
            return true;
        }
        return false;
    }

    #endregion
}
