using UnityEngine;
using System.Collections;

public class AI_CloseFight : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        _SelfMotion = GetComponent<MotionManager>();
        _MotionMove = GetComponentInChildren<ObjMotionMove>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        UpdateMove();
    }

    #region ai

    public MotionManager _Enemy;
    public ObjMotionSkillBase _AttackMotion;
    public float _AttackRange;
    public float AttackCD;

    private MotionManager _SelfMotion;
    private ObjMotionMove _MotionMove;
    private float _AttackCD;

    public void UpdateMove()
    {
        Vector3 direct = _Enemy.transform.position - _SelfMotion.transform.position;
        if (direct.magnitude < _AttackRange)
        {
            _MotionMove.MotionFinish();
            Attack();
        }
        else if(_MotionMove.IsCanActiveMotion())
        {
            transform.rotation = Quaternion.LookRotation(direct);
            _MotionMove.MoveDirect(direct);
        }
    }

    public void Attack()
    {
        if (_AttackCD > 0)
        {
            _AttackCD -= Time.fixedDeltaTime;
            return;
        }

        _AttackCD = AttackCD;
        _AttackMotion.ActSkill();
    }

    #endregion
}
