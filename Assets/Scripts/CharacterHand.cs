using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer))]
public class CharacterHand : MonoBehaviour
{
    [SerializeField] ContactFilter2D handContactFilter = new ContactFilter2D();
    [SerializeField] List<Sprite> handSprites = null;
    [SerializeField] HingeJoint2D grabJoint = null;

    enum HandState 
    {
        Idle,
        Open,
        Grab
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

    [SerializeField, HideInInspector] SpriteRenderer _spriteRenderer = null;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            return _spriteRenderer;
        }
    }

    HandState handState = HandState.Idle;

    void Awake() 
    {
        grabJoint.enabled = false;
    }

    void Update()
    {
        ContactPoint2D[] contacts = new ContactPoint2D[1];
        if (handState == HandState.Open && RigidBody.GetContacts(handContactFilter, contacts) > 0)
        {
            handState = HandState.Grab;
            SpriteRenderer.sprite = handSprites[(int)handState];
            
            grabJoint.connectedBody = contacts[0].rigidbody;
            grabJoint.connectedAnchor = contacts[0].rigidbody.GetPoint(contacts[0].point);
            grabJoint.enabled = true;

            Grabable grabable = grabJoint.connectedBody.GetComponent<Grabable>();
            if (grabable != null)
            {
                grabable.StartGrab(this);
            }
        }
    }

    public void SetHandGrab(bool value)
    {
        if (handState == HandState.Idle && value)
        {
            handState = HandState.Open;
            SpriteRenderer.sprite = handSprites[(int)handState];
        }
        else if (!value)
        {
            handState = HandState.Idle;
            SpriteRenderer.sprite = handSprites[(int)handState];
            ReleaseGrabbedObject();
        }
    }

    public void ReleaseGrabbedObject()
    {
        Grabable grabable = grabJoint.connectedBody?.GetComponent<Grabable>();
        if (grabable != null)
        {
            grabable.StopGrab(this);
        }

        grabJoint.connectedBody = null;
        grabJoint.enabled = false;
    }

    public Rigidbody2D GetGrabbedObject()
    {
        return grabJoint.isActiveAndEnabled ? grabJoint.connectedBody : null;
    }
}
