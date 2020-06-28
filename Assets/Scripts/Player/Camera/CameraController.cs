using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform target;
    public float smoothSpeed = 0.0625f;
    public Vector3 offset;
    public bool canFollow = true;
    public Vector2 topLeftBound;
    public Vector2 bottomRightBound;

    private BoxCollider2D boxCollider;


    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        #region Debug & Set Target
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            canFollow = true;
        }
        #endregion
    }

    private void LateUpdate()
    {
        #region Move to Target Smoothly
        if (canFollow && target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            if ((bottomRightBound.y < smoothedPosition.y && smoothedPosition.y < topLeftBound.y) && (bottomRightBound.x > smoothedPosition.y && smoothedPosition.x > topLeftBound.x))
            {
                transform.position = smoothedPosition;
            }
        }
        #endregion
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(topLeftBound.x, topLeftBound.y, 0), .5f);
        Gizmos.DrawWireSphere(new Vector3(bottomRightBound.x, bottomRightBound.y, 0), .5f);
    }
}
