using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerController
{
    #region Variables
    // * //
    #region DAMPING
    [Header("Ground Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float moveDampPct = .25f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float stopDampPct = .6f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float turnDampPct = .2f; //The pct used to dampen normal turning to go the other way


    [Header("Air Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float airMoveDampPct = .25f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float airStopDampPct = .6f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float airTurnDampPct = 1f; //The pct used to dampen normal turning to go the other way
    [Range(0, 1)] //Kept as a pct value
    public float dashStopDampPct = .6f; //The pct used to dampen dash decceleration/stopping
    #endregion

    #region JUMPING
    [Header("Jumping")]
    public float jumpForce = 6.5f;
    public int extraJumpValue = 1;
    [Range(0, 1)]
    public float jumpHeightCutPct = .5f;
    public float coyoteTimeValue = .2f;
    #endregion

    #region WALL
    // * //
    #region SLIDING
    [Space] //* WALL SLIDING
    public float wallSlidingSpeed = 1.5f;

    //* Hidden in Inspector
    [HideInInspector]
    public bool isWallSliding;
    #endregion

    #region JUMPING
    [Space] //* WALL JUMPING
    public float xWJCutPct = .25f;
    public float xWJForce = 3;
    public float yWJForce = 6;
    public float wallJumpTime = .05f;

    //* Hidden in Inspector
    [HideInInspector]
    public bool isWallJumping;
    #endregion
    #endregion

    #region ROOF HANGING
    public float hangStaminaValue;

    [HideInInspector]
    public float hangStaminaTimer;
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

        #region Ledge Grabbing
        bool isTouchingLedge = playerCollision.isOnLedge;
        bool isTouchingWall = playerCollision.isOnWall;


        if (!isTouchingLedge && isTouchingWall && Input.GetKey(KeyCode.Y) && !isDashing) //If we are touching the wall but not the ledge (aka at the top of the wall/ledge) and not dashing (so we can move)
        {
            playerCollision.isLedgeGrabbing = true; //We are now grabbing the ledge
        }
        else //otherwise
        {
            playerCollision.isLedgeGrabbing = false; //we arent grabbing the ledge
        }

        if (playerCollision.isLedgeGrabbing) //if we are grabbing the ledge
        {
            rb.velocity = Vector2.zero; //stop moving
        }

        if (playerCollision.isLedgeGrabbing && Input.GetKeyDown(KeyCode.U))
        {
            if (xRawInput != 0 || yRawInput != 0)
            {
                playerCollision.isLedgeGrabbing = false;
                Dash(xRawInput, yRawInput);
            }
        }

        #endregion

        #region Roof Hanging
        bool isTouchingRoof = playerCollision.isOnRoof;

        if (isTouchingRoof && Input.GetKey(KeyCode.Y) && !isDashing && !playerCollision.isOnWall) //If we are touching the roof, holding grab key, and not dashing (so we can move)
        {
            if (!playerCollision.isRoofHanging)
            {

            }

            hangStaminaTimer -= Time.deltaTime;
            playerCollision.isRoofHanging = true; //We are now grabbing the ledge
        }
        else //otherwise
        {
            playerCollision.isRoofHanging = false; //we arent grabbing the ledge
        }

        if (playerCollision.isRoofHanging) //if we are grabbing the ledge
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); //stop moving
        }

        playerUIManager.SetStamina((hangStaminaTimer / hangStaminaValue) * 100);
        #endregion

        #region Wall Sliding
        if (isTouchingWall && !playerCollision.isGrounded && xRawInput != 0 && !playerCollision.isLedgeGrabbing) //if we are touching the wall, arent grounded, moving (towards the wall), and not grabbing the ledge
        {
            isWallSliding = true; //we are wall sliding
        }
        else //otherwise
        {
            isWallSliding = false; //we arent wall sliding
        }

        if (isWallSliding) //if we are wall sliding
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue)); //Move down the wall at a speed clamped between sliding speed and the max possble float
        }
        #endregion

        #region Wall Jumping
        if (Input.GetKeyDown(KeyCode.T) && isWallSliding && !playerCollision.isLedgeGrabbing)
        {
            isWallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }

        if (isWallJumping)
        {
            rb.velocity = new Vector2(xWJForce * -xRawInput, yWJForce);
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

    void SetWallJumpingToFalse()
    {
        isWallJumping = false;
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
