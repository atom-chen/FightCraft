using UnityEngine;
using System.Collections;

public class ObjectTween : MonoBehaviour {

    private delegate void OperationFun();

    public enum Operation
    {
        None,
        rotation,
        ChangeSize,
        FowardCamera,
        MoveTo,
    }

    public Operation[] m_operas;
    public float m_speedX = 1.0f;
    public float m_speedY = 1.0f;
    public float m_speedZ = 1.0f;

    private float startTime = 0;
    public float moveTime = 1;
    private Vector3 startPos = Vector3.zero;
    public Vector3 destantPos = Vector3.zero;
    public bool isDestroy = true;

    public int Axis;  //1X轴 2Y轴 4Z轴
    private int axisX = 1;
    private int axisY = 2;
    private int axisZ = 4;
    private OperationFun m_operaFun = null;
    // Use this for initialization
    void DoAction () {

        for(int i=0;i<m_operas.Length;i++)
        {
            switch (m_operas[i])
            {
                case Operation.rotation:
                    {
                        axisX = axisX & Axis;
                        axisY = axisY & Axis;
                        axisZ = axisZ & Axis;
                        RationSelf();
                        break;
                    }
                case Operation.ChangeSize:
                    {
                        ChangeSizeSelf();
                        break;
                    }
                case Operation.FowardCamera:
                    {
                        FowardCamera();
                        break;
                    }
                case Operation.MoveTo:
                    {
                        if(startTime==0 && startPos==Vector3.zero)
                        {
                            startTime = Time.realtimeSinceStartup;
                            startPos = transform.localPosition;
                        }

                        MoveTo();
                        break;
                    }
            }
        }

	}
	
	// Update is called once per frame
	void Update () {

        DoAction();

    }

    void RationSelf()
    {
        transform.Rotate(new Vector3(axisX*m_speedX,axisY*m_speedY, axisZ*m_speedZ));
    }
    void ChangeSizeSelf()
    {
        Projector pro = GetComponentInChildren<Projector>();
        if (pro != null)
            pro.orthographicSize = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;
    }

    void FowardCamera()
    {
        if(Camera.main!=null)
        {
            Vector3 lookAt = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
            transform.LookAt(lookAt);
        }
    }

    void MoveTo()
    {
        if((Time.realtimeSinceStartup - startTime)>=moveTime)
        {
            if(isDestroy)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
            }
            return;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, destantPos, (Time.realtimeSinceStartup - startTime) / moveTime);
    }
}
