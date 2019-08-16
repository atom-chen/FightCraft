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

        //if (!_IsEnemyAlert)
        //{
        //    foreach (var ai in _EnemyAI)
        //    {
        //        if (ai == null)
        //        {
        //            continue;
        //        }

        //        if (Vector3.Distance(ai.transform.position, FightManager.Instance.MainChatMotion.transform.position) < _EnemyAlertDistance)
        //        {
        //            _IsEnemyAlert = true;
        //            SetAllAlert();
        //        }
        //    }
        //}

        UpdateFish();
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
        if (IsBossArea())
        {
            if (motion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Hero)
            {
                ClearFish();
            }
            else
            {
                FishDie(motion);
            }
        }
        //else
        //{
        //    var ai = motion.GetComponent<AI_Base>();
        //    if (_EnemyAI.Contains(ai))
        //    {
        //        _EnemyAI.Remove(ai);
        //    }

        //    if (_EnemyAI.Count == 0)
        //    {
        //        FinishArea();
        //    }
        //}
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
    

    public bool IsBossArea()
    {
        return _MonPoses.Count == 1;
    }

    public void InitRandomMonsters()
    {
        InitMonTrans();
        InitMonPos();

        int maticTotalCnt = GameRandom.GetRandomLevel(7, 3) + 1;
        int monTotalCnt = GameRandom.GetRandomLevel(5, 3, 2) + 7 - maticTotalCnt;
        if (IsBossArea())
        {
            maticTotalCnt = 0;
            monTotalCnt = 1;
        }
        else
        {
            monTotalCnt = Mathf.Min(_MonPoses.Count - maticTotalCnt, monTotalCnt);
        }

        List<int> monIds = new List<int>();
        _BossFishInfo = null;
        int diff = ActData.Instance.GetNormalDiff();
        if (IsBossArea())
        {
            monIds.Add(RandomLogic.BossID);
            _BossFishInfo = GetStageDiffBossFishInfo(diff);
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

        var stageDiffInfo = GetStageDiffInfo(diff, monIds.Count, RandomLogic);

        for (int i = 0; i < monIds.Count; ++i)
        {
            Tables.MOTION_TYPE motionType = Tables.MOTION_TYPE.Normal;
            string monId = monIds[i].ToString();
            if (IsBossArea())
            {
                motionType = Tables.MOTION_TYPE.Hero;
            }
            else
            {
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
                    motionType = Tables.MOTION_TYPE.ExElite;
                    monId = stageDiffInfo.ExtraMonID.ToString();
                }
            }

            var rot = Quaternion.LookRotation(_MonLookTrans, Vector3.up);
            MotionManager enemy = FightManager.Instance.InitEnemy(monId, _MonPoses[i], rot.eulerAngles, motionType);

            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            enemyAI.GroupID = AreaID;
            if (motionType != Tables.MOTION_TYPE.Normal)
            {
                var bossAI = enemyAI as AI_HeroBase;
                if (bossAI != null)
                {
                    if (IsBossArea())
                    {
                        InitBossAILevel(diff, bossAI);
                    }
                    else if (stageDiffInfo.RandomBuffCnt > 0)
                    {
                        var randomBuff = ResourcePool.Instance.GetConfig<Transform>(ResourcePool.ConfigEnum.RandomBuff);
                        randomBuff.GetComponentInChildren<ImpactBuffRandomSub>()._ActSubCnt = stageDiffInfo.RandomBuffCnt;
                        bossAI._PassiveGO = randomBuff;
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
        if (RandomLogic.StartBossArea)
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

    #region update fish

    public List<Transform> _BossFishPoses;

    private StageDiffBossFishInfo _BossFishInfo;
    private bool _InitFish = false;
    public enum MonsterEliteType
    {
        None,
        Normal,
        Elite,
        Ex,
        QiLin,
        ExQiLin,
    }
    private float _CreateMonTimeCD = 0;
    List<MotionManager> _FishMotion = new List<MotionManager>();
    MotionManager _FishEliteMotion;

    private void UpdateFish()
    {
        if (_BossFishInfo == null)
            return;

        if (_BossFishInfo.FishCnt == 0)
            return;

        if (!_InitFish)
        {
            List<Vector3> initPos = new List<Vector3>();
            initPos.Add(transform.position + new Vector3(1.5f, 0, 1.5f));
            initPos.Add(transform.position + new Vector3(1.5f, 0, -1.5f));
            initPos.Add(transform.position + new Vector3(-1.5f, 0, 1.5f));
            initPos.Add(transform.position + new Vector3(-1.5f, 0, -1.5f));

            _FishMotion.Clear();
            var rot = Quaternion.LookRotation(_MonLookTrans, Vector3.up);
            for (int i = 0; i < initPos.Count; ++i)
            {
                string randomFish = GetRandomFish().ToString();
                MonsterEliteType eliteType = GetMonsterType(i + 1);
                Tables.MOTION_TYPE motionType = Tables.MOTION_TYPE.Normal;
                if (eliteType == MonsterEliteType.Elite)
                {
                    motionType = Tables.MOTION_TYPE.Elite;
                }
                else if (eliteType == MonsterEliteType.Ex)
                {
                    motionType = Tables.MOTION_TYPE.ExElite;
                }
                else if (eliteType == MonsterEliteType.QiLin)
                {
                    randomFish = FightSceneLogicRandomArea._QiLin[Random.Range(0, FightSceneLogicRandomArea._QiLin.Count)].ToString();
                }
                else if (eliteType == MonsterEliteType.ExQiLin)
                {
                    randomFish = FightSceneLogicRandomArea._QiLin[Random.Range(0, FightSceneLogicRandomArea._QiLinEx.Count)].ToString();
                }

                MotionManager enemy = FightManager.Instance.InitEnemy(randomFish, initPos[i], rot.eulerAngles, motionType);
                _FishMotion.Add(enemy);
                if (eliteType == MonsterEliteType.Normal)
                {
                    _CreateMonTimeCD = Time.time + _BossFishInfo.NormalBornCD;
                }
                else
                {
                    _CreateMonTimeCD = Time.time + _BossFishInfo.EliteDieCD;
                    _FishEliteMotion = enemy;
                }
            }
            _InitFish = true;
        }

        if (_FishMotion.Count < _BossFishInfo.MaxFishCnt && _CreateMonTimeCD <= Time.time)
        {
            string randomFish = GetRandomFish().ToString();
            MonsterEliteType eliteType = GetMonsterType(-1);
            if (eliteType == MonsterEliteType.None)
                return;

            Tables.MOTION_TYPE motionType = Tables.MOTION_TYPE.Normal;
            if (eliteType == MonsterEliteType.Elite)
            {
                motionType = Tables.MOTION_TYPE.Elite;
            }
            else if (eliteType == MonsterEliteType.Ex)
            {
                motionType = Tables.MOTION_TYPE.ExElite;
            }
            else if (eliteType == MonsterEliteType.QiLin)
            {
                motionType = Tables.MOTION_TYPE.Elite;
                randomFish = FightSceneLogicRandomArea._QiLin[Random.Range(0, FightSceneLogicRandomArea._QiLin.Count)].ToString();
            }
            else if (eliteType == MonsterEliteType.ExQiLin)
            {
                motionType = Tables.MOTION_TYPE.ExElite;
                randomFish = FightSceneLogicRandomArea._QiLin[Random.Range(0, FightSceneLogicRandomArea._QiLinEx.Count)].ToString();
            }

            var rot = Quaternion.LookRotation(_MonLookTrans, Vector3.up);
            MotionManager enemy = FightManager.Instance.InitEnemy(randomFish, GetFarPos(), rot.eulerAngles, motionType);
            _FishMotion.Add(enemy);
            if (eliteType == MonsterEliteType.Normal)
            {
                _CreateMonTimeCD = Time.time + _BossFishInfo.NormalBornCD;
            }
            else
            {
                _CreateMonTimeCD = Time.time + _BossFishInfo.EliteDieCD;
                _FishEliteMotion = enemy;
            }
        }
    }

    private Vector3 GetFarPos()
    {
        List<Vector3> farPoses = new List<Vector3>();
        foreach (var fishTrans in _BossFishPoses)
        {
            if (Vector3.Distance(FightManager.Instance.MainChatMotion.transform.position, fishTrans.position) > _EnemyAlertDistance)
            {
                farPoses.Add(fishTrans.position);
            }
        }

        return farPoses[Random.Range(0, farPoses.Count)];
    }

    private MonsterEliteType GetMonsterType(int initIdx)
    {
        if (initIdx > 0 && _BossFishInfo.ElitRate >= initIdx)
            return MonsterEliteType.Elite;

        if (initIdx > 0 && _BossFishInfo.ExRate >= initIdx)
            return MonsterEliteType.Ex;

        if (_FishEliteMotion != null)
            return MonsterEliteType.Normal;

        MonsterEliteType eliteType = MonsterEliteType.None;
        float randomRate = Random.Range(0, 1.0f);
        if (randomRate < _BossFishInfo.ElitRate)
        {
            eliteType = MonsterEliteType.Elite;
        }
        else if (randomRate < _BossFishInfo.ElitRate + _BossFishInfo.ExRate)
        {
            eliteType = MonsterEliteType.Ex;
        }
        else if (randomRate < _BossFishInfo.ElitRate + _BossFishInfo.ExRate + _BossFishInfo.QiLinRate)
        {
            eliteType = MonsterEliteType.QiLin;
        }
        else if (randomRate < _BossFishInfo.ElitRate + _BossFishInfo.ExRate + _BossFishInfo.QiLinRate + _BossFishInfo.ExQiLinRate)
        {
            eliteType = MonsterEliteType.ExQiLin;
        }
        else
        {
            eliteType = MonsterEliteType.Normal;
        }

        return eliteType;
    }

    private int GetRandomFish()
    {
        if (GameRandom.IsInRate(2000))
        {
            return RandomLogic.MagicMonster;
        }
        else
        {
            return RandomLogic.NormalMonster[Random.Range(0, RandomLogic.NormalMonster.Count)];
        }
    }

    private void FishDie(MotionManager dieMotion)
    {
        if (_BossFishInfo == null)
            return;

        if (_FishMotion.Contains(dieMotion))
            _FishMotion.Remove(dieMotion);

        if (_FishEliteMotion == dieMotion)
            _FishEliteMotion = null;
    }

    private void ClearFish()
    {
        foreach (var fishMotion in _FishMotion)
        {
            fishMotion.MotionDie();
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
        public float ElitRate = 0;
        public float ExRate = 0;
        public float QiLinRate = 0;
        public float ExQiLinRate = 0;

        public int MaxFishCnt = 4;
        public int NormalBornCD = 5;
        public int EliteDieCD = 15;
    }

    public static StageDiffInfo GetStageDiffInfo(int diff, int monCnt, FightSceneLogicRandomArea randomArea)
    {
        StageDiffInfo stageInfo = new StageDiffInfo();
        int eliteRate = Random.Range(0, 10000);
        switch (diff)
        {
            case 0:
                break;
            case 1:
                if (eliteRate < 1500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 2:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 3:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if(eliteRate < 4000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }

                break;
            case 4:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 5000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                break;
            case 5:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 5000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 6500)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }

                break;
            case 6:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 5000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 7500)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }
                break;
            case 7:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 5000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 7500)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }
                break;
            case 8:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 6000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 8500)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }
                break;
            case 9:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 6000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 8500)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }
                stageInfo.RandomBuffCnt = 1;
                break;
            case 10:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 6000)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 8500)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }
                stageInfo.RandomBuffCnt = 2;
                break;
            default:
                if (eliteRate < 2500)
                {
                    stageInfo.EliteIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 7200)
                {
                    stageInfo.ExIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                }
                else if (eliteRate < 10000)
                {
                    stageInfo.ExtraMonIdxs = GameRandom.GetIndependentRandoms(0, monCnt, 1);
                    stageInfo.ExtraMonID = randomArea.QiLin;
                }
                stageInfo.RandomBuffCnt = 2;
                break;
        }

        return stageInfo;
    }

    public static bool IsExtraHero(int diff)
    {
        return false;
    }

    public static bool IsQiLin(int diff)
    {
        if (diff >= 5)
            return true;
        return false;
    }

    public static bool IsExQiLin(int diff)
    {
        if (diff >= 7)
            return true;
        return false;
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
                aiBoss._IsContainsNormalAtk = false;
                aiBoss._IsRiseBoom = false;
                break;
            case 1:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                break;
            case 2:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                aiBoss.InitProtectTimes(1);
                break;
            case 3:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                aiBoss.InitProtectTimes(1);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 4:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 5:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                break;
            case 6:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(0.5f);
                aiBoss.IsCancelNormalAttack = true;
                break;
            case 7:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
                aiBoss.InitProtectTimes(2);
                aiBoss._StageBuffHpPersent.Add(2f);
                aiBoss._StageBuffHpPersent.Add(0.6f);
                aiBoss.IsCancelNormalAttack = true;
                break;
            default:
                aiBoss._IsContainsNormalAtk = true;
                aiBoss._IsRiseBoom = true;
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
                fishInfo.ElitRate = 1;
                break;
            case 10:
                fishInfo.FishCnt = 4;
                fishInfo.ElitRate = 2;
                break;
            case 11:
                fishInfo.FishCnt = 4;
                fishInfo.ExRate = 1;
                break;
            case 12:
                fishInfo.FishCnt = 4;
                fishInfo.ExRate = 2;
                break;
            case 13:
                fishInfo.FishCnt = -1;
                break;
            case 14:
                fishInfo.FishCnt = -1;
                fishInfo.ElitRate = 0.5f;
                break;
            case 15:
                fishInfo.FishCnt = -1;
                fishInfo.ElitRate = 0.4f;
                fishInfo.ExRate = 0.25f;
                break;
            case 16:
                fishInfo.FishCnt = -1;
                fishInfo.ElitRate = 0.3f;
                fishInfo.ExRate = 0.25f;
                fishInfo.QiLinRate = 0.15f;
                break;
            case 17:
                fishInfo.FishCnt = -1;
                fishInfo.ElitRate = 0.2f;
                fishInfo.ExRate = 0.2f;
                fishInfo.QiLinRate = 0.2f;
                fishInfo.ExQiLinRate = 0.2f;
                break;
            default:
                fishInfo.FishCnt = -1;
                fishInfo.ElitRate = 0.25f;
                fishInfo.ExRate = 0.25f;
                fishInfo.QiLinRate = 0.25f;
                fishInfo.ExQiLinRate = 0.25f;
                break;
        }

        return fishInfo;
    }

    #region boss fish



    #endregion

    #endregion

    #region 

    public float _DistanceToPlayer;

    #endregion
}
