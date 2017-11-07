using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// char alpha
public class EffectModelAlpha : EffectController
{
    public override void PlayEffect()
    {
        base.PlayEffect();
        InitShader();
        ChangeCharShader();
    }

    public override void HideEffect()
    {
        base.HideEffect();
        ChangeCharShaderBack();
    }

    #region org

    private Shader _OrgShader;
    private SkinnedMeshRenderer _CharMesh;

    private void ChangeCharShader()
    {
        var motion = gameObject.GetComponentInParent<MotionManager>();
        var skinnedRenderers = motion.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var skinnedRender in skinnedRenderers)
        {
            if (skinnedRender.material.shader.name == "TYImage/Standard(Specular)CullOff")
            {
                _CharMesh = skinnedRender;
            }
        }
        if (_CharMesh.material.shader.name == "TYImage/CharAlpha")
            return;
        _OrgShader = _CharMesh.material.shader;
        _CharMesh.material.shader = _AlphaShader;
    }

    private void ChangeCharShaderBack()
    {
        _CharMesh.material.shader = _OrgShader;
    }

    #endregion

    #region shader

    public static string _AlphaShaderName = "TYImage/CharAlpha";
    public static Shader _AlphaShader;

    public static void InitShader()
    {
        if (_AlphaShader == null)
        {
            _AlphaShader = Shader.Find(_AlphaShaderName);
        }
    }

    #endregion
}