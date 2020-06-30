using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class PlayerAnimation : PlayerController
{
    [Header("General")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer rimRenderer;
    public Rigidbody2D rb;
    public TrailRenderer trailRenderer;
    public float trailTime = .5f;


    #region Rim Animation
    public GameObject rimObject;
    public float jumpingScaleEffector;
    public float fallingScaleEffector;
    public float jumpingEffectTime = 2;
    public float fallingEffectTime = 2;
    [MinMaxSlider(-5, 5)]
    public Vector2 trailTimeClamp;
    private Vector3 scaleNormKeeper;
    #endregion


    //* PRIVATE //
    [HideInInspector]
    public bool isFacingRight = true;
    [HideInInspector]
    public bool isFlipped = false;


    public override void Start()
    {
        base.Start();
        if (rimObject != null)
            scaleNormKeeper = rimObject.transform.localScale;
        if (trailRenderer != null)
            trailRenderer.endColor = Color.clear;
    }

    public void Update()
    {
        #region Rim Jelly Effect
        /**if (rb.velocity.y > 0)
        {
            Vector3 tempScale = scaleNormKeeper;
            tempScale.x = Mathf.Lerp(transform.localScale.x, jumpingScaleEffector, jumpingEffectTime * Time.deltaTime);
            rimRenderer.transform.localScale = tempScale;
        }
        else if (rb.velocity.y < 0)
        {
            Vector3 tempScale = scaleNormKeeper;
            tempScale.y = Mathf.Lerp(transform.localScale.y, fallingScaleEffector, fallingEffectTime * Time.deltaTime);
            rimRenderer.transform.localScale = tempScale;
        }
        else
        {
            Vector3 tempScale = scaleNormKeeper;
            tempScale.x = Mathf.Lerp(rimRenderer.transform.localScale.x, scaleNormKeeper.x, fallingEffectTime * Time.deltaTime);
            tempScale.y = Mathf.Lerp(rimRenderer.transform.localScale.y, scaleNormKeeper.y, fallingEffectTime * Time.deltaTime);

            rimRenderer.transform.localScale = tempScale;
        }*/
        #endregion

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
            if (isFlipped == false && playerStateMachine.colorScripts.purpleMovement.gravityContainer < 0)
            {
                FlipUp();
            }
            else if (isFlipped == true && playerStateMachine.colorScripts.purpleMovement.gravityContainer > 0)
            {
                FlipUp();
            }
        }

        if (trailRenderer != null)
        {
            switch (playerStateMachine.colorState)
            {
                case ColorStates.Red:
                    trailRenderer.startColor = playerStateMachine.colors[0].primaryColor;
                    break;
                case ColorStates.Yellow:
                    trailRenderer.startColor = playerStateMachine.colors[1].primaryColor;
                    break;
                case ColorStates.Green:
                    trailRenderer.startColor = playerStateMachine.colors[2].primaryColor;
                    break;
                case ColorStates.Blue:
                    trailRenderer.startColor = playerStateMachine.colors[3].primaryColor;
                    break;
                case ColorStates.Purple:
                    trailRenderer.startColor = playerStateMachine.colors[4].primaryColor;
                    break;
            }

            trailRenderer.time = Mathf.Clamp((trailTime * ((rb.velocity.x + rb.velocity.y) / 2) <= 0) ? trailTime * ((rb.velocity.x + rb.velocity.y) / 2) * -1 : trailTime * ((rb.velocity.x + rb.velocity.y) / 2), trailTimeClamp.x, trailTimeClamp.y);
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
