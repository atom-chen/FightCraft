using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBuff : ObjMotionSkillBase
{
    void Update()
    {
        if (!_CanNextInput)
            return;

        if (InputManager.Instance.IsKeyHold("k"))
        {
            if (_MotionManager._StateSkill._SkillMotions.ContainsKey(_ActSkillInput))
            {
                if (_MotionManager._StateSkill._SkillMotions[_ActSkillInput].CanSkillActAfterDebuff())
                {
                    _MotionManager.ActSkill(_MotionManager._StateSkill._SkillMotions[_ActSkillInput]);
                }
            }
        }

    }

    private int _AttackStep = 0;
    private string _ActSkillInput = "";
    private bool _CanNextInput = false;

    public override void AnimEvent(string function, object param)
    {
        base.AnimEvent(function, param);

        switch (function)
        {
            case AnimEventManager.NEXT_INPUT_START:
                _CanNextInput = true;
                break;
            case AnimEventManager.NEXT_INPUT_END:
                _CanNextInput = false;
                break;
        }
    }

    public override bool ActSkill(Hashtable exHash = null)
    {
        base.ActSkill(exHash);

        _AttackStep = 0;
        _ActSkillInput = "";
        _CanNextInput = false;
        if (exHash != null && exHash.ContainsKey("AttackStep"))
        {
            _AttackStep = (int)exHash["AttackStep"];
            _ActSkillInput = "k" + (_AttackStep + 1);
        }
        return true;
    }
}
