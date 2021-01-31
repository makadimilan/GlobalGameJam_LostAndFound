using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OverrideCenterOfMass : MonoBehaviour
{
    [SerializeField] Vector2 centerOfMass = new Vector2();
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

    void Awake() 
    {
        RigidBody.centerOfMass = centerOfMass;
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.DrawSphere(RigidBody.GetRelativePoint(centerOfMass), 0.1f);    
    }
}
