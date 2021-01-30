using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 1f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float jumpForce = 100f;
    [SerializeField, Range(0.0f, 0.99f)] float flipDeadZone = 0.05f;
    [SerializeField] Rigidbody2D[] groundedCheckRigidbodies;
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

    [SerializeField, HideInInspector] Animator _animator = null;
    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            return _animator;
        }
    }

    bool canJump = false;
    bool IsFacingRight = true;
    Flipable[] flipableComponents = null;

    void Awake()
    {
        flipableComponents = GetComponentsInChildren<Flipable>();
    }

    void Update()
    {
        for(int i = 0; i < groundedCheckRigidbodies.Length; i++)
        {
            if (!canJump && groundedCheckRigidbodies[i].IsTouching(groundedContactFilter) && groundedCheckRigidbodies[i].velocity.y <= 0)
            {
                canJump = true;
                Animator.SetBool("IsGrounded", true);
            }
        }
    }

    public void Move(float value)
    {
        if ((IsFacingRight && value < -flipDeadZone) || (!IsFacingRight && value > flipDeadZone))
        {
            foreach(Flipable element in flipableComponents)
            {
                element.SetFlip(IsFacingRight);
            }

            IsFacingRight = !IsFacingRight;
            Animator.SetBool("IsFacingRight", IsFacingRight);
        }
        
        if (Mathf.Abs(RigidBody.velocity.x) < maxSpeed)
        {
            RigidBody.AddForce(new Vector2(value * moveForce * RigidBody.mass, 0.0f));
        }

        Animator.SetFloat("Move", value);
    }

    public void Jump()
    {
        if (canJump)
        {   
            RigidBody.AddForce(new Vector2(0.0f, jumpForce * RigidBody.mass));
            canJump = false;
            Animator.SetBool("IsGrounded", false);
        }
    }
}
