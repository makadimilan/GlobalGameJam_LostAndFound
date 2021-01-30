using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flipable : MonoBehaviour
{
    [SerializeField] bool FlipSpriteX = true;
    [SerializeField] bool FlipSpriteY = false;
    [SerializeField] bool FlipSortingOrder = false;

    [SerializeField] bool FlipTransformX = false;
    [SerializeField] bool FlipTransformY = false;
    [SerializeField] bool FlipHingeJointAngleLimits = false;

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

    [SerializeField, HideInInspector] HingeJoint2D _hingeJoint = null;
    public HingeJoint2D HingeJoint
    {
        get
        {
            if (_hingeJoint == null)
            {
                _hingeJoint = GetComponent<HingeJoint2D>();
            }

            return _hingeJoint;
        }
    }

    Vector3 OriginalLocalScale;
    bool OriginalFlipX;
    bool OriginalFlipY;
    int OriginalSortingOrder;
    float OriginalHingeJointLowerAngleLimit;
    float OriginalHingeJointUpperAngleLimit;

    void Awake()
    {
        OriginalLocalScale = transform.localScale;

        if (SpriteRenderer)
        {
            OriginalFlipX = SpriteRenderer.flipX;
            OriginalFlipY = SpriteRenderer.flipY;
            OriginalSortingOrder = SpriteRenderer.sortingOrder;
        }
        else if (FlipSpriteX || FlipSpriteY || FlipSortingOrder)
        {
            Debug.LogError("Can not flip missing SpriteRenderer", gameObject);
        }

        if (HingeJoint)
        {
            OriginalHingeJointLowerAngleLimit = HingeJoint.limits.min;
            OriginalHingeJointUpperAngleLimit = HingeJoint.limits.max;
        }
        else if (FlipHingeJointAngleLimits)
        {
            Debug.LogError("Can not flip missing HingeJoint", gameObject);
        }
    }

    public void SetFlip(bool value)
    {
        if (SpriteRenderer)
        {
            SpriteRenderer.flipX = FlipSpriteX ? value ^ OriginalFlipX : OriginalFlipX;
            SpriteRenderer.flipY = FlipSpriteY ? value ^ OriginalFlipY : OriginalFlipY;

            if (FlipSortingOrder)
            {
                SpriteRenderer.sortingOrder = value ? -OriginalSortingOrder : OriginalSortingOrder;
            }
        }

        if (HingeJoint && FlipHingeJointAngleLimits)
        {
            float limitA = value ? OriginalHingeJointLowerAngleLimit-180 : OriginalHingeJointLowerAngleLimit;
            float limitB = value ? OriginalHingeJointUpperAngleLimit-180 : OriginalHingeJointUpperAngleLimit;

            while(limitA < -180)
            {
                limitA += 360;
            }

            while(limitB < -180)
            {
                limitB += 360;
            }

            while(limitA > 180)
            {
                limitA -= 360;
            }

            while(limitB > 180)
            {
                limitB -= 360;
            }

            JointAngleLimits2D tempLimits = HingeJoint.limits;
            tempLimits.min = Mathf.Min(limitA, limitB);
            tempLimits.max = Mathf.Max(limitA, limitB);
            HingeJoint.limits = tempLimits;
        }

        Vector3 tempScale = OriginalLocalScale;

        if (FlipTransformX)
        {
            tempScale.x = value ? -OriginalLocalScale.x : OriginalLocalScale.x;
        }

        if (FlipTransformY)
        {
            tempScale.y = value ? -OriginalLocalScale.y : OriginalLocalScale.y;
        }

        transform.localScale = tempScale;
    }
}
