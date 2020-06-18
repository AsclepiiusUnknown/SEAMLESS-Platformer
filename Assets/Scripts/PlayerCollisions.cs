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

    #region MISC.
    [Header("Misc.")]
    public float collisionRadius = 0.15f;
    public Vector2 bottomOffset, rightOffset, leftOffset, topRightOffset, topLeftOffset;
    public Color debugCollisionColor = new Color32();
    #endregion
    #endregion


    // Update is called once per frame
    void Update()
    {
        #region Set Contacts
        isOnGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, whatIsGround);

        isOnWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, whatIsWall) || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, whatIsWall);

        isOnLedge = Physics2D.OverlapCircle((Vector2)transform.position + topRightOffset, collisionRadius, whatIsLedge) || Physics2D.OverlapCircle((Vector2)transform.position + topLeftOffset, collisionRadius, whatIsLedge);
        #endregion

        isGrounded = isOnGround;
        grndLeewayKeeper -= Time.deltaTime;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = debugCollisionColor;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

        Gizmos.DrawWireSphere((Vector2)transform.position + topRightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + topLeftOffset, collisionRadius);
    }
}
