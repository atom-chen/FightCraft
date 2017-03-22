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

    void Start()
    {
        StartCoroutine(InitDelay());
    }

    void FixedUpdate()
    {
        if (!_Init)
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
    }

    protected virtual void AIUpdate()
    {

    }

}
