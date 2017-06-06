
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
    [CustomEditor(typeof(MapPosObj))]
    public class MapPosObjEditor : Editor
    {
        //navmesh object reference
        private MapPosObj script;

        private bool placing;


        void OnEnable()
        {
            script = (MapPosObj)target;
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
                List<Transform> enemyPoses = new List<Transform>();
                var trans = script.GetComponentsInChildren<Transform>();
                foreach (var tran in trans)
                {
                    if (tran.name == "MapPos")
                    {
                        enemyPoses.Add(tran);
                    }
                }

                var fightKAll = script.GetComponent<FightSceneAreaKAllEnemy>();
                if (fightKAll != null)
                {
                    fightKAll._EnemyBornPos = new SerializeEnemyInfo[enemyPoses.Count];
                    for (int i = 0; i < enemyPoses.Count; ++i)
                    {
                        fightKAll._EnemyBornPos[i] = new SerializeEnemyInfo();
                        fightKAll._EnemyBornPos[i]._EnemyTransform = enemyPoses[i];
                        fightKAll._EnemyBornPos[i]._EnemyDataID = "23";
                    }
                }

                var fightKCnt = script.GetComponent<FightSceneAreaKEnemyCnt>();
                if (fightKCnt != null)
                {
                    fightKCnt._EnemyBornPos = new Transform[enemyPoses.Count];
                    for (int i = 0; i < enemyPoses.Count; ++i)
                    {
                        fightKCnt._EnemyBornPos[i] = enemyPoses[i];
                        fightKCnt._EnemyMotionID = "23";
                    }
                }
            }

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