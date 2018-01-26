using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKAllEnemy : FightSceneAreaBase
{
    public override void StartArea()
    {
        base.StartArea();

        StartStep();
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
        return _EnemyBornPos[0]._EnemyTransform;
    }

    #region enemy step

    public SerializeEnemyInfo[] _EnemyBornPos;
    private int _DeadEnemyCnt;
    private List<AI_Base> _EnemyAI = new List<AI_Base>();

    private void StartStep()
    {
        foreach (var enemyInfo in _EnemyBornPos)
        {
            var enemy = FightManager.Instance.InitEnemy(enemyInfo._EnemyDataID, enemyInfo._EnemyTransform.position, enemyInfo._EnemyTransform.rotation.eulerAngles);

            var enemyAI = enemy.gameObject.GetComponent<AI_Base>();
            _EnemyAI.Add(enemyAI);
            if (_IsEnemyAlert)
            {
                enemyAI._TargetMotion = FightManager.Instance.MainChatMotion;
            }
        }
    }

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

        if (_DeadEnemyCnt >= _EnemyBornPos.Length)
        {
            FinishArea();
        }
    }

    #endregion
}
