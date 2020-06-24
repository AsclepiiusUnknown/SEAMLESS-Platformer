using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : PlayerController
{
    #region Variables
    public Image staminaBar;

    public Text timeScaleDisplay;

    [HideInInspector]
    public float staminaFill;
    #endregion

    public override void Update()
    {
        base.Update();
        //staminaBar.fillAmount = .75f;
    }

    public void SetStamina(float value)
    {
        staminaBar.fillAmount = value;
    }
}