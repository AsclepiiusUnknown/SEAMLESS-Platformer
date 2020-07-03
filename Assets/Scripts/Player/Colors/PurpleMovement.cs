using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleMovement : PlayerController
{
    #region Variables
    // * //
    #region DAMPING
    [Header("Ground Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float moveDampPct = .3f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float stopDampPct = .7f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float turnDampPct = 1; //The pct used to dampen normal turning to go the other way


    [Header("Air Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float airMoveDampPct = .35f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float airStopDampPct = .6f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float airTurnDampPct = .85f; //The pct used to dampen normal turning to go the other way
    #endregion


    #region JUMPING
    [Header("Jumping")]
    public float jumpForce = 6.4f;
    public int extraJumpValue = 1;
    [Range(0, 1)]
    public float jumpHeightCutPct = .5f;
    public float coyoteTimeValue = .2f;
    #endregion


    #region GRAVITY CONTROL
    [Header("Gravity Control")]
    public int extraGravSwitchValue = 1;
    public float normalGravity = .8f;

    //* PRIVATE //
    [HideInInspector]
    public int gravityContainer;
    [HideInInspector]
    public int extraGravSwitchKeeper;
    #endregion


    #region STAMINA
    public float zeroCostPct = .15f;
    public float waitTime = 1;
    //*PRIVATE//
    [HideInInspector]
    public float circleFillAmount;
    #endregion


    #region MISC.
    [Header("MISC")]
    public float speedMultiplier = .915f; //Multiplier used to change the overall speed (1 = no difference)
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
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void Start()
    {
        base.Start();
        rb.gravityScale = normalGravity;

        extraJumpKeeper = extraJumpValue;
        extraGravSwitchKeeper = extraGravSwitchValue;
        CoyoteTimeKeeper = coyoteTimeValue;
    }

    private void OnEnable()
    {
        circleFillAmount = .5f;
        rb.gravityScale = normalGravity;
    }
    #endregion

    public void Update()
    {
        if (!GameManager.canMove) return;

        #region Gravity Checking
        if (rb.gravityScale < 0)
        {
            gravityContainer = -1;
        }
        else if (rb.gravityScale == 0)
        {
            gravityContainer = 0;
            circleFillAmount -= (zeroCostPct * Time.deltaTime) * waitTime;
            if (circleFillAmount <= 0)
            {
                ToggleGravityPause();
            }
        }
        else
        {
            gravityContainer = 1;
        }
        #endregion


        #region Gravity Input
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleGravityPause();
        }

        if (extraGravSwitchKeeper > 0 && Input.GetKeyDown(KeyCode.U))
        {
            SwitchGravity();
        }

        #endregion


        #region Jumping
        if (gravityContainer == 0)
        {
            return;
        }

        CoyoteTimeKeeper -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.T))
        {
            CoyoteTimeKeeper = coyoteTimeValue;
        }

        if (playerCollision.isGrounded == true)
        {
            playerCollision.grndLeewayKeeper = playerCollision.grndLeewayValue;
            extraJumpKeeper = extraJumpValue;
            extraGravSwitchKeeper = extraGravSwitchValue;
        }

        if (CoyoteTimeKeeper > 0 && playerCollision.grndLeewayKeeper > 0)
        {
            CoyoteTimeKeeper = 0;
            playerCollision.grndLeewayKeeper = 0;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce * gravityContainer);
        }
        else if (Input.GetKeyDown(KeyCode.T) && extraJumpKeeper > 0 && !playerCollision.isGrounded && !playerCollision.isLedgeGrabbing) //ledge grabbing?
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * gravityContainer);

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
        if (!GameManager.canMove) return;

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
                xRawInput *= Mathf.Pow(1f - airStopDampPct, Time.deltaTime * dampElapseTime);
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

    #region Gravity Control
    public void ToggleGravityPause()
    {
        if (gravityContainer > 0 && (circleFillAmount - (zeroCostPct * Time.deltaTime) * waitTime) > 0)
        {
            gravityScaleKeeper = rb.gravityScale;

            rb.gravityScale = 0;
            gravityContainer = 0;
            circleFillAmount = 1;
        }
        else
        {
            rb.gravityScale = gravityScaleKeeper;
            gravityContainer = Mathf.FloorToInt(Mathf.Sign(gravityScaleKeeper));
            circleFillAmount = .5f;
        }

    }

    public void SwitchGravity()
    {
        rb.gravityScale *= -1;
        gravityContainer *= -1;

        if (playerCollision.grndLeewayKeeper < 0)
        {
            extraGravSwitchKeeper--;
        }


    }
    #endregion
}
