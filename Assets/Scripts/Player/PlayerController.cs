using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("General")]
    public int respawns = 3;
    public float respawnWaitTime = 3;

    protected PlayerCollisions playerCollision;
    protected PlayerUIManager playerUIManager;
    //? Is this NEEDED with the current art?
    protected PlayerAnimation playerAnimation;
    protected PlayerStateMachine playerStateMachine;

    protected CameraShake cameraShake;

    protected GameObject playerGO;


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

        playerGO = this.gameObject;
    }
    #endregion

    private void OnBecameInvisible()
    {
        if (!isDead)
        {
            // Die();
        }
    }

    private void Die()
    {
        isDead = true;

        if (respawns > 0)
        {
            respawns--;
            StopAllCoroutines();
            StartCoroutine(Respawn(respawnWaitTime, Vector2.zero, playerGO));
        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }


    public IEnumerator Respawn(float waitTime, Vector2 pos, GameObject player)
    {
        yield return new WaitForSeconds(waitTime);

        isDead = false;
        Instantiate(player, pos, Quaternion.identity);
    }
}