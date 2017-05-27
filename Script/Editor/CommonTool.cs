
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CommonTool : Editor
{

    [MenuItem("TyTools/Map/DisableMapCollider")]
    public static void DisableMapCollider()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            DisableCollider(selectGO as GameObject);
        }
    }

    #region particleTime

    private static void DisableCollider(GameObject particleObj)
    {
        var colliders = particleObj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    #endregion
    
}
