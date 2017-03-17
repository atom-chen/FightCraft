using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 边缘高亮
public class EffectOutLine : EffectController
{
    public Material _OutLineMaterial;

    public override void PlayEffect()
    {
        base.PlayEffect();

        AddMaterial();
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect(speed);

        AddMaterial();
    }

    public override void HideEffect()
    {
        base.HideEffect();

        RemoveMaterial();
    }

    #region 

    private SkinnedMeshRenderer _SkinnedMesh;

    private void AddMaterial()
    {
        var motion = gameObject.GetComponentInParent<MotionManager>();
        _SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            Material[] newMaterialArray = new Material[_SkinnedMesh.materials.Length + 1];

            newMaterialArray[0] = _OutLineMaterial;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains("Outline"))
                {
                    return;
                }
                else
                {
                    newMaterialArray[i + 1] = _SkinnedMesh.materials[i];
                    newMaterialArray[i + 1].SetInt("_ZWrite", 1);
                    newMaterialArray[i + 1].renderQueue = 2450;
                }
            }

            _SkinnedMesh.materials = newMaterialArray;

        }
    }

    private void RemoveMaterial()
    {
        var motion = gameObject.GetComponentInParent<MotionManager>();
        _SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            int newMaterialArrayCount = 0;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains("Outline"))
                {
                    newMaterialArrayCount++;
                }
            }

            if (newMaterialArrayCount > 0)
            {
                Material[] newMaterialArray = new Material[newMaterialArrayCount];
                int curMaterialIndex = 0;
                for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
                {
                    if (curMaterialIndex >= newMaterialArrayCount)
                    {
                        break;
                    }
                    if (!_SkinnedMesh.materials[i].name.Contains("Outline"))
                    {
                        newMaterialArray[curMaterialIndex] = _SkinnedMesh.materials[i];
                        curMaterialIndex++;
                    }
                }

                _SkinnedMesh.materials = newMaterialArray;
            }
        }
    }

    #endregion
}