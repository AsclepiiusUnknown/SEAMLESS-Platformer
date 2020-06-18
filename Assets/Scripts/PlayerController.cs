using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    protected PlayerMovement playerMovement;

    [Header("Collisions")]
    protected PlayerCollisions playerCollision;

    [Header("Animation")]
    protected PlayerAnimation playerAnimation;
    #endregion

    /**
     * *Important
     * TODO
     * ! ALERT
     * ? QUERY
     * DEAD
     */

    #region Setup
    public virtual void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCollision = GetComponent<PlayerCollisions>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }
    #endregion
}
