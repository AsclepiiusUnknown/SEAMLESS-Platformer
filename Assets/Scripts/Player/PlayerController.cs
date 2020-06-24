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
    }
    #endregion

    public virtual void Update()
    {
        #region State Cycling
        /**#if UNITY_EDITOR //!REMOVE when implemented colored platforms
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (colorState == ColorStates.Red)
                    {
                        colorState = ColorStates.Yellow;
                    }
                    else if (colorState == ColorStates.Yellow)
                    {
                        colorState = ColorStates.Green;
                    }
                    else if (colorState == ColorStates.Green)
                    {
                        colorState = ColorStates.Blue;
                    }
                    else if (colorState == ColorStates.Blue)
                    {
                        colorState = ColorStates.Purple;
                    }
                    else if (colorState == ColorStates.Purple)
                    {
                        colorState = ColorStates.Red;
                    }

                    print("Up " + colorState.ToString());
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (colorState == ColorStates.Red)
                    {
                        colorState = ColorStates.Purple;
                    }
                    else if (colorState == ColorStates.Yellow)
                    {
                        colorState = ColorStates.Red;
                    }
                    else if (colorState == ColorStates.Green)
                    {
                        colorState = ColorStates.Yellow;
                    }
                    else if (colorState == ColorStates.Blue)
                    {
                        colorState = ColorStates.Green;
                    }
                    else if (colorState == ColorStates.Purple)
                    {
                        colorState = ColorStates.Blue;
                    }
                    print("Down " + colorState.ToString());
                }
#endif*/
        #endregion
    }

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