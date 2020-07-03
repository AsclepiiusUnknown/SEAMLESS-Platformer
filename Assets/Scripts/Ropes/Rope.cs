using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    #region Variables
    //*PUBLIC//
    public float ropeSegLen = .25f;
    public int segmentCount = 35;
    public float lineWidth = .1f;
    public int constraintInvokeAmount = 50;
    public Vector2 gravityForce = new Vector2(0, -1f);

    //*PRIVATE//
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    #endregion


    #region Default Setup
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < segmentCount; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }
    }
    #endregion

    #region Updates
    private void Update()
    {
        this.DrawRope();
    }
    private void FixedUpdate()
    {
        this.Simulate();
    }
    #endregion

    #region Simulation
    void Simulate()
    {
        #region Simulate rope effect using gravity on the segments
        for (int i = 0; i < this.segmentCount; i++)
        {
            RopeSegment thisSegment = this.ropeSegments[i];
            Vector2 velocity = thisSegment.posNow - thisSegment.posOld;
            thisSegment.posOld = thisSegment.posNow;
            thisSegment.posNow += velocity;
            thisSegment.posNow += gravityForce * Time.deltaTime;
            this.ropeSegments[i] = thisSegment;
        }
        #endregion

        #region Apply simulation constraint ('x' amount of times)
        int x = constraintInvokeAmount;
        for (int i = 0; i < x; i++)
        {
            this.ApplyConstraint();
        }
        #endregion
    }
    #endregion

    #region Constraints
    void ApplyConstraint()
    {
        #region 1. First segment always follows mouse position
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.ropeSegments[0] = firstSegment;
        #endregion

        #region 2. Segments must always be an equal and predetermined distance (and length)
        for (int i = 0; i < this.segmentCount - 1; i++)
        {
            RopeSegment segment1 = this.ropeSegments[i];
            RopeSegment segment2 = this.ropeSegments[i + 1];

            float dist = (segment1.posNow - segment2.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);

            error = dist - ropeSegLen;
            Vector2 changeDir = (segment1.posNow - segment2.posNow).normalized;
            Vector2 changeAmount = changeDir * error;

            if (i != 0)
            {
                segment1.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = segment1;
                segment2.posNow += changeAmount * .5f;
                this.ropeSegments[i + 1] = segment2;
            }
            else
            {
                segment2.posNow += changeAmount;
                this.ropeSegments[i + 1] = segment2;
            }
        }
        #endregion
    }
    #endregion

    #region Draw rope segment visuals
    void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentCount];
        for (int i = 0; i < this.segmentCount; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }
    #endregion

    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
