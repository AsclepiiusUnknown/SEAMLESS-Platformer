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
            transform.position = smoothedPosition;
        }
        #endregion
    }
}
