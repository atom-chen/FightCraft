using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneLogicSimple : FightSceneLogicBase
{
    public Transform[] _EnemyBornPos;
    public string _EnemyMotionName;
    public Transform _BossBornPos;
    public string _BossMotionName;
    public int _KillEnemyCnt = 20;
    public int _FightingEnemyCnt = 2;

    private int _DeadEnemyCnt = 0;
    private int _InitPosIdx = 0;

    public override void StartLogic()
    {
        base.StartLogic();

        _DeadEnemyCnt = 0;
        _InitPosIdx = 0;

        CreateEngoughEnemy();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();


    }

    private void UpdateNoBoss()
    {

    }

    private void CreateEngoughEnemy()
    {
        if (_DeadEnemyCnt >= _KillEnemyCnt)
        {
            return;
        }

        int createCnt = _KillEnemyCnt - _DeadEnemyCnt;
        createCnt = createCnt > _FightingEnemyCnt ? _FightingEnemyCnt : createCnt;

        for (int i = 0; i < createCnt; ++i)
        {
            if (_InitPosIdx >= _EnemyBornPos.Length)
            {
                _InitPosIdx = 0;
            }

            FightManager.Instance.InitEnemy(_EnemyMotionName, _EnemyBornPos[_InitPosIdx].position, _EnemyBornPos[_InitPosIdx].rotation.eulerAngles);
            ++_InitPosIdx;
        }
    }
}
