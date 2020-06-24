using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : PlayerController
{
    #region Variables
    public Image circleBar;
    public Text circleBarText;

    public Text scaleText;
    #endregion

    public override void Update()
    {
        base.Update();
    }

    public void SetStamina(float value)
    {
        if (circleBar == null)
        {
            print("**NULL**");
            return;
        }
        circleBar.fillAmount = value;
    }
}