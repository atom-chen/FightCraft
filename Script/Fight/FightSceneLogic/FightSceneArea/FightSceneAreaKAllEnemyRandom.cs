using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKAllEnemyRandom : FightSceneAreaKAllEnemy
{

    #region enemy step

    public int _EliteCnt = 1;
    public List<string> _EliteRandomIDs = new List<string>()
    {"21","23","25","27","33","37","48","50","31","39", "43", "44", "45"};
    public int _ExEliteCnt = 0;
    public bool _IsSameEx = true;
    public List<string> _ExEliteRandomIDs = new List<string>()
    {"201","202","203","204","205","205","206","207","208","209", "210", "211"};
    public List<string> _NormaRandomIDs = new List<string>()
    {"21","23","25","27","33","37","48","50","31","39"};
    public int _RandomBuffEliteCnt = 0;
    public GameObject _RandomBuffPassiveGO;

    private string _RandomNormalId = "";
    private string _RandomEliteID = "";
    private string _RandomEliteExID = "";
    protected override void StartStep()
    {
        for(int i = 0; i< _EnemyBornPos.Length; ++i)
        {
            string enemyDataID = "";
            if (i < _ExEliteCnt)
            {
                if (_IsSameEx && string.IsNullOrEmpty(_RandomEliteExID))
                {
                    int randomIdx = Random.Range(0, _ExEliteRandomIDs.Count);
                    enemyDataID = _ExEliteRandomIDs[randomIdx];
                    _RandomEliteExID = enemyDataID;
                }
                else if (!_IsSameEx)
                {
                    int randomIdx = Random.Range(0, _ExEliteRandomIDs.Count);
                    enemyDataID = _ExEliteRandomIDs[randomIdx];
                    _RandomEliteExID = enemyDataID;
                }
            }
            else if (i < _EliteCnt + _ExEliteCnt)
            {
                int randomIdx = Random.Range(0, _EliteRandomIDs.Count);
                enemyDataID = _EliteRandomIDs[randomIdx];
                _RandomEliteID = enemyDataID;
            }
            else if (string.IsNullOrEmpty(_RandomNormalId))
            {
                int randomIdx = Random.Range(0, _NormaRandomIDs.Count);
                _RandomNormalId = _NormaRandomIDs[randomIdx];
                enemyDataID = _RandomNormalId;
            }
            else
            {
                enemyDataID = _RandomNormalId;
            }

            Debug.Log("Init Random Mon:" + enemyDataID);
            MotionManager enemy = FightManager.Instance.InitEnemy(enemyDataID, _EnemyBornPos[i]._EnemyTransform.position, _EnemyBornPos[i]._EnemyTransform.rotation.eulerAngles, false);
            
            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            _EnemyAI.Add(enemyAI);
            if (_IsEnemyAlert)
            {
                enemyAI._TargetMotion = FightManager.Instance.MainChatMotion;
                enemyAI.AIWake = true;
            }

            if (_RandomBuffEliteCnt > i && _RandomBuffPassiveGO != null)
            {
                var eliteAI = enemyAI as AI_HeroBase;
                eliteAI._PassiveGO = _RandomBuffPassiveGO.transform;
            }
        }
    }
    
    #endregion
    
}
