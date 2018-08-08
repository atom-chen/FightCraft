using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaRandom : FightSceneAreaBase
{
    public override void StartArea()
    {
        base.StartArea();
    }

    public override void InitArea()
    {
        base.InitArea();

        InitRandomMonsters();
    }

    public override void UpdateArea()
    {
        base.UpdateArea();

        if (!_IsEnemyAlert)
        {
            foreach (var ai in _EnemyAI)
            {
                if (ai == null)
                {
                    continue;
                }

                if (Vector3.Distance(ai.transform.position, FightManager.Instance.MainChatMotion.transform.position) < _EnemyAlertDistance)
                {
                    _IsEnemyAlert = true;
                    SetAllAlert();
                }
            }
        }
    }

    public override void MotionDie(MotionManager motion)
    {
        //Debug.Log("MotionDie motion " + motion.name);

        base.MotionDie(motion);

        StepMotionDie(motion);

    }

    public override Transform GetAreaTransform()
    {
        return transform;
    }

    #region enemy step

    private int _DeadEnemyCnt;
    private List<AI_Base> _EnemyAI = new List<AI_Base>();
    
    private void SetAllAlert()
    {
        foreach (var ai in _EnemyAI)
        {
            if (ai == null)
            {
                continue;
            }

            ai._TargetMotion = FightManager.Instance.MainChatMotion;
        }
    }

    private void StepMotionDie(MotionManager motion)
    {

        ++_DeadEnemyCnt;

        if (_DeadEnemyCnt >= _MonPoses.Count)
        {
            FinishArea();
        }
    }

    public void ClearAllEnemy()
    {
        foreach (var ai in _EnemyAI)
        {
            if (ai == null)
            {
                continue;
            }

            FightManager.Instance.ObjDisapear(ai._SelfMotion);
        }
    }

    #endregion

   
    #region random monster

    public static float _MonDistance = 2.2f;

    public List<Transform> _DirectTrans;
    public FightSceneLogicRandomArea RandomLogic { get; set; }
    public int AreaID { get; set; }

    private Vector3 _MonTrans;
    private Vector3 _MonLookTrans;
    private List<Vector3> _MonPoses = new List<Vector3>();

    public void InitRandomMonsters()
    {
        InitMonTrans();
        InitMonPos();

        int maticTotalCnt = GameRandom.GetRandomLevel(7, 3) + 1;
        int monTotalCnt = GameRandom.GetRandomLevel(5, 3, 2) + 7 - maticTotalCnt;
        if (_MonPoses.Count == 1)
        {
            maticTotalCnt = 0;
            monTotalCnt = 1;
        }
        else
        {
            monTotalCnt = Mathf.Min(_MonPoses.Count - maticTotalCnt, monTotalCnt);
        }
        List<int> monIds = new List<int>();
        for (int i = 0; i < RandomLogic.NormalMonster.Count; ++i)
        {
            if (i == RandomLogic.NormalMonster.Count - 1)
            {
                for (int j = monIds.Count; j < monTotalCnt; ++j)
                {
                    monIds.Add(RandomLogic.NormalMonster[RandomLogic.NormalMonster.Count - 1]);
                }
            }
            else
            {
                int randomCnt = Random.Range(0, monTotalCnt - monIds.Count + 1);
                for (int j = 0; j < randomCnt; ++j)
                {
                    monIds.Add(RandomLogic.NormalMonster[i]);
                }
            }
        }
        for (int i = 0; i < maticTotalCnt; ++i)
        {
            monIds.Add(RandomLogic.MagicMonster);
        }

        int eliteIdx = -1;
        if (ActData.Instance._ProcessStageDiff > 1)
        {
            var eliteRate = FightManager.Instance.GetEliteMonsterRate();
            var eliteRandom = Random.Range(0, GameDataValue.GetMaxRate());

            if (eliteRandom < eliteRate)
            {
                eliteIdx = Random.Range(0, monIds.Count);
            }
        }
        
        for (int i = 0; i < monIds.Count; ++i)
        {
            bool isElite = false;
            if (eliteIdx == i)
            {
                isElite = true;
            }

            var rot = Quaternion.LookRotation(_MonLookTrans, Vector3.up);
            MotionManager enemy = FightManager.Instance.InitEnemy(monIds[i].ToString(), _MonPoses[i], rot.eulerAngles, isElite);

            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            enemyAI.GroupID = AreaID;
            if (enemy.MonsterBase.MotionType == Tables.MOTION_TYPE.Hero)
            {
                var heroAi = enemyAI as AI_HeroBase;
                if (heroAi != null)
                {
                    heroAi._IsRiseBoom = true;
                }
            }
            _EnemyAI.Add(enemyAI);
        }
    }

    private void InitMonTrans()
    {
        if (_DirectTrans.Count == 0)
        {
            _MonTrans = Vector3.zero;
            _MonLookTrans = Vector3.zero;
        }
        else if (_DirectTrans.Count <= RandomLogic.MainChatPosIdx)
        {
            _MonTrans = _DirectTrans[0].localPosition;
            _MonLookTrans = _DirectTrans[0].position - transform.position;
        }
        else
        {
            _MonTrans = _DirectTrans[RandomLogic.MainChatPosIdx].localPosition;
            _MonLookTrans = _DirectTrans[RandomLogic.MainChatPosIdx].position - transform.position;
        }
    }

    private void InitMonPos()
    {
        _MonPoses.Clear();
        if (RandomLogic.IsChangeToBoss)
        {
            _MonPoses.Add(transform.position);
        }
        else
        {
            var scale = transform.localScale;
            if (scale.x > scale.z)
            {
                Vector3 basePos = new Vector3(_MonDistance * 1.5f, 0, _MonDistance * 0.5f);
                ModifyBaseDirect(ref basePos);
                Vector3 slotPos = new Vector3(_MonDistance, 0, _MonDistance);
                ModifySlotDirect(ref slotPos);
                for (int i = 0; i < 2; ++i)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        Vector3 pos = new Vector3(slotPos.x * j, 0, slotPos.z * i);
                        pos = basePos + pos + transform.position;
                        _MonPoses.Add(pos);
                    }
                }
            }
            else if (scale.x < scale.z)
            {
                Vector3 basePos = new Vector3(_MonDistance * 0.5f, 0, _MonDistance * 1.5f);
                ModifyBaseDirect(ref basePos);
                Vector3 slotPos = new Vector3(_MonDistance, 0, _MonDistance);
                ModifySlotDirect(ref slotPos);
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 2; ++j)
                    {
                        Vector3 pos = new Vector3(slotPos.x * j, 0, slotPos.z * i);
                        pos = basePos + pos + transform.position;
                        _MonPoses.Add(pos);
                    }
                }
            }
            else
            {
                Vector3 basePos = new Vector3(_MonDistance * 1f, 0, _MonDistance * 1f);
                ModifyBaseDirect(ref basePos);
                Vector3 slotPos = new Vector3(_MonDistance, 0, _MonDistance);
                ModifySlotDirect(ref slotPos);
                for (int i = 0; i < 3; ++i)
                {
                    for (int j = 0; j < 3; ++j)
                    {
                        Vector3 pos = new Vector3(slotPos.x * j, 0, slotPos.z * i);
                        pos = basePos + pos + transform.position;
                        _MonPoses.Add(pos);
                    }
                }
            }
        }
    }

    private void ModifyBaseDirect(ref Vector3 basePos)
    {
        if (_MonTrans.x > 0)
        {

        }
        else if (_MonTrans.x < 0)
        {
            basePos.x = -basePos.x;
            basePos.z = -basePos.z;
        }
        else if (_MonTrans.z > 0)
        {
            basePos.x = -basePos.x;
        }
        else if (_MonTrans.z < 0)
        {
            basePos.z = -basePos.z;
        }
    }

    private void ModifySlotDirect(ref Vector3 basePos)
    {
        if (_MonTrans.x > 0)
        {
            basePos.x = -basePos.x;
            basePos.z = -basePos.z;
        }
        else if (_MonTrans.x < 0)
        {
            
        }
        else if (_MonTrans.z > 0)
        {
            basePos.z = -basePos.z;
        }
        else if (_MonTrans.z < 0)
        {
            basePos.x = -basePos.x;
        }
    }

    #endregion
}
