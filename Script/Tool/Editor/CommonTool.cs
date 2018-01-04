
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class CommonTool : Editor
{

    #region disableCollider

    [MenuItem("TyTools/Map/DisableMapCollider")]
    public static void DisableMapCollider()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            DisableCollider(selectGO as GameObject);
        }
    }

    private static void DisableCollider(GameObject particleObj)
    {
        var colliders = particleObj.GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    #endregion

    #region disableshadow

    [MenuItem("TyTools/Model/DisableShadow")]
    public static void DisableShadow()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            DisableShadow(selectGO as GameObject);
        }
    }

    private static void DisableShadow(GameObject particleObj)
    {
        var skinRenders = particleObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var render in skinRenders)
        {
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            render.receiveShadows = false;
        }

        var meshRenders = particleObj.GetComponentsInChildren<MeshRenderer>();
        foreach (var render in meshRenders)
        {
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            render.receiveShadows = false;
        }
    }

    #endregion

    #region 

    [MenuItem("TyTools/Test/CaptureScreenshot")]
    public static void CaptureScreenshot()
    {
        Application.CaptureScreenshot(Application.dataPath + "/" + "capture.png", 0);

    }

    #endregion
}
