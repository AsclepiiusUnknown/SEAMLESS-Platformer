using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgColorCycler : MonoBehaviour
{
    public Color[] colors;
    [HideInInspector]
    public Color color;
    public float changeSpeed = 5;
    int currentIndex = 0;
    public SpriteRenderer sprite;
    bool shouldChange = false;
    public SpriteRenderer topCoverSprite;

    private void Start()
    {
        currentIndex = 0;
        SetColor(colors[currentIndex]);
    }

    private void Update()
    {
        if (shouldChange)
        {
            var startColor = sprite.color;
            var endColor = color;

            var newColor = Color.Lerp(startColor, endColor, Time.deltaTime * changeSpeed);
            SetColor(newColor);

            if (newColor == endColor && shouldChange)
            {
                shouldChange = false;
            }
        }
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
        if (topCoverSprite != null)
            topCoverSprite.color = color;
    }

    public void ProcessColorString(string colorName)
    {
        switch (colorName)
        {
            case "Red":
                color = colors[0];
                break;
            case "Yellow":
                color = colors[1];
                break;
            case "Green":
                color = colors[2];
                break;
            case "Blue":
                color = colors[3];
                break;
            case "Purple":
                color = colors[4];
                break;
        }
        shouldChange = true;
    }
}
