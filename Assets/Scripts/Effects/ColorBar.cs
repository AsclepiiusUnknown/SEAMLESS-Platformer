using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBar : MonoBehaviour
{
    #region Variables
    public RectTransform[] barScales;

    public Vector3 risenScale;

    private PlayerStateMachine playerStateMachine;
    private Vector3 scaleKeeper;
    #endregion

    private void Start()
    {
        playerStateMachine = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>();

        scaleKeeper = barScales[0].localScale;
    }

    private void Update()
    {
        switch (playerStateMachine.colorState)
        {
            case ColorStates.Red:
                for (int i = 0; i < barScales.Length; i++)
                {
                    barScales[i].localScale = (i == 0) ? risenScale : scaleKeeper;
                }
                break;
            case ColorStates.Yellow:
                for (int i = 0; i < barScales.Length; i++)
                {
                    barScales[i].localScale = (i == 1) ? risenScale : scaleKeeper;
                }
                break;
            case ColorStates.Green:
                for (int i = 0; i < barScales.Length; i++)
                {
                    barScales[i].localScale = (i == 2) ? risenScale : scaleKeeper;
                }
                break;
            case ColorStates.Blue:
                for (int i = 0; i < barScales.Length; i++)
                {
                    barScales[i].localScale = (i == 3) ? risenScale : scaleKeeper;
                }
                break;
            case ColorStates.Purple:
                for (int i = 0; i < barScales.Length; i++)
                {
                    barScales[i].localScale = (i == 4) ? risenScale : scaleKeeper;
                }
                break;
        }
    }
}