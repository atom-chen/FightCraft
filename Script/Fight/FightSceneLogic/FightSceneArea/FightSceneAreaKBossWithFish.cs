using UnityEngine;
using System.Collections;

public class FightSceneAreaKBossWithFish : FightSceneAreaBase
{

    public override void StartArea()
    {
        base.StartArea();

        StartStep();
    }

    protected override void UpdateArea()
    {
        base.UpdateArea();

        UpdateFish();
    }

    public override void MotionDie(MotionManager motion)
    {
        Debug.Log("MotionDie motion " + motion.name);

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    #region enemy step

    public Transform _BossBornPos;
    public string _BossMotionID;
    public Transform[] _EnemyBornPos;
    public SerializeRandomEnemy[] _EnemyMotionID;
    public float _BossStepEnemyInterval = 10;
    public int _FightingEnemyCnt = 2;

    private int _RateTotle = -1;
    private int _LivingEnemyCnt = 0;
    private float _LastUpdateFishTime = 0;

    private void StartStep()
    {
        FightManager.Instance.InitEnemy(_BossMotionID, _BossBornPos.position, _BossBornPos.rotation.eulerAngles);
        for (int i = 0; i < _FightingEnemyCnt; ++i)
        {
            CreateEngoughEnemy();
            _LastUpdateFishTime = Time.time;
        }
    }

    private void UpdateFish()
    {
        if (Time.time - _LastUpdateFishTime > _BossStepEnemyInterval)
        {
            if (_LivingEnemyCnt < _FightingEnemyCnt)
            {
                CreateEngoughEnemy();
            }
        }
    }

    private void StepMotionDie(MotionManager motion)
    {
        if (motion.MonsterBase.Id == _BossMotionID)
        {
            FinishArea();
        }
        --_LivingEnemyCnt;
    }

    private void CreateEngoughEnemy()
    {
        int randomPos = Random.Range(0, _EnemyBornPos.Length);
        var enemyMotion = FightManager.Instance.InitEnemy(GetRandomEnmeyID(), _EnemyBornPos[randomPos].position, _EnemyBornPos[randomPos].rotation.eulerAngles);
        AI_CloseAttack ai = enemyMotion.GetComponent<AI_CloseAttack>();
        if (ai != null)
        {
            ai._AlertRange = 1000;
        }
        ++_LivingEnemyCnt;
    }

    private string GetRandomEnmeyID()
    {
        if (_RateTotle < 0)
        {
            _RateTotle = 0;
            foreach (var random in _EnemyMotionID)
            {
                _RateTotle += random._Rate;
            }
        }

        int randomValue = Random.Range(0, _RateTotle);
        int totleRate = 0;
        foreach (var random in _EnemyMotionID)
        {
            if (random._Rate + totleRate >= randomValue)
            {
                return random._EnemyDataID;
            }
            totleRate += random._Rate;
        }
        return _EnemyMotionID[_EnemyMotionID.Length - 1]._EnemyDataID;
    }
    #endregion
}
