using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class PlatformController : MonoBehaviour
{
    public PlayerColor[] colors;
    public SpriteRenderer thisRenderer;
    public int colorIndex;
    public bool useColorIndex;

    public PlatformController otherBlock;
    public bool useOtherBlockColor;

    [Header("Shadown Effect")]
    public Vector2 shadowOffset = new Vector2(-.15f, -.15f);
    public Material shadowMat;
    [MinMaxSlider(-10, 10)]
    public Vector2 shadowOffsetClamping;
    //*PRIVATE//
    private SpriteRenderer sasterSprRnd;
    private SpriteRenderer shadowSprRnd;
    private Transform casterTrans;
    private Transform shadowTrans;
    private Transform cameraTrans;



    private void Start()
    {
        if (thisRenderer == null)
        {
            thisRenderer = GetComponent<SpriteRenderer>();
        }

        if (useOtherBlockColor && otherBlock != null)
        {
            thisRenderer.color = otherBlock.thisRenderer.color;
        }
        else
        {
            if (!useColorIndex)
            {
                colorIndex = Random.Range(0, colors.Length);
            }

            thisRenderer.color = colors[colorIndex].primaryColor;
        }

        #region Shadow Setup
        cameraTrans = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        casterTrans = transform;
        shadowTrans = new GameObject().transform;
        shadowTrans.parent = casterTrans;
        shadowTrans.gameObject.name = casterTrans.gameObject.name + " Shadow";

        sasterSprRnd = GetComponent<SpriteRenderer>();
        shadowSprRnd = shadowTrans.gameObject.AddComponent<SpriteRenderer>();
        shadowTrans.localRotation = Quaternion.identity;

        shadowSprRnd.material = shadowMat;
        shadowSprRnd.color = colors[colorIndex].secondaryColor;
        shadowSprRnd.sortingLayerName = sasterSprRnd.sortingLayerName;
        shadowSprRnd.sortingOrder = sasterSprRnd.sortingOrder - 1;

        shadowTrans.localScale = new Vector3(1, 1, 1);
        #endregion
    }

    private void Update()
    {
        if (useOtherBlockColor && otherBlock != null && thisRenderer.color != otherBlock.thisRenderer.color)
        {
            thisRenderer.color = otherBlock.thisRenderer.color;
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].primaryColor == otherBlock.thisRenderer.color)
                {
                    colorIndex = i;
                }
            }
        }

        #region Shadow Color Updates
        if (shadowSprRnd.color != colors[colorIndex].secondaryColor)
        {
            shadowSprRnd.color = colors[colorIndex].secondaryColor;
        }
        #endregion
    }

    #region Shadow Updates
    private void LateUpdate()
    {
        float xCamTemp = Mathf.Clamp(casterTrans.position.x - cameraTrans.position.x, shadowOffsetClamping.x, shadowOffsetClamping.y);
        float yCamTemp = Mathf.Clamp(casterTrans.position.y - cameraTrans.position.y, shadowOffsetClamping.x, shadowOffsetClamping.y);

        float xShadowPos = (casterTrans.position.x + (shadowOffset.x * -xCamTemp));
        float yShadowPos = casterTrans.position.y + (shadowOffset.y * -yCamTemp);

        shadowTrans.position = new Vector2(xShadowPos, yShadowPos);

        shadowSprRnd.sprite = sasterSprRnd.sprite;
    }
    #endregion
}