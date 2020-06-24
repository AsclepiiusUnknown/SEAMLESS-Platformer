using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : PlayerController
{
    #region Variables
    [Header("State Machine")]
    public ColorStates colorState;


    [Header("Colors")]
    public Colors colors;
    #endregion


    public override void Start()
    {
        colors.redMovement = GetComponent<RedMovement>();
        colors.yellowMovement = GetComponent<YellowMovement>();
        colors.greenMovement = GetComponent<GreenMovement>();
        colors.blueMovement = GetComponent<BlueMovement>();
        colors.purpleMovement = GetComponent<PurpleMovement>();

        ChangeState(colorState.ToString());
        print(colorState);
    }

    #region State Machine
    // * //
    #region RED
    public IEnumerator RedState()
    {
        #region Initialisation
        Debug.Log("RED: Enter");
        colorState = ColorStates.Red;

        #region Enabling
        colors.redMovement.enabled = true;
        colors.yellowMovement.enabled = false;
        colors.greenMovement.enabled = false;
        colors.blueMovement.enabled = false;
        colors.purpleMovement.enabled = false;
        #endregion
        #endregion

        #region Looping
        while (colorState == ColorStates.Red)
        {
            yield return 0;
        }
        #endregion

        #region Exit
        Debug.Log("RED: Exit");
        //ChangeState(colorState.ToString());
        #endregion
    }
    #endregion

    #region YELLOW
    public IEnumerator YellowState()
    {
        #region Initialisation
        Debug.Log("YELLOW: Enter");
        colorState = ColorStates.Yellow;

        #region Enabling
        colors.redMovement.enabled = false;
        colors.yellowMovement.enabled = true;
        colors.greenMovement.enabled = false;
        colors.blueMovement.enabled = false;
        colors.purpleMovement.enabled = false;
        #endregion
        #endregion

        #region Looping
        while (colorState == ColorStates.Yellow)
        {
            yield return 0;
        }
        #endregion

        #region Exit
        Debug.Log("YELLOW: Exit");
        //ChangeState(colorState.ToString());
        #endregion
    }
    #endregion

    #region GREEN
    public IEnumerator GreenState()
    {
        #region Initialisation
        Debug.Log("GREEN: Enter");
        colorState = ColorStates.Green;

        #region Enabling
        colors.redMovement.enabled = false;
        colors.yellowMovement.enabled = false;
        colors.greenMovement.enabled = true;
        colors.blueMovement.enabled = false;
        colors.purpleMovement.enabled = false;
        #endregion
        #endregion

        #region Looping
        while (colorState == ColorStates.Green)
        {
            yield return 0;
        }
        #endregion

        #region Exit
        Debug.Log("GREEN: Exit");
        //ChangeState(colorState.ToString());
        #endregion
    }
    #endregion

    #region BLUE
    public IEnumerator BlueState()
    {
        #region Initialisation
        Debug.Log("BLUE: Enter");
        colorState = ColorStates.Blue;

        #region Enabling
        colors.redMovement.enabled = false;
        colors.yellowMovement.enabled = false;
        colors.greenMovement.enabled = false;
        colors.blueMovement.enabled = true;
        colors.purpleMovement.enabled = false;
        #endregion
        #endregion

        #region Looping
        while (colorState == ColorStates.Blue)
        {
            yield return 0;
        }
        #endregion

        #region Exit
        Debug.Log("BLUE: Exit");
        //ChangeState(colorState.ToString());
        #endregion
    }
    #endregion

    #region PURPLE
    public IEnumerator PurpleState()
    {
        #region Initialisation
        Debug.Log("PURPLE: Enter");
        colorState = ColorStates.Purple;

        #region Enabling
        colors.redMovement.enabled = false;
        colors.yellowMovement.enabled = false;
        colors.greenMovement.enabled = false;
        colors.blueMovement.enabled = false;
        colors.purpleMovement.enabled = true;
        #endregion
        #endregion

        #region Looping
        while (colorState == ColorStates.Purple)
        {
            yield return 0;
        }
        #endregion

        #region Exit
        Debug.Log("PURPLE: Exit");
        //ChangeState(colorState.ToString());
        #endregion
    }
    #endregion
    #endregion

    #region State Changing
    public void ChangeState(string stateName)
    {
        StopAllCoroutines();
        StartCoroutine(stateName + "State");
        print("changed to " + stateName);
    }
    #endregion
}
public enum ColorStates
{
    Red,
    Yellow,
    Green,
    Blue,
    Purple
}

[System.Serializable]
public struct Colors
{
    [Header("Red Attributes")]
    public RedMovement redMovement;

    [Header("Yellow Attributes")]
    public YellowMovement yellowMovement;

    [Header("Green Attributes")]
    public GreenMovement greenMovement;

    [Header("Blue Attributes")]
    public BlueMovement blueMovement;

    [Header("Purple Attributes")]
    public PurpleMovement purpleMovement;
}