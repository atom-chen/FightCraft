using UnityEngine;
using System.Collections;

public class AI_Base : MonoBehaviour
{

    public class AI_Skill_Prior
    {
        public ObjMotionSkillBase SkillBase;
        public float SkillRange;
        public float SkillInterval;
    }

    bool _Init = false;

    protected MotionManager _SelfMotion;

    void Start()
    {
        
        //var navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        //if (navMeshAgent != null)
        //{
        //    navMeshAgent.enabled = false;
        //}
        StartCoroutine(InitDelay());
    }

    void FixedUpdate()
    {
        if (!_Init)
            return;

        if (_SelfMotion == null || _SelfMotion.IsMotionDie)
            return;

        AIUpdate();
    }

    private IEnumerator InitDelay()
    {
        yield return new WaitForFixedUpdate();
        Init();
    }

    protected virtual void Init()
    {
        _Init = true;
        _SelfMotion = GetComponent<MotionManager>();
    }

    protected virtual void AIUpdate()
    {

    }

}
