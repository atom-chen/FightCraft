using UnityEngine;
using System.Collections;

public class FightSceneAreaKEnemyCnt : FightSceneAreaBase
{

    public override void StartArea()
    {
        base.StartArea();

        StartStep();
    }

    protected override void UpdateArea()
    {
        base.UpdateArea();
    }

    public override void MotionDie(MotionManager motion)
    {
        Debug.Log("MotionDie motion " + motion.name);

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    #region enemy step

    public Transform[] _EnemyBornPos;
    public string _EnemyMotionID;
    public int _KillEnemyCnt = 20;
    public int _FightingEnemyCnt = 6;

    private int _DeadEnemyCnt = 0;
    private int _InitPosIdx = 0;
    private int _CurEnemyCnt = 0;

    private void StartStep()
    {
        _DeadEnemyCnt = 0;
        _InitPosIdx = 0;
        _CurEnemyCnt = 0;
        for (int i = 0; i < _FightingEnemyCnt; ++i)
        {
            CreateEngoughEnemy();
        }

    }

    private void StepMotionDie(MotionManager motion)
    {

        ++_DeadEnemyCnt;
        --_CurEnemyCnt;
        CreateEngoughEnemy();

        if (_DeadEnemyCnt >= _KillEnemyCnt)
        {
            FinishArea();
        }
    }

    private void CreateEngoughEnemy()
    {
        if (_DeadEnemyCnt >= _KillEnemyCnt)
        {
            return;
        }

        if (_InitPosIdx >= _EnemyBornPos.Length)
        {
            _InitPosIdx = 0;
        }

        FightManager.Instance.InitEnemy(_EnemyMotionID, _EnemyBornPos[_InitPosIdx].position, _EnemyBornPos[_InitPosIdx].rotation.eulerAngles);
        ++_InitPosIdx;
        ++_CurEnemyCnt;
    }

    #endregion
}
