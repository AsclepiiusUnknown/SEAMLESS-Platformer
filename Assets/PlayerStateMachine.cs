using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : PlayerController
{
    #region Variables
    [Header("State Machine")]
    public ColorStates colorState;


    [Header("Colors")]
    public ColorScripts colorScripts;
    public PlayerColor[] colors;
    #endregion


    public override void Start()
    {
        base.Start();
        colorScripts.redMovement = GetComponent<RedMovement>();
        colorScripts.yellowMovement = GetComponent<YellowMovement>();
        colorScripts.greenMovement = GetComponent<GreenMovement>();
        colorScripts.blueMovement = GetComponent<BlueMovement>();
        colorScripts.purpleMovement = GetComponent<PurpleMovement>();

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

        #region Sprite Colors
        /**for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }*/
        #endregion

        #region Enabling
        colorScripts.redMovement.enabled = true;
        colorScripts.yellowMovement.enabled = false;
        colorScripts.greenMovement.enabled = false;
        colorScripts.blueMovement.enabled = false;
        colorScripts.purpleMovement.enabled = false;
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

        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Enabling
        colorScripts.redMovement.enabled = false;
        colorScripts.yellowMovement.enabled = true;
        colorScripts.greenMovement.enabled = false;
        colorScripts.blueMovement.enabled = false;
        colorScripts.purpleMovement.enabled = false;
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

        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Enabling
        colorScripts.redMovement.enabled = false;
        colorScripts.yellowMovement.enabled = false;
        colorScripts.greenMovement.enabled = true;
        colorScripts.blueMovement.enabled = false;
        colorScripts.purpleMovement.enabled = false;
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

        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Enabling
        colorScripts.redMovement.enabled = false;
        colorScripts.yellowMovement.enabled = false;
        colorScripts.greenMovement.enabled = false;
        colorScripts.blueMovement.enabled = true;
        colorScripts.purpleMovement.enabled = false;
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

        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Enabling
        colorScripts.redMovement.enabled = false;
        colorScripts.yellowMovement.enabled = false;
        colorScripts.greenMovement.enabled = false;
        colorScripts.blueMovement.enabled = false;
        colorScripts.purpleMovement.enabled = true;
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

    #region Block/Color Detection
    private void OnCollisionEnter2D(Collision2D other)
    {
        print("HIT");
        GameObject otherGO = other.gameObject;

        if (playerCollision == null)
        {
            print("**NULL**");
            return;
        }

        if (otherGO.tag == "Ground" && otherGO.GetComponent<PlatformController>() != null)
        {
            PlatformController platform = otherGO.GetComponent<PlatformController>();
            print("Hit ground with color: " + platform.colors[platform.colorIndex]);

            ChangeState(colors[platform.colorIndex].name);
        }
    }
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
public struct ColorScripts
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

[System.Serializable]
public struct PlayerColor
{
    public string name;
    public Color32 primaryColor;
    public Color32 secondaryColor;
}