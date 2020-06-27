using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : PlayerController
{
    #region Variables
    public Image circleBar;
    public TextMeshProUGUI circleBarText;
    private Color cicleBarColor;

    public TextMeshProUGUI scaleText;
    #endregion

    public override void Start()
    {
        cicleBarColor = circleBar.color;
    }

    private void Update()
    {
        if (playerStateMachine == null)
        {
            playerStateMachine = GetComponent<PlayerStateMachine>();
            print("**NULL**");
            return;
        }


        switch (playerStateMachine.colorState)
        {
            case ColorStates.Red:

                break;
            #region Yellow
            case ColorStates.Yellow:
                YellowMovement yellow = playerStateMachine.colorScripts.yellowMovement;

                if (yellow.circleFillAmount != circleBar.fillAmount)
                {
                    circleBar.fillAmount = yellow.circleFillAmount;
                    //print("reduced " + yellow.circleFillAmount);
                }

                circleBarText.text = "Stamina"; //! Optimise
                scaleText.text = "";
                break;
            #endregion

            #region Green
            case ColorStates.Green:
                GreenMovement green = playerStateMachine.colorScripts.greenMovement;

                if (green.circleFillAmount != circleBar.fillAmount)
                {
                    circleBar.fillAmount = green.circleFillAmount;
                    print("reduced " + green.circleFillAmount);
                }

                circleBarText.text = "Stamina"; //! Optimise
                scaleText.text = "";
                break;
            #endregion

            #region Blue
            case ColorStates.Blue:
                BlueMovement blue = playerStateMachine.colorScripts.blueMovement;

                if (blue.circleFillAmount != circleBar.fillAmount)
                {
                    circleBar.fillAmount = blue.circleFillAmount;
                    print("reduced " + blue.circleFillAmount);
                }

                circleBarText.text = "Time Scale: \n" + Time.timeScale; //! Optimise
                scaleText.text = "";
                break;
            #endregion

            #region Purple
            case ColorStates.Purple:
                PurpleMovement purple = playerStateMachine.colorScripts.purpleMovement;

                if (purple.circleFillAmount != circleBar.fillAmount)
                {
                    circleBar.fillAmount = purple.circleFillAmount;
                    print("reduced " + purple.circleFillAmount);
                }

                if (purple.rb.gravityScale < 0 && !circleBar.fillClockwise)
                {
                    circleBar.fillClockwise = true;
                }
                else if (purple.rb.gravityScale > 0 && circleBar.fillClockwise)
                {
                    circleBar.fillClockwise = false;
                }

                circleBarText.text = "Gravity Scale: \n" + purple.rb.gravityScale; //! Optimise
                scaleText.text = "";
                break;
                #endregion
        }
    }

    public IEnumerator NegativeFlash(Image image, Color newColor, int amount)  //! wrong flash behaviour
    {
        Color normalColor = image.color;

        for (int i = 0; i < amount; i++)
        {
            image.color = newColor;

            yield return new WaitForSeconds(1.5f / amount);

            image.color = normalColor;
        }
    }
}