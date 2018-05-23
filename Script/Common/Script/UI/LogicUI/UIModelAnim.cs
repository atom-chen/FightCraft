using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class UIModelAnim : MonoBehaviour
{

    private Animation _Animation;
    private List<AnimationClip> _Anims;

    public void Awake()
    {
        _Animation = GetComponent<Animation>();
    }

    public void InitAnim(List<AnimationClip> anims)
    {
        _Anims = anims;
        _Animation.AddClip(_Anims[0], "0");
        _Animation.AddClip(_Anims[1], "1");

        PlayAnim();
    }

    public void PlayAnim()
    {
        StartCoroutine(PlayerAnim());
    }

    private IEnumerator PlayerAnim()
    {
        
        _Animation.Play("0");

        yield return new WaitForSeconds(_Anims[0].length);

        _Animation.Play("1");

    }
}
