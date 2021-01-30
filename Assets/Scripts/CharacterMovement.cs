using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 1f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float jumpForce = 100f;

    [SerializeField] ContactFilter2D groundedContactFilter;

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

    bool _canJump = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (!_canJump && RigidBody.IsTouching(groundedContactFilter))
        {
            _canJump = true;
        }
    }

    public void Move(float value)
    {
        if (Mathf.Abs(RigidBody.velocity.x) < maxSpeed)
        {
            RigidBody.AddForce(new Vector2(value * moveForce, 0.0f));
        }
    }

    public void Jump()
    {
        if (_canJump)
        {   
            RigidBody.AddForce(new Vector2(0.0f, jumpForce));
            _canJump = false;
        }
    }
}
