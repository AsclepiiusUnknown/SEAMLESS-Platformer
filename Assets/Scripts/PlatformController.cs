using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public PlayerColor[] colors;
    public SpriteRenderer thisRenderer;
    public int colorIndex;
    public bool useColorIndex;

    private void Start ()
    {
        if (thisRenderer == null)
        {
            thisRenderer = GetComponent<SpriteRenderer> ();
        }

        if (!useColorIndex)
            colorIndex = Random.Range (0, colors.Length);

        thisRenderer.color = colors[colorIndex].primaryColor;
    }
}