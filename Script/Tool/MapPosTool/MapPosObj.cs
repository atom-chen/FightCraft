using UnityEngine;
using System.Collections;

public class MapPosObj : MonoBehaviour
{

    public string _MonsterId = "23";

    public void ShowMonsterByID()
    {
        if (Tables.TableReader.MonsterBase == null)
        {
            Tables.TableReader.ReadTables();
        }
        var monsterBase = Tables.TableReader.MonsterBase.GetRecord(_MonsterId);
        if (monsterBase == null)
        {
            Debug.Log("MonsterBase Null:" + _MonsterId);
            var priObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            priObj.transform.SetParent(transform);
            priObj.transform.localPosition = Vector3.zero;
            priObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
            priObj.name = "ShowChil";
            return;
        }

        var obj = GameBase.ResourceManager.Instance.GetInstanceGameObject("ModelBase/" + monsterBase.Name);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        obj.name = "ShowChil";
    }

    public void RemoveShow()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            GameObject.DestroyImmediate(meshFilter);
        }

        var meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            GameObject.DestroyImmediate(meshRenderer);
        }

        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            GameObject.DestroyImmediate(collider);
        }

        var showChil = transform.FindChild("ShowChil");
        if (showChil != null)
        {
            GameObject.DestroyImmediate(showChil.gameObject);
        }
    }
}
