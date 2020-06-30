using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMovement : PlayerController
{
    #region Variables
    // * //
    #region DAMPING
    [Header("Ground Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float moveDampPct = .45f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float stopDampPct = .25f; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float turnDampPct = 0; //The pct used to dampen normal turning to go the other way


    [Header("Air Damping")]
    [Range(0, 1)] //Kept as a pct value
    public float airMoveDampPct = .5f; //The pct used to dampen normal acceleration/movement
    [Range(0, 1)] //Kept as a pct value
    public float airStopDampPct = 0; //The pct used to dampen normal decceleration/stopping 
    [Range(0, 1)] //Kept as a pct value
    public float airTurnDampPct = 1; //The pct used to dampen normal turning to go the other way
    #endregion


    #region JUMPING
    [Header("Jumping")]
    public float jumpForce = 6.3f;
    [Range(0, 1)]
    public float jumpHeightCutPct = .5f;
    public float coyoteTimeValue = .2f;
    #endregion


    #region MISC.
    [Header("MISC")]
    public float speedMultiplier = .975f; //Multiplier used to change the overall speed (1 = no difference)
    public float dampElapseTime = 12; //How long the damping is spread over time (can be refined like the damp pct values above)

    //* PRIVATE //
    [HideInInspector]
    public float xRawInput; //The integer input between -1 and 1 to move and check movements on x
    [HideInInspector]
    public float yRawInput; //The integer input between -1 and 1 to move and check movements on y
    [HideInInspector]
    public Rigidbody2D rb; //The players rigidbody
    [HideInInspector]
    private float CoyoteTimeKeeper;
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
        rb.gravityScale = 1;
        CoyoteTimeKeeper = coyoteTimeValue;
    }
    #endregion

    private void OnEnable()
    {
        rb.gravityScale = 1;
    }

    public void Update()
    {
        #region Jumping
        CoyoteTimeKeeper -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.T))
        {
            CoyoteTimeKeeper = coyoteTimeValue;
        }

        if (playerCollision.isGrounded == true)
        {
            playerCollision.grndLeewayKeeper = playerCollision.grndLeewayValue;
        }

        if (CoyoteTimeKeeper > 0 && playerCollision.grndLeewayKeeper > 0)
        {
            CoyoteTimeKeeper = 0;
            playerCollision.grndLeewayKeeper = 0;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            playerCollision.isOnEnemy = false;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 direction = other.transform.position - transform.position;

        if (other.gameObject.tag == "Enemy" && playerCollision.isOnEnemy)
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            enemy.Die();

            if (enemy.enabled == false)
            {
                if (CoyoteTimeKeeper > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce / 3 * 4);
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce / 3 * 2);
                }
            }
        }
    }
}
