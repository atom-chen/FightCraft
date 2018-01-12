using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class FightSceneAreaKBossWithFish : FightSceneAreaBase
{

    public override void StartArea()
    {
        base.StartArea();

        UIFightWarning.ShowBossAsyn();

        StartStep();
    }

    public override void UpdateArea()
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

    public override Transform GetAreaTransform()
    {
        return _BossBornPos;
    }

    #region enemy step

    public Transform _BossBornPos;
    public string _BossMotionID;
    public bool _FishBornFixPos = true;
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
                _LastUpdateFishTime = Time.time;
            }
        }
    }

    private void StepMotionDie(MotionManager motion)
    {
        if (motion.MonsterBase == null)
            return;

        if (motion.MonsterBase.Id == _BossMotionID)
        {
            FinishArea();
        }
        
        --_LivingEnemyCnt;
    }

    private void CreateEngoughEnemy()
    {
        if (_EnemyMotionID.Length == 0)
            return;

        MotionManager enemyMotion;
        if (_FishBornFixPos)
        {
            int randomPos = Random.Range(0, _EnemyBornPos.Length);
            enemyMotion = FightManager.Instance.InitEnemy(GetRandomEnmeyID(), _EnemyBornPos[randomPos].position, _EnemyBornPos[randomPos].rotation.eulerAngles);
        }
        else
        {
            enemyMotion = FightManager.Instance.InitEnemy(GetRandomEnmeyID(), GetFishRandomPos(), Vector3.zero);
        }
        AI_CloseAttack ai = enemyMotion.GetComponent<AI_CloseAttack>();
        if (ai != null)
        {
            ai._AlertRange = 1000;
        }
        ++_LivingEnemyCnt;
    }

    private static List<Vector3> _FishRandomPosDelta = new List<Vector3>()
    {
        new Vector3(-10,0,0),
        new Vector3(-7.5f,0,7.5f),
        new Vector3(0,0,10),
        new Vector3(7.5f,0,7.5f),
        new Vector3(10,0,0),
        new Vector3(7.5f,0,-7.5f),
        new Vector3(0,0,-10),
        new Vector3(-7.5f,0,-7.5f),
    };
    private Vector3 GetFishRandomPos()
    {
        int randomIdx = Random.Range(0, _FishRandomPosDelta.Count);
        var randomPos = _FishRandomPosDelta[randomIdx] + FightManager.Instance.MainChatMotion.transform.position;
        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(randomPos, out navMeshHit, 100, -1))
        {
            return navMeshHit.position;
        }
        return _BossBornPos.position;

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
