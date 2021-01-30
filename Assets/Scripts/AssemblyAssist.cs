using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AssemblyAssist : MonoBehaviour
{
    [System.Serializable]
    public struct AssistSetting
    {
        public float maxTorque;
        public AnimationCurve animCurve;
    }

    [SerializeField] AssistSetting setting = new AssistSetting();
    [SerializeField] bool UseLocalSpace = false;

    Rigidbody2D rb;
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
	}

    void FixedUpdate()
    {
        float angle = UseLocalSpace ? transform.localEulerAngles.z : transform.eulerAngles.z;

        while (angle > 180f)
        {
            angle -= 360f;
        }
        
        while (angle < -180f)
        {
            angle += 360f;
        }

        rb.AddTorque(-setting.animCurve.Evaluate(Mathf.Abs(angle / 180f)) * setting.maxTorque * Mathf.Sign(angle));
    }
}
