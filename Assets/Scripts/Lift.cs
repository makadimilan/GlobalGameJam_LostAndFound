using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Lift : MonoBehaviour
{
    [SerializeField] float length = 100;
    [SerializeField] float RaiseSpeed = 10;
    [SerializeField] float Inertia = 1f/30f;

    [SerializeField, HideInInspector] Rigidbody2D _rigidBody = null;
    public Rigidbody2D RigidBody
    {
        get
        {
            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody2D>();
            }

            return _rigidBody;
        }
    }

    float progress = 0;
    bool isRaising = false;
    Vector2 originalPosition;

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
            progress += (0 - progress) * Mathf.Clamp01(Time.fixedDeltaTime / Inertia);
        }

        progress = Mathf.Clamp(progress, 0, length);
        RigidBody.MovePosition(originalPosition + Vector2.up * progress);
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * length);
    }
}
