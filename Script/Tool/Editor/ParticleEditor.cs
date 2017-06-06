
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public class ParticleEditor : Editor
{

    [MenuItem("TyTools/Editor/ParticleTime")]
    public static void ParticleTime()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            ParticleTimeInner(selectGO as GameObject);
        }
    }

    [MenuItem("TyTools/Editor/ParticleAwakePlay")]
    public static void ParticleAwakePlay()
    {
        var selects = Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel);
        foreach (var selectGO in selects)
        {
            ParticleAwakePlayInner(selectGO as GameObject);
        }
    }


    #region particleTime

    private static void ParticleTimeInner(GameObject particleObj)
    {
        var particleSys = particleObj.GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in particleSys)
        {
            float delayTime = particle.startDelay;
            ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[particle.emission.burstCount];
            particle.emission.GetBursts(bursts);
            if (bursts.Length > 0)
            {
                bursts[0].time = delayTime;
            }
            particle.emission.SetBursts(bursts);


        }
    }

    #endregion

    #region particleAwkePlay

    private static void ParticleAwakePlayInner(GameObject particleObj)
    {
        var particleSys = particleObj.GetComponentsInChildren<ParticleSystem>();
        foreach (var particle in particleSys)
        {
            particle.playOnAwake = true;


        }
    }

    #endregion
}
