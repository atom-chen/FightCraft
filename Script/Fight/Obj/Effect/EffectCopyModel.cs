using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 残影效果
public class EffectCopyModel : EffectController
{

    public float _Duration;
    
    public MeshRenderer[] _MeshRenderers;
    public SkinnedMeshRenderer[] SkinnedRenderers;
    public Material _Material;

    private List<Mesh> _BakedMeshes = new List<Mesh>();

    public override void PlayEffect()
    {
        base.PlayEffect();
        InitMesh();
        CreateImage();
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect();
        InitMesh();
        CreateImage();
    }

    public override void HideEffect()
    {
        _BakedMeshes.Clear();
        base.HideEffect();
    }

    private void InitMesh()
    {
        if (_MeshRenderers.Length == 0 || SkinnedRenderers.Length == 0)
        {
            var motion = gameObject.GetComponentInParent<MotionManager>();
            //_MeshRenderers = motion.GetComponentsInChildren<MeshRenderer>();
            SkinnedRenderers = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
        }
    }

    private void CreateImage()
    {

        CombineInstance[] combineInstances = new CombineInstance[_MeshRenderers.Length + SkinnedRenderers.Length];

        Transform t = transform;
        Material mat = null;
        
        for (int i = 0; i < _MeshRenderers.Length; ++i)
        {
            var item = _MeshRenderers[i];
            t = item.transform;
            mat = new Material(_Material);
            //mat.shader = _shaderAfterImage;

            var mesh = GameObject.Instantiate<Mesh>(item.GetComponent<MeshFilter>().mesh);
            _BakedMeshes.Add(mesh);
        }
        for (int i = 0; i < SkinnedRenderers.Length; ++i)
        {
            var item = SkinnedRenderers[i];
            t = item.transform;
            mat = new Material(_Material);
            //mat.shader = _shaderAfterImage;

            var mesh = new Mesh();
            item.BakeMesh(mesh);
            _BakedMeshes.Add(mesh);
        }

        foreach (var mesh in _BakedMeshes)
        {

        }

    }

}