using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerController
{
    [Header("Sprite Flipping")]
    //* PRIVATE //
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    private SpriteRenderer spriteRenderer;

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFacingRight == false && playerMovement.xRawInput > 0)
        {
            Flip();
        }
        else if (isFacingRight = true && playerMovement.xRawInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
