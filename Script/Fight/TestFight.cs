using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFight : MonoBehaviour
{	
	// Update is called once per frame
	void Update ()
    {
        //if (_EnemyMotion == null)
        {
            if (!FindEnemy())
            {
                FindNextArea();
            }
        }

        //if (_EnemyMotion != null)
        {
            CloseUpdate();
        }
	}

    void Start()
    {
        InputManager.Instance._EmulateMode = true;
        _NormalAttack = gameObject.GetComponentInChildren<ObjMotionSkillAttack>();
    }

    #region find target

    private MotionManager _EnemyMotion;
    private Vector3 _NextAreaPos;

    private bool FindEnemy()
    {
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            float distance = Vector3.Distance(transform.position, _EnemyMotion.transform.position);
            if (distance < 2.0f)
                return true;
        }

        _EnemyMotion = null;
        var motions = GameObject.FindObjectsOfType<MotionManager>();
        float tarDistance = 10;
        foreach (var motion in motions)
        {
            if (motion.RoleAttrManager.MotionType != MotionType.MainChar && !motion.IsMotionDie)
            {
                float distance = Vector3.Distance(transform.position, motion.transform.position);
                if (distance < tarDistance)
                {
                    _EnemyMotion = motion;
                    tarDistance = distance;
                }
            }
        }

        return _EnemyMotion != null;
    }

    private bool FindNextArea()
    {
        var fightManager = GameObject.FindObjectOfType<FightSceneLogicPassArea>();
        _NextAreaPos = fightManager.GetNextAreaPos();
        if (_NextAreaPos == Vector3.zero)
        {
            return false;
        }
        return true;
    }

    #endregion

    #region control

    private float _CloseRange = 2.0f;
    private int _RandomSkillIdx = 0;
    private ObjMotionSkillAttack _NormalAttack;

    private void CloseUpdate()
    {

        //if (FightManager.Instance.MainChatMotion.ActingSkill != null)
        //{
        //    StartSkill();
        //    return;
        //}

        var destPos = Vector3.zero;
        if (_NextAreaPos != Vector3.zero)
        {
            destPos = _NextAreaPos;
        }
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            destPos = _EnemyMotion.transform.position;
        }
        if (destPos == Vector3.zero)
            return;

        float distance = Vector3.Distance(transform.position, destPos);
        if (distance > _CloseRange)
        {
            FightManager.Instance.MainChatMotion.BaseMotionManager.MoveTarget(destPos);
            ReleaseSkill();
        }
        else
        {
            if (FightManager.Instance.MainChatMotion.BaseMotionManager.IsMoving())
            {
                FightManager.Instance.MainChatMotion.BaseMotionManager.StopMove();
            }
            StartSkill();
        }
    }

    private bool StartSkill()
    {
        if (FightManager.Instance.MainChatMotion.ActingSkill == null)
        {
            _RandomSkillIdx = Random.Range(0, 4);
            //FightManager.Instance.MainChatMotion.ActSkill(FightManager.Instance.MainChatMotion._SkillMotions["j"]);
            transform.LookAt(_EnemyMotion.transform.position);
            InputManager.Instance.SetEmulatePress("j");
        }

        else if (FightManager.Instance.MainChatMotion.ActingSkill == _NormalAttack)
        {
            if (_NormalAttack.CurStep > 0 && _NormalAttack.CurStep == _RandomSkillIdx)
            {
                //if (_NormalAttack.CanNextInput)
                {
                    InputManager.Instance.SetEmulatePress("k");
                    Debug.Log("emulate key k");
                    _RandomSkillIdx = -1;
                }
            }
        }
        return true;
    }

    private void ReleaseSkill()
    {
        InputManager.Instance.ReleasePress();
    }
    #endregion
}
