using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : Grabable
{
    [SerializeField] float length = 100;
    [SerializeField] float RaiseSpeed = 10;
    [SerializeField] float Inertia = 1f/30f;

    float progress = 0;
    bool isGrabbed = false;
    Vector3 originalPosition;

    void Awake() 
    {
        originalPosition = transform.position;
    }

    protected override void OnGrabStarted() 
    {
        isGrabbed = true;
    }

    protected override void OnGrabEnded() 
    {
        isGrabbed = false;
    }

    void FixedUpdate() 
    {
        if (isGrabbed && progress < length)
        {
            progress += RaiseSpeed * Time.fixedDeltaTime;
        }
        else if (!isGrabbed && progress > 0)
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
