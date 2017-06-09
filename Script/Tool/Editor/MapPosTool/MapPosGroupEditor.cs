
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace NavMeshExtension
{
    /// <summary>
    /// Custom Editor for editing vertices and exporting the mesh.
    /// </summary>
    [CustomEditor(typeof(MapPosGroup))]
    public class MapPosGroupEditor : Editor
    {
        //navmesh object reference
        private MapPosGroup script;

        private bool placing;


        void OnEnable()
        {
            script = (MapPosGroup)target;
        }


        /// <summary>
        /// Custom inspector override for buttons.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            if (GUILayout.Button("FitEnemies"))
            {
                var enemyPoses = script.GetComponentsInChildren<MapPosObj>();

                var fightKAll = script.GetComponent<FightSceneAreaKAllEnemy>();
                if (fightKAll != null)
                {
                    fightKAll._EnemyBornPos = new SerializeEnemyInfo[enemyPoses.Length];
                    for (int i = 0; i < enemyPoses.Length; ++i)
                    {
                        fightKAll._EnemyBornPos[i] = new SerializeEnemyInfo();
                        fightKAll._EnemyBornPos[i]._EnemyTransform = enemyPoses[i].transform;
                        fightKAll._EnemyBornPos[i]._EnemyDataID = enemyPoses[i]._MonsterId;
                        enemyPoses[i].ShowMonsterByID();
                    }
                }

                var fightKCnt = script.GetComponent<FightSceneAreaKEnemyCnt>();
                if (fightKCnt != null)
                {
                    fightKCnt._EnemyBornPos = new Transform[enemyPoses.Length];
                    for (int i = 0; i < enemyPoses.Length; ++i)
                    {
                        fightKCnt._EnemyBornPos[i] = enemyPoses[i].transform;
                        //fightKCnt._EnemyMotionID = enemyPoses[i]._MonsterId;
                        enemyPoses[i].ShowMonsterByID();
                    }
                }

                var fightBossCnt = script.GetComponent<FightSceneAreaKBossWithFish>();
                if (fightBossCnt != null)
                {
                    fightBossCnt._BossBornPos = enemyPoses[0].transform;
                    fightBossCnt._BossMotionID = enemyPoses[0]._MonsterId;
                    enemyPoses[0].ShowMonsterByID();

                    fightBossCnt._EnemyBornPos = new Transform[enemyPoses.Length - 1];
                    for (int i = 1; i < enemyPoses.Length; ++i)
                    {
                        fightBossCnt._EnemyBornPos[i - 1] = enemyPoses[i].transform;
                        //fightKCnt._EnemyMotionID = enemyPoses[i]._MonsterId;
                        enemyPoses[i].ShowMonsterByID();
                    }
                }
            }

            if (GUILayout.Button("RemoveAllShow"))
            {
                var enemyPoses = script.GetComponentsInChildren<MapPosObj>();

                for (int i = 0; i < enemyPoses.Length; ++i)
                {
                    enemyPoses[i].RemoveShow();
                }

            }

            //if (GUILayout.Button("FitEnemies"))
            //{
            //}

        }
    

        /// <summary>
        /// Draw Scene GUI handles, circles and outlines for submesh vertices.
        /// <summary>
        public void OnSceneGUI()
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hitInfo;
            Event e = Event.current;

            if (e.type == EventType.mouseDown && e.control)
            {
                Physics.Raycast(worldRay, out hitInfo);
                if (Physics.Raycast(worldRay, out hitInfo))
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.AddComponent<MapPosObj>();
                    go.name = "MapPos";
                    go.transform.SetParent(script.transform);
                    go.transform.position = hitInfo.point;
                }
                else
                {
                    Debug.Log("Not Hit Navmesh");
                }

            }
        }

        
    }
}