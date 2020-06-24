using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : PlayerController
{
    #region Variables
    ///
    #region GROUND
    [Header("Ground")] //* GROUNDING
    public LayerMask whatIsGround;
    public float grndLeewayValue = .4f;

    //* Hidden in Inspector
    [HideInInspector]
    public bool isGrounded; //the bool used to set and check whether we are currently grounded
    [HideInInspector]
    public float grndLeewayKeeper;
    [HideInInspector]
    public bool isOnGround;
    #endregion

    #region WALL
    [Header("Wall")]
    public LayerMask whatIsWall;
    public float frontCheckRadius = .2f;
    [HideInInspector]
    public bool isOnWall;
    #endregion

    #region LEDGE
    [Header("Ledge")] //* LEDGE GRABBING
    public LayerMask whatIsLedge;
    public float grabCheckRadius = .2f;

    //* Hidden in Inspector
    [HideInInspector]
    public bool isLedgeGrabbing;
    [HideInInspector]
    public bool isOnLedge;
    #endregion

    #region Roof
    [Header("Roof")] //* ROOF HANGING
    public LayerMask whatIsRoof;
    public float hangCheckRadius = .2f;

    //* Hidden in Inspector
    [HideInInspector]
    public bool isRoofHanging;
    [HideInInspector]
    public bool isOnRoof;
    #endregion

    #region Enemy
    //* Hidden in Inspector
    [HideInInspector]
    public bool isOnEnemy;
    #endregion

    #region MISC.
    [Header("Misc.")]
    public float collisionRadius = 0.15f;
    public Vector2 bottomOffset, rightOffset, leftOffset, topRightOffset, topLeftOffset;
    public Vector2 tempBottomOffset, tempRightOffset, tempLeftOffset, tempTopRightOffset, tempTopLeftOffset;
    public Color debugCollisionColor = new Color32();
    #endregion
    #endregion


    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        #region Set Contacts
        if (playerStateMachine.colors.purpleMovement != null && playerStateMachine.colors.purpleMovement.enabled)
        {
            tempBottomOffset = bottomOffset * playerStateMachine.colors.purpleMovement.gravityContainer;
            tempRightOffset = rightOffset * playerStateMachine.colors.purpleMovement.gravityContainer;
            tempLeftOffset = leftOffset * playerStateMachine.colors.purpleMovement.gravityContainer;
            tempTopRightOffset = topRightOffset * playerStateMachine.colors.purpleMovement.gravityContainer;
            tempTopLeftOffset = topLeftOffset * playerStateMachine.colors.purpleMovement.gravityContainer;

            isOnGround = Physics2D.OverlapCircle((Vector2)transform.position + tempBottomOffset, collisionRadius, whatIsGround);

            isOnWall = Physics2D.OverlapCircle((Vector2)transform.position + tempRightOffset, collisionRadius, whatIsWall) || Physics2D.OverlapCircle((Vector2)transform.position + tempLeftOffset, collisionRadius, whatIsWall);

            isOnLedge = Physics2D.OverlapCircle((Vector2)transform.position + tempTopRightOffset, collisionRadius, whatIsLedge) || Physics2D.OverlapCircle((Vector2)transform.position + tempTopLeftOffset, collisionRadius, whatIsLedge);

            isOnRoof = Physics2D.OverlapCircle((Vector2)transform.position + tempTopRightOffset, collisionRadius, whatIsRoof) || Physics2D.OverlapCircle((Vector2)transform.position + tempTopLeftOffset, collisionRadius, whatIsRoof);
        }
        else
        {
            isOnGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, whatIsGround);

            isOnWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, whatIsWall) || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, whatIsWall);

            isOnLedge = Physics2D.OverlapCircle((Vector2)transform.position + topRightOffset, collisionRadius, whatIsLedge) || Physics2D.OverlapCircle((Vector2)transform.position + topLeftOffset, collisionRadius, whatIsLedge);

            isOnRoof = Physics2D.OverlapCircle((Vector2)transform.position + topRightOffset, collisionRadius, whatIsRoof) || Physics2D.OverlapCircle((Vector2)transform.position + topLeftOffset, collisionRadius, whatIsRoof);
        }
        #endregion


        isGrounded = isOnGround;
        grndLeewayKeeper -= Time.deltaTime;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugCollisionColor;

        var positions = new Vector2[] { bottomOffset, topRightOffset, topLeftOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + topRightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + topLeftOffset, collisionRadius);
    }
}
