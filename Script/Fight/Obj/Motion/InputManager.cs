using UnityEngine;
using System.Collections;

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
        _Axis.x = Input.GetAxis("Horizontal");
        _Axis.y = Input.GetAxis("Vertical");
        if (_Axis != Vector2.zero)
        {
            if (_InputMotion.BaseMotionManager.CanMotionMove())
                _InputMotion.BaseMotionManager.MoveDirect(_Axis);
        }
        else
        {
            if(_InputMotion.BaseMotionManager.CanMotionIdle())
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

    public Vector2 _Axis;

    public bool IsKeyDown(string key)
    {
        if (key.Length != 1)
            return false;
        return Input.GetKeyDown(key);
    }

    public bool IsKeyHold(string key)
    {
        return Input.GetKey(key);
    }

    #endregion

    #region normal skill

    private ObjMotionSkillAttack _NormalAttack;
    public void CharSkill()
    {
        if (_NormalAttack == null)
            return;

        if (IsKeyDown("k"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "k" + (_NormalAttack.CurStep + 1);
                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey]);
                }
            }
            else if(_InputMotion.ActingSkill == null)
            {
                string inputKey = "k0";
                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey]);
                }
            }
        }

        if (IsKeyDown("u"))
        {
            if (_InputMotion.ActingSkill == _NormalAttack && _NormalAttack.CurStep > 0 && _NormalAttack.CurStep < 4 && _NormalAttack.CanNextInput)
            {
                string inputKey = "u" + (_NormalAttack.CurStep + 1);
                if (_InputMotion._SkillMotions.ContainsKey(inputKey))
                {
                    _InputMotion.ActSkill(_InputMotion._SkillMotions[inputKey]);
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

    #endregion
}
