using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameUI;

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
        if (Axis != Vector2.zero)
        {
            if (_InputMotion.BaseMotionManager.CanMotionMove())
            {
                _InputMotion.BaseMotionManager.MoveDirect(CameraAxis);
            }
        }
        else
#if UNITY_EDITOR
        if (!_EmulateMode)
#endif
        {

            if (_InputMotion.BaseMotionManager.IsMoving())
                _InputMotion.BaseMotionManager.StopMove();
            if (_InputMotion.BaseMotionManager.CanMotionIdle())
                _InputMotion.BaseMotionManager.MotionIdle();
        }

        //if (_InputMotion.ActingSkill == null)
        {
            foreach (var skill in _InputMotion._SkillMotions)
            {
                if (IsKeyDown(skill.Key))
                {
                    _InputMotion.ActSkill(skill.Value);
                }
            }
        }
        CharSkill();
    }

    public MotionManager _InputMotion;

    #region input 
    private Vector2 _Axis;
    public Vector2 Axis
    {
        get
        {
            return _Axis;
        }

        set
        {
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
            _InputMotion.ActSkill(_InputMotion._SkillMotions["j"]);
        }

        if (IsKeyHold("k"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "k" + (_NormalAttack.CurStep + 1);
                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    AutoRotate();
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey]);
                }
            }
            else if(_InputMotion.ActingSkill == null)
            {
                string inputKey = "k0";
                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    AutoRotate();
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey]);
                }
            }
        }

        if (IsKeyHold("u"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "u1"/* + (_NormalAttack.CurStep + 1)*/;
                Hashtable hash = new Hashtable();
                hash.Add("AttackStep", _NormalAttack.CurStep);

                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey], hash);
                }
            }
            else if (_InputMotion.ActingSkill == null)
            {
                string inputKey = "u0";
                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey]);
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

#endregion
}
