using UnityEngine;
using System.Collections;

namespace Prg.Scripts
{
    public class PS_AutoDestory : MonoBehaviour {

        [SerializeField]
        float mfSeconds=1.0f;

	    // Use this for initialization
	    void Start () {
            StartCoroutine(OnDestoryCallBack());
	
	    }
        IEnumerator OnDestoryCallBack()
        {
            yield return new WaitForSeconds(mfSeconds);

            DestroyObject(gameObject);
        }
	
	    
    }
}
