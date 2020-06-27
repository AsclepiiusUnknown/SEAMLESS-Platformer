using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("General")]
    protected PlayerCollisions playerCollision;
    protected PlayerUIManager playerUIManager;
    //? Is this NEEDED with the current art?
    protected PlayerAnimation playerAnimation;
    protected PlayerStateMachine playerStateMachine;

    protected CameraShake cameraShake;


    [Header("MISC.")]
    [HideInInspector]
    protected bool isDead = false;

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
        playerCollision = GetComponent<PlayerCollisions>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerUIManager = GetComponent<PlayerUIManager>();
        playerStateMachine = GetComponent<PlayerStateMachine>();

        cameraShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }
    #endregion

    private void OnBecameInvisible()
    {
        if (!isDead)
            Die();
    }

    private void Die()
    {
        isDead = true;
        //print("Player died.");
        this.gameObject.SetActive(false);
    }



}