using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlEnemyAI : MonoBehaviour
{
    public float speed;
    public LayerMask whatIsPlatform;

    //private bool movingRight = true;

    public Vector2 offset1, offset2, offset3, offset4;
    public float collisionRadius = 0.15f;

    bool touch1, touch2, touch3, touch4;



    private void Update()
    {
        touch1 = Physics2D.OverlapCircle((Vector2)transform.position + offset1, collisionRadius, whatIsPlatform);
        touch2 = Physics2D.OverlapCircle((Vector2)transform.position + offset2, collisionRadius, whatIsPlatform);
        touch3 = Physics2D.OverlapCircle((Vector2)transform.position + offset3, collisionRadius, whatIsPlatform);
        touch4 = Physics2D.OverlapCircle((Vector2)transform.position + offset4, collisionRadius, whatIsPlatform);

        if (touch3 && touch4) //Touching Bottom
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime); //Move Right
        }

        if (touch2 && touch4) //Touching Right
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime); //Move Up
        }

        if (touch4 && !touch2 && !touch3)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime); //Move Up
        }


    }

    void SwitchOffsets()
    {/*
        Vector2 tempBottom = bottomOffset;
        Vector2 tempRight = rightOffset;

        bottomOffset = tempRight;
        rightOffset = tempBottom;*/
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere((Vector2)transform.position + offset1, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + offset2, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + offset3, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + offset4, collisionRadius);
    }
}
