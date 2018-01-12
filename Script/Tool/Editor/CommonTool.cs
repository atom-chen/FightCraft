
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

    [MenuItem("TyTools/Test/GemSetTest")]
    public static void GemSetTest()
    {
        foreach (var gemSetRecord in Tables.TableReader.GemSet.Records.Values)
        {
            GemSetGemsCriticSelf(gemSetRecord);
        }

    }

    private static void GemSetGemsCriticSelf(Tables.GemSetRecord gemSet)
    {
        foreach (var gem in gemSet.Gems)
        {
            var sameGems = gemSet.Gems.FindAll((gemInfo) =>
            {
                if (gem.Id == gemInfo.Id)
                    return true;
                return false;
            });
            if (sameGems.Count > 1)
            {
                Debug.Log("GemSetGemsCriticSelf:" + gemSet.Id);
            }
        }
    }

    [MenuItem("TyTools/Test/GemSetGroupTest")]
    public static void GemSetCriticTest()
    {
        foreach (var gemSetRecordA in Tables.TableReader.GemSet.Records.Values)
        {
            foreach (var gemSetRecordB in Tables.TableReader.GemSet.Records.Values)
            {
                if (gemSetRecordA == gemSetRecordB)
                    continue;

                GemSetCriticTableTest(gemSetRecordA, gemSetRecordB);
            }
        }
    }

    public static void GemSetCriticTableTest(Tables.GemSetRecord gemSetA, Tables.GemSetRecord gemSetB)
    {
        foreach (var gem in gemSetA.Gems)
        {
            var sameGems = gemSetB.Gems.Find((gemInfo) =>
            {
                if (gem.Id == gemInfo.Id)
                    return true;
                return false;
            });

            if (sameGems == null)
            {
                return;
            }
        }
        Debug.Log("gemSetCritic:" + gemSetA.Id + "," + gemSetB.Id);
    }

    [MenuItem("TyTools/Test/NumaricTest")]
    public static void NumaricTest()
    {
        int testNum = 1500;
        for(int i = 0; i< 100; ++i)
        {
            Debug.Log(testNum);
            int decNum = Mathf.CeilToInt( testNum * 0.03f);
            decNum = Mathf.Max(decNum, Mathf.CeilToInt(testNum / 100 + 1));
            testNum = testNum - decNum;
        }
    }
    #endregion
}
