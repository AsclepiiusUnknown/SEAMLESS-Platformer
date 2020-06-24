using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : PlayerController
{
    [Header("Animation")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer rimRenderer;

    //* PRIVATE //
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isFlipped = false;


    public override void Update()
    {
        base.Update();
        /*if (isFacingRight == false && playerMovement.xRawInput > 0)
        {
            FlipSide();
        }
        else if (isFacingRight == true && playerMovement.xRawInput < 0)
        {
            FlipSide();
        }*/

        if (playerStateMachine.colorScripts.purpleMovement != null)
        {
            if (isFlipped == false && playerStateMachine.colorScripts.purpleMovement.isNegativeGrav)
            {
                FlipUp();
            }
            else if (isFlipped == true && !playerStateMachine.colorScripts.purpleMovement.isNegativeGrav)
            {
                FlipUp();
            }
        }

    }

    void FlipSide()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void FlipUp()
    {
        isFlipped = !isFlipped;
        Vector3 Scaler = transform.localScale;
        Scaler.y *= -1;
        transform.localScale = Scaler;
    }
}
