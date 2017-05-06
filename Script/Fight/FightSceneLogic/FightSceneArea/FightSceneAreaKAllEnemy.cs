using UnityEngine;
using System.Collections;

public class FightSceneAreaKAllEnemy : FightSceneAreaBase
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

    public SerializeEnemyInfo[] _EnemyBornPos;
    private int _DeadEnemyCnt;

    private void StartStep()
    {
        foreach (var enemyInfo in _EnemyBornPos)
        {
            FightManager.Instance.InitEnemy(enemyInfo._EnemyDataID, enemyInfo._EnemyTransform.position, enemyInfo._EnemyTransform.rotation.eulerAngles);
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
