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

        _AreaStarted = true;
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
        if (RandomLogic.IsChangeToBoss)
        {
            monIds.Add(RandomLogic.BossID);
        }
        else
        {
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
        }


        //var stageDiffInfo = GetStageDiffInfo(ActData.Instance.GetNormalDiff(), monIds.Count);
        int diff = ActData.Instance._NormalStageIdx;
        var stageDiffInfo = GetStageDiffInfo(ActData.Instance._NormalStageIdx, monIds.Count);

        for (int i = 0; i < monIds.Count; ++i)
        {
            Tables.MOTION_TYPE motionType = Tables.MOTION_TYPE.Normal;
            string monId = monIds[i].ToString();
            if (stageDiffInfo.EliteIdxs.Contains(i))
            {
                motionType = Tables.MOTION_TYPE.Elite;
            }
            else if (stageDiffInfo.ExIdxs.Contains(i))
            {
                motionType = Tables.MOTION_TYPE.ExElite;
            }
            else if (stageDiffInfo.ExtraMonIdxs.Contains(i))
            {
                motionType = Tables.MOTION_TYPE.Normal;
                monId = stageDiffInfo.ExtraMonID.ToString();
            }

            var rot = Quaternion.LookRotation(_MonLookTrans, Vector3.up);
            MotionManager enemy = FightManager.Instance.InitEnemy(monIds[i].ToString(), _MonPoses[i], rot.eulerAngles, motionType);

            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            enemyAI.GroupID = AreaID;
            if (motionType != Tables.MOTION_TYPE.Normal)
            {
                var bossAI = enemyAI as AI_HeroBase;
                if (bossAI != null)
                {
                    if (RandomLogic.IsChangeToBoss)
                    {
                        InitBossAILevel(diff, bossAI);
                    }
                    else if (stageDiffInfo.RandomBuffCnt > 0)
                    {
                        var passiveBuff = ResourceManager.Instance.GetInstanceGameObject("SkillMotion/CommonImpact/EliteRandomBuff");
                        var randomBuff = passiveBuff.GetComponent<ImpactBuffRandomSub>();
                        randomBuff._ActSubCnt = stageDiffInfo.RandomBuffCnt;
                        bossAI._PassiveGO = randomBuff.transform;
                    }
                }
                //var heroAi = enemyAI as AI_HeroBase;
                //if (heroAi != null)
                //{
                //    heroAi._IsRiseBoom = true;
                //}
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

    #region stage diff

    public class StageDiffInfo
    {
        public List<int> EliteIdxs = new List<int>();
        public List<int> ExIdxs = new List<int>();
        public List<int> ExtraMonIdxs = new List<int>();
        public int ExtraMonID = -1;
        public int RandomBuffCnt = 0;
    }

    public class StageDiffBossFishInfo
    {
        public int FishCnt = 0;
        public int ElitCnt = 0;
        public int ExCnt = 0;
        public int QiLinCnt = 0;
        public int ExQiLinCnt = 0;

        public int MaxFishCnt = 4;
        public int NormalBornCD = 5;
        public int EliteDieCD = 15;
    }

    public static StageDiffInfo GetStageDiffInfo(int diff, int monCnt)
    {
        StageDiffInfo stageInfo = new StageDiffInfo();
        switch (diff)
        {
            case 0:
                break;
            case 1:
                if (Random.Range(0, 1) < 0.25)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 2:
                if (Random.Range(0, 1) < 0.5)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 3:
                if (Random.Range(0, 1) < 0.35)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 4:
                if (Random.Range(0, 1) < 0.5)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 5:
                if (Random.Range(0, 1) < 0.5)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    if (Random.Range(0, 1) < 0.5)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLin.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLin[qilinIdx];
                    }
                }
                break;
            case 6:
                if (Random.Range(0, 1) < 0.35)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    float bossRate = Random.Range(0, 1);
                    if (bossRate < 0.35f)
                    {
                        int bossIdx = Random.Range(0, FightSceneLogicRandomArea._BossType.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._BossType[bossIdx];
                    }
                    else if(bossRate < 0.4f)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLin.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLin[qilinIdx];
                    }
                }
                break;
            case 7:
                if (Random.Range(0, 1) < 0.5)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    float bossRate = Random.Range(0, 1);
                    if (bossRate < 0.35f)
                    {
                        int bossIdx = Random.Range(0, FightSceneLogicRandomArea._BossType.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._BossType[bossIdx];
                    }
                    else if (bossRate < 0.5f)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLin.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLin[qilinIdx];
                    }
                }
                break;
            case 8:
                if (Random.Range(0, 1) < 0.6)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    float bossRate = Random.Range(0, 1);
                    if (bossRate < 0.4f)
                    {
                        int bossIdx = Random.Range(0, FightSceneLogicRandomArea._BossType.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._BossType[bossIdx];
                    }
                    else if (bossRate < 0.7f)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLinEx.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLinEx[qilinIdx];
                    }
                }
                break;
            case 9:
                if (Random.Range(0, 1) < 0.6)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    float bossRate = Random.Range(0, 1);
                    if (bossRate < 0.4f)
                    {
                        int bossIdx = Random.Range(0, FightSceneLogicRandomArea._BossType.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._BossType[bossIdx];
                    }
                    else if (bossRate < 0.7f)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLinEx.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLinEx[qilinIdx];
                    }
                }
                stageInfo.RandomBuffCnt = 1;
                break;
            case 10:
                if (Random.Range(0, 1) < 0.6)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    float bossRate = Random.Range(0, 1);
                    if (bossRate < 0.4f)
                    {
                        int bossIdx = Random.Range(0, FightSceneLogicRandomArea._BossType.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._BossType[bossIdx];
                    }
                    else if (bossRate < 0.7f)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLinEx.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLinEx[qilinIdx];
                    }
                }
                stageInfo.RandomBuffCnt = 2;
                break;
            default:
                if (Random.Range(0, 1) < 0.6)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    float bossRate = Random.Range(0, 1);
                    if (bossRate < 0.4f)
                    {
                        int bossIdx = Random.Range(0, FightSceneLogicRandomArea._BossType.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._BossType[bossIdx];
                    }
                    else if (bossRate < 0.7f)
                    {
                        int qilinIdx = Random.Range(0, FightSceneLogicRandomArea._QiLinEx.Count);
                        stageInfo.ExtraMonID = FightSceneLogicRandomArea._QiLinEx[qilinIdx];
                    }
                }
                stageInfo.RandomBuffCnt = 2;
                break;
        }

        return stageInfo;
    }

    public static bool IsDiffBossElite(int diff)
    {
        if (diff >= 5)
            return true;
        return false;
    }

    public void InitBossAILevel(int diff, AI_HeroBase aiBoss)
    {
        if (aiBoss == null)
            return;
        switch (diff)
        {
            case 0:
                aiBoss._IsRiseBoom = false;
                break;
            case 1:
                aiBoss._IsRiseBoom = true;
                break;
            case 2:
                aiBoss.InitProtectTimes(1);
                break;
            case 3:
                aiBoss.InitProtectTimes(1);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 4:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 5:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 6:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                aiBoss.IsCancelNormalAttack = true;
                break;
            case 7:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(2f);
                aiBoss._StageBuffHpPersent.Add(0.6f);
                aiBoss.IsCancelNormalAttack = true;
                break;
            default:
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(2f);
                aiBoss._StageBuffHpPersent.Add(0.6f);
                aiBoss.IsCancelNormalAttack = true;
                break;
        }
    }

    public static StageDiffBossFishInfo GetStageDiffBossFishInfo(int diff)
    {
        StageDiffBossFishInfo fishInfo = new StageDiffBossFishInfo();
        switch (diff)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                break;
            case 8:
                fishInfo.FishCnt = 4;
                break;
            case 9:
                fishInfo.FishCnt = 4;
                fishInfo.ElitCnt = 1;
                break;
            case 10:
                fishInfo.FishCnt = 4;
                fishInfo.ElitCnt = 2;
                break;
            case 11:
                fishInfo.FishCnt = 4;
                fishInfo.ExCnt = 1;
                break;
            case 12:
                fishInfo.FishCnt = 4;
                fishInfo.ExCnt = 2;
                break;
            case 13:
                fishInfo.FishCnt = -1;
                break;
            case 14:
                fishInfo.FishCnt = -1;
                fishInfo.ElitCnt = 1;
                break;
            case 15:
                fishInfo.FishCnt = -1;
                fishInfo.ExCnt = 1;
                break;
            case 16:
                fishInfo.FishCnt = -1;
                fishInfo.ExCnt = 1;
                fishInfo.QiLinCnt = 1;
                break;
            case 17:
                fishInfo.FishCnt = -1;
                fishInfo.ExCnt = 1;
                fishInfo.ExQiLinCnt = 1;
                break;
            default:
                fishInfo.FishCnt = -1;
                fishInfo.ExCnt = 1;
                fishInfo.ExQiLinCnt = 1;
                break;
        }

        return fishInfo;
    }

    #region boss fish



    #endregion

    #endregion
}
