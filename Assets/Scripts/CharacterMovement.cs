using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 1f;
    [SerializeField] float maxSpeed = 10f;

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

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(float value)
    {
        if (Mathf.Abs(RigidBody.velocity.x) < maxSpeed)
        {
            RigidBody.AddForce(new Vector2(value * moveForce, 0.0f));
        }
    }
}
