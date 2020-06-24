using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowMovement : PlayerController
{
    #region Variables
    // * //
    #region DAMPING
    [Header("Ground Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float moveDampPct = .1f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float stopDampPct = .7f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float turnDampPct = 1; //The pct used to dampen normal turning to go the other way


    [Header("Air Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float airMoveDampPct = .15f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float airStopDampPct = .6f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float airTurnDampPct = .9f; //The pct used to dampen normal turning to go the other way
    [Range(0, 1)] //Kept as a pct value
    public float dashStopDampPct = .6f; //The pct used to dampen dash decceleration/stopping
    #endregion


    #region JUMPING
    [Header("Jumping")]
    public float jumpForce = 6;
    public int extraJumpValue = 2;
    [Range(0, 1)]
    public float jumpHeightCutPct = .5f;
    public float coyoteTimeValue = .3f;
    #endregion


    #region DASHING
    [Header("Dashing")]
    public float dashSpeed = 5;
    public float dashLength = .4f;
    private bool hasDashed;
    private bool isDashing;
    #endregion


    #region MISC.
    [Header("MISC")]
    public float speedMultiplier = .95f; //Multiplier used to change the overall speed (1 = no difference)
    public float dampElapseTime = 10; //How long the damping is spread over time (can be split like the damp pct values above)

    //* PRIVATE //
    [HideInInspector]
    public float gravityScaleKeeper;
    [HideInInspector]
    public float xRawInput; //The integer input between -1 and 1 to move and check movements on x
    [HideInInspector]
    public float yRawInput; //The integer input between -1 and 1 to move and check movements on y
    [HideInInspector]
    public Rigidbody2D rb; //The players rigidbody
    [HideInInspector]
    private float CoyoteTimeKeeper;
    [HideInInspector]
    private int extraJumpKeeper;
    #endregion
    #endregion

    #region Setup
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        gravityScaleKeeper = rb.gravityScale;
        extraJumpKeeper = extraJumpValue;
        CoyoteTimeKeeper = coyoteTimeValue;
    }
    #endregion

    public override void Update()
    {
        base.Update();
        rb.gravityScale = (playerCollision.isLedgeGrabbing) ? 0 : gravityScaleKeeper;

        #region Jumping
        CoyoteTimeKeeper -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.T))
        {
            CoyoteTimeKeeper = coyoteTimeValue;
        }

        if (playerCollision.isGrounded == true)
        {
            playerCollision.grndLeewayKeeper = playerCollision.grndLeewayValue;
            extraJumpKeeper = extraJumpValue;
        }

        if (CoyoteTimeKeeper > 0 && playerCollision.grndLeewayKeeper > 0)
        {
            CoyoteTimeKeeper = 0;
            playerCollision.grndLeewayKeeper = 0;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (Input.GetKeyDown(KeyCode.T) && extraJumpKeeper > 0 && !playerCollision.isGrounded && !playerCollision.isLedgeGrabbing) //ledge grabbing?
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            extraJumpKeeper--;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightCutPct);
            }
        }
        #endregion
    }

    private void FixedUpdate()
    {
        #region Movement
        // * //
        #region Raw Input Setup
        //Horizontal
        xRawInput = rb.velocity.x;
        xRawInput += Input.GetAxisRaw("Horizontal");
        //Vertical
        yRawInput = rb.velocity.y;
        yRawInput += Input.GetAxisRaw("Vertical");
        #endregion

        #region Ground Damping
        if (playerCollision.isGrounded)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < .01f)
            {
                xRawInput *= Mathf.Pow(1f - stopDampPct, Time.deltaTime * dampElapseTime);
            }
            else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(xRawInput))
            {
                xRawInput *= Mathf.Pow(1f - turnDampPct, Time.deltaTime * dampElapseTime);
            }
            else
            {
                xRawInput *= Mathf.Pow(1f - moveDampPct, Time.deltaTime * dampElapseTime);
            }
        }
        #endregion
        #region Air Damping
        else
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) < .01f)
            {
                if (!isDashing)
                    xRawInput *= Mathf.Pow(1f - airStopDampPct, Time.deltaTime * dampElapseTime);
                else
                {
                    xRawInput *= Mathf.Pow(1f - dashStopDampPct, Time.deltaTime * dampElapseTime);
                }
            }
            else if (Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(xRawInput))
            {
                xRawInput *= Mathf.Pow(1f - airTurnDampPct, Time.deltaTime * dampElapseTime);
            }
            else
            {
                xRawInput *= Mathf.Pow(1f - airMoveDampPct, Time.deltaTime * dampElapseTime);
            }
        }
        #endregion

        #region Apply 
        if (!playerCollision.isLedgeGrabbing)
        {
            rb.velocity = new Vector2(xRawInput * speedMultiplier, rb.velocity.y);
        }
        #endregion
        #endregion
    }


    #region Dashing Functionality
    private void Dash(float x, float y)
    {
        isDashing = true;

        hasDashed = true;

        //anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);
        //print("Dashed" + x + ", " + y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        StartCoroutine(GroundDash());

        //dashParticle.Play();
        rb.gravityScale = 0;
        //GetComponent<BetterJumping>().enabled = false;
        //wallJumped = true;
        // 
        //isDashing = true;

        yield return new WaitForSeconds(dashLength);

        rb.gravityScale = gravityScaleKeeper;
        //GetComponent<BetterJumping>().enabled = true;
        //wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (playerCollision.isGrounded)
            hasDashed = false;
    }
    #endregion
}
