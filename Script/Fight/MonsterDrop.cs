using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameLogic;

public class DropItemData
{
    public ItemEquip _ItemEquip;
    public ItemBase _ItemBase;
    public int _DropGold;
    public int _DropDiamond;

    public Vector3 _DropPos;
    public Vector3 _MonsterPos;
    public bool _IsAutoPick;
}

public class MonsterDrop
{

    public static void MonsterDripItems(MotionManager monsterMotion)
    {
        Debug.Log("MonsterDripItems:" + monsterMotion.name);

        var drops = GetMonsterDrops(monsterMotion);
        var randomPoses = GameBase.GameRandom.GetIndependentRandoms(0, 16, drops.Count);
        int posIdx = 0;
        foreach (var drop in drops)
        {
            var pos = GetDropPos(monsterMotion, randomPoses[posIdx]);
            ++posIdx;
            drop._DropPos = pos;
            drop._MonsterPos = monsterMotion.transform.position;
            var obj = GameBase.ResourceManager.Instance.GetInstanceGameObject("Drop/DropItem");
            DropItem dropItem = obj.GetComponent<DropItem>();
            dropItem.InitDrop(drop);
        }

    }

    private static List<DropItemData> GetMonsterDrops(MotionManager monsterMotion)
    {
        List<DropItemData> dropList = new List<DropItemData>();

        for (int i = 0; i < 1; ++i)
        {
            if (monsterMotion.RoleAttrManager.MotionType == MotionType.Normal)
            {
                DropItemData drop = new DropItemData();
                drop._DropGold = 100;
                dropList.Add(drop);
            }
            else if (monsterMotion.RoleAttrManager.MotionType == MotionType.Elite)
            {
                DropItemData drop = new DropItemData();
                drop._DropGold = 300;
                dropList.Add(drop);
            }
            else if (monsterMotion.RoleAttrManager.MotionType == MotionType.Hero)
            {
                DropItemData drop = new DropItemData();
                drop._DropGold = 1000;
                dropList.Add(drop);
            }
        }
        return dropList;
    }

    private static Vector3 GetDropPos(MotionManager monsterMotion, int posIdx)
    {
        int rangeParam = posIdx / 8;
        int angleParam = posIdx % 8;

        float range = (rangeParam + 1) * 1;
        float angle = angleParam * 45;

        Vector3 pos = new Vector3(0, monsterMotion.transform.position.y, 0);
        pos.x = monsterMotion.transform.position.x + Mathf.Sin(angle) * range;
        pos.z = monsterMotion.transform.position.z + Mathf.Cos(angle) * range;

        NavMeshHit navMeshHit;
        if (NavMesh.SamplePosition(pos, out navMeshHit, range, NavMesh.AllAreas))
        {
            return navMeshHit.position;
        }
        return pos;
    }

    public static void PickItem(DropItem dropItem)
    {
        Debug.Log("PickItem");
        GameBase.ResourceManager.Instance.DestoryObj(dropItem.gameObject);
    }
}
