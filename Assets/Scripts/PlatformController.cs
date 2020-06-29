using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public PlayerColor[] colors;
    public SpriteRenderer thisRenderer;
    public int colorIndex;
    public bool useColorIndex;

    public PlatformController otherBlock;
    public bool useOtherBlockColor;

    private void Start()
    {
        if (thisRenderer == null)
        {
            thisRenderer = GetComponent<SpriteRenderer>();
        }

        if (useOtherBlockColor && otherBlock != null)
        {
            thisRenderer.color = otherBlock.thisRenderer.color;
            colorIndex = otherBlock.colorIndex;
        }
        else
        {
            if (!useColorIndex)
            {
                colorIndex = Random.Range(0, colors.Length);
            }

            thisRenderer.color = colors[colorIndex].primaryColor;
        }
    }

    private void Update()
    {
        if (useOtherBlockColor && otherBlock != null && thisRenderer.color != otherBlock.thisRenderer.color)
        {
            thisRenderer.color = otherBlock.thisRenderer.color;
        }
    }
}