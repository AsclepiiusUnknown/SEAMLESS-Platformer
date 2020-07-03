using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    SpringJoint2D springJoint;
    public float distance = 10f;
    public LayerMask whatCanGrapple;
    public LineRenderer grappleLine;
    public float minDist = 1;
    public float stepSize = .2f;


    void Start()
    {
        springJoint = GetComponent<SpringJoint2D>();
        springJoint.enabled = false;
        grappleLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (springJoint.distance > minDist)
        {
            springJoint.distance -= stepSize;
        }
        else
        {
            grappleLine.enabled = false;
            springJoint.enabled = false;

        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPos - transform.position, distance, whatCanGrapple);

            if (hit.collider != null)
            {
                springJoint.enabled = true;
                //	Debug.Log (hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
                Vector2 connectPoint = hit.point - new Vector2(hit.collider.transform.position.x, hit.collider.transform.position.y);
                // connectPoint.x = connectPoint.x / hit.collider.transform.localScale.x;
                // connectPoint.y = connectPoint.y / hit.collider.transform.localScale.y;
                // Debug.Log(connectPoint);

                springJoint.connectedAnchor = hit.point;
                //		joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
                //springJoint.distance = Vector2.Distance(transform.position, hit.point);

                grappleLine.enabled = true;
                grappleLine.SetPosition(0, transform.position);
                grappleLine.SetPosition(1, hit.point);
            }
            else
            {
                print("ERROR");
                //print(hit.collider);
            }
        }

        if (springJoint.enabled && grappleLine.GetPosition(0) != transform.position)
        {
            grappleLine.SetPosition(0, transform.position);
        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            springJoint.enabled = false;
            grappleLine.enabled = false;
        }

    }
}