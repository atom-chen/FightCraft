using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjMotionSkillBuff : ObjMotionSkillBase
{

    void Update()
    {
        if (_CanNextInput && _IsCanActAfterBuff)
        {
            if (InputManager.Instance.IsKeyHold("k") || InputManager.Instance.IsKeyHold(_ActSkillInput))
            {
                _SkillProcess = 1.1f;
                InputManager.Instance.SetRotate();
                MotionManager.FinishSkill(this);
                MotionManager.ActSkill(MotionManager._StateSkill._SkillMotions[_ActSkillInput]);
            }
        }

        _SkillProcess += Time.deltaTime;
        MotionManager._SkillProcessing = _SkillProcess / GetTotalAnimLength();
    }

    private bool _IsCanActAfterBuff = true;
    public bool IsCanActAfterBuff
    {
        get
        {
            return _IsCanActAfterBuff;
        }
        set
        {
            _IsCanActAfterBuff = value;
        }
    }
    private float _SkillProcess = 0;

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

    public override bool IsCanActSkill()
    {
        return base.IsCanActSkill();
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
            _ActSkillInput = _AttackStep.ToString();
        }
        _SkillProcess = 0;
        return true;
    }
}
