using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : MonoBehaviour
{
    [SerializeField] float length = 100;
    [SerializeField] float RaiseSpeed = 10;
    [SerializeField] float Inertia = 1f/30f;

    float progress = 0;
    bool isRaising = false;
    Vector3 originalPosition;

    void Awake() 
    {
        originalPosition = transform.position;
    }

    public void Raise() 
    {
        isRaising = true;
    }

    public void Fall() 
    {
        isRaising = false;
    }

    void FixedUpdate() 
    {
        if (isRaising && progress < length)
        {
            progress += RaiseSpeed * Time.fixedDeltaTime;
        }
        else if (!isRaising && progress > 0)
        {
            progress = (0 - progress) * Mathf.Clamp01(Time.fixedDeltaTime / Inertia);
        }

        progress = Mathf.Clamp(progress, 0, length);
        transform.position = originalPosition + Vector3.up * progress;
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * length);
    }
}
