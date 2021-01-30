using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Flipable : MonoBehaviour
{
    [SerializeField] bool FlipSpriteX = true;
    [SerializeField] bool FlipSpriteY = false;

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

    Vector3 OriginalLocalScale;
    bool OriginalFlipX;
    bool OriginalFlipY;

    void Awake()
    {
        OriginalLocalScale = transform.localScale;
        OriginalFlipX = SpriteRenderer.flipX;
        OriginalFlipY = SpriteRenderer.flipY;
    }

    public void SetFlip(bool value)
    {
        SpriteRenderer.flipX = FlipSpriteX ? value ^ OriginalFlipX : OriginalFlipX;
        SpriteRenderer.flipY = FlipSpriteY ? value ^ OriginalFlipY : OriginalFlipY;
    }
}
