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


    [Header("Effects")]
    public float colorShakeMagnitude = .25f;
    public float colorShakeDuration = .15f;
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
        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                if (playerAnimation.spriteRenderer.color != colors[i].primaryColor)
                {
                    RedMovement M = colorScripts.redMovement;
                    M.rb.velocity = new Vector2(M.rb.velocity.x, M.jumpForce);
                }

                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Looping
        while (colorState == ColorStates.Red)
        {
            yield return 0;
        }
        #endregion

        AutomateExit("Red");
    }
    #endregion

    #region YELLOW
    public IEnumerator YellowState()
    {
        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                if (playerAnimation.spriteRenderer.color != colors[i].primaryColor)
                {
                    YellowMovement M = colorScripts.yellowMovement;
                    M.rb.velocity = new Vector2(M.rb.velocity.x, M.jumpForce);
                }

                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Looping
        while (colorState == ColorStates.Yellow)
        {
            yield return 0;
        }
        #endregion

        AutomateExit("Yellow");
    }
    #endregion

    #region GREEN
    public IEnumerator GreenState()
    {

        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                if (playerAnimation.spriteRenderer.color != colors[i].primaryColor)
                {
                    GreenMovement M = colorScripts.greenMovement;
                    M.rb.velocity = new Vector2(M.rb.velocity.x, M.jumpForce);
                }

                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Looping
        while (colorState == ColorStates.Green)
        {
            yield return 0;
        }
        #endregion

        AutomateExit("Green");
    }
    #endregion

    #region BLUE
    public IEnumerator BlueState()
    {
        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                if (playerAnimation.spriteRenderer.color != colors[i].primaryColor)
                {
                    BlueMovement M = colorScripts.blueMovement;
                    M.rb.velocity = new Vector2(M.rb.velocity.x, M.jumpForce);
                }

                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Looping
        while (colorState == ColorStates.Blue)
        {
            yield return 0;
        }
        #endregion

        AutomateExit("Blue");
    }
    #endregion

    #region PURPLE
    public IEnumerator PurpleState()
    {
        #region Sprite Colors
        for (int i = 0; i < colors.Length; i++)
        {
            if (colors[i].name == colorState.ToString())
            {
                if (playerAnimation.spriteRenderer.color != colors[i].primaryColor)
                {
                    PurpleMovement M = colorScripts.purpleMovement;
                    M.rb.velocity = new Vector2(M.rb.velocity.x, M.jumpForce);
                }

                playerAnimation.spriteRenderer.color = colors[i].primaryColor;
                playerAnimation.rimRenderer.color = colors[i].secondaryColor;
            }
        }
        #endregion

        #region Looping
        while (colorState == ColorStates.Purple)
        {
            yield return 0;
        }
        #endregion

        AutomateExit("Red");
    }
    #endregion
    #endregion

    #region State Automation
    public void AutomateEnter(string color)
    {
        #region Resets
        Time.timeScale = 1;
        if (playerUIManager.circleBar != null)
            playerUIManager.circleBar.fillAmount = 1;
        #endregion

        #region Initialisation
        Debug.Log(color + ": Enter");
        colorState = (ColorStates)System.Enum.Parse(typeof(ColorStates), color);

        #region Enabling
        colorScripts.redMovement.enabled = (color == "Red") ? true : false;
        colorScripts.yellowMovement.enabled = (color == "Yellow") ? true : false;
        colorScripts.greenMovement.enabled = (color == "Green") ? true : false;
        colorScripts.blueMovement.enabled = (color == "Blue") ? true : false;
        colorScripts.purpleMovement.enabled = (color == "Purple") ? true : false;
        #endregion
        #endregion
    }

    public void AutomateExit(string color)
    {
        #region Exit
        Debug.Log(color + ": Exit");
        #endregion
    }
    #endregion

    #region Block/Color Detection
    private void OnCollisionEnter2D(Collision2D other)
    {
        //print("HIT");
        GameObject otherGO = other.gameObject;

        if (playerCollision == null)
        {
            print("**NULL**");
            return;
        }

        if (otherGO.tag == "Ground" && otherGO.GetComponent<PlatformController>() != null)
        {
            PlatformController platform = otherGO.GetComponent<PlatformController>();
            //print("Hit ground with color: " + platform.colors[platform.colorIndex]);

            if (colorState.ToString() != colors[platform.colorIndex].name)
            {
                ChangeState(colors[platform.colorIndex].name);
                StartCoroutine(cameraShake.Shake(colorShakeDuration, colorShakeMagnitude));
            }
        }
    }
    #endregion

    #region State Changing
    public void ChangeState(string color)
    {
        StopAllCoroutines();

        AutomateEnter(color);
        StartCoroutine(color + "State");
        print("changed to " + color);
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