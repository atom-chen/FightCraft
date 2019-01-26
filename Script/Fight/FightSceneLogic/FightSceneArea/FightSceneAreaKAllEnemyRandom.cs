using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKAllEnemyRandom : FightSceneAreaKAllEnemy
{

    #region enemy step

    public int _EliteCnt = 1;
    public List<string> _EliteRandomIDs = new List<string>()
    {"21","23","25","27","33","37","48","50","31","39", "43", "44", "45"};
    public List<string> _NormaRandomIDs = new List<string>()
    {"21","23","25","27","33","37","48","50","31","39"};

    private string _RandomNormalId = "";
    protected override void StartStep()
    {
        for(int i = 0; i< _EnemyBornPos.Length; ++i)
        {
            string enemyDataID = "";
            bool isElite = false;
            if (i < _EliteCnt)
            {
                int randomIdx = Random.Range(0, _EliteRandomIDs.Count);
                enemyDataID = _EliteRandomIDs[randomIdx];
                isElite = true;
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

            MotionManager enemy = FightManager.Instance.InitEnemy(enemyDataID, _EnemyBornPos[i]._EnemyTransform.position, _EnemyBornPos[i]._EnemyTransform.rotation.eulerAngles, isElite);
            
            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            _EnemyAI.Add(enemyAI);
            if (_IsEnemyAlert)
            {
                enemyAI._TargetMotion = FightManager.Instance.MainChatMotion;
                enemyAI.AIWake = true;
            }
        }
    }
    
    #endregion
    
}
