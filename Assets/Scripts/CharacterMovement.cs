using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] float moveForce = 1f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float jumpForce = 100f;
    [SerializeField, Range(0.0f, 0.99f)] float flipDeadZone = 0.05f;
    [SerializeField] Rigidbody2D[] groundedCheckRigidbodies = null;
    [SerializeField] ContactFilter2D groundedContactFilter = new ContactFilter2D();
    [SerializeField] Rigidbody2D armTarget = null; 
    [SerializeField] float armTargetLenght = 1;
    [SerializeField] AnimationCurve armTargeFrequency = AnimationCurve.Linear(0,0, 1,1);

    enum JumpState 
    {
        Grounded,
        Jumping,
        Falling
    }

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

    JumpState jumpState = JumpState.Falling;
    bool IsFacingRight = true;
    Flipable[] flipableComponents = null;
    SpringJoint2D[] armTargetSpringJoints = null;
    CharacterHand[] handComponents = null;

    void Awake()
    {
        flipableComponents = GetComponentsInChildren<Flipable>();
        armTargetSpringJoints = armTarget.GetComponents<SpringJoint2D>();
        handComponents = GetComponentsInChildren<CharacterHand>();
    }

    void Update()
    {
        if (jumpState == JumpState.Jumping && RigidBody.velocity.y < 0)
        {
            jumpState = JumpState.Falling;
        }

        if (jumpState == JumpState.Falling)
        {
            for(int i = 0; i < groundedCheckRigidbodies.Length; i++)
            {
                ContactPoint2D[] contacts = new ContactPoint2D[1];
                if (groundedCheckRigidbodies[i].GetContacts(groundedContactFilter, contacts) > 0)
                {
                    bool isValidGround = true;
                    for(int j = 0; j < handComponents.Length; j++)
                    {
                        Rigidbody2D otherRB = contacts[0].otherRigidbody;
                        if (otherRB != null && otherRB == handComponents[j].GetGrabbedObject())
                        {
                            isValidGround = false;
                            break;
                        }
                    }

                    if (isValidGround)
                    {
                        jumpState = JumpState.Grounded;
                        Animator.SetBool("IsGrounded", true);
                    }
                }
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
        }
        
        if (Mathf.Abs(RigidBody.velocity.x) < maxSpeed)
        {
            RigidBody.AddForce(new Vector2(value * moveForce * RigidBody.mass, 0.0f));
        }

        Animator.SetFloat("MoveSpeed", Mathf.Abs(value));
    }

    public void SetArmTarget(Vector2 value)
    {
        if (value.sqrMagnitude > 1)
        {
            value = value.normalized;
        }

        armTarget.transform.localPosition = value * armTargetLenght;

        for(int i = 0; i < armTargetSpringJoints.Length; i++)
        {
            armTargetSpringJoints[i].frequency = 0.0001f + armTargeFrequency.Evaluate(value.magnitude);
        }
    }

    public void Jump()
    {
        if (jumpState == JumpState.Grounded)
        {   
            RigidBody.AddForce(new Vector2(0.0f, jumpForce * RigidBody.mass));
            jumpState = JumpState.Jumping;
            Animator.SetBool("IsGrounded", false);
            Debug.Log("jump");
        }
    }

    public void SetHandGrab(bool value)
    {
        for(int i = 0; i < handComponents.Length; i++)
        {
            handComponents[i].SetHandGrab(value);
        }   
    }
}
