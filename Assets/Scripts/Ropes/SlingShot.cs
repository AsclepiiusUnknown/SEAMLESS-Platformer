using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;

    public bool useCollider = true;

    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    public float ropeSegLen = 0.25f;
    public int segmentCount = 35;
    public float lineWidth = 0.1f;

    //Sling Shot
    bool moveToMouse = false;
    Vector3 mousePositionWorld;
    int indexMousePos;
    //Gameobject following
    //public GameObject followObject;

    public EdgeCollider2D edgeCollider;


    // Use this for initialization
    void Start()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = StartPoint.position;

        for (int i = 0; i < segmentCount; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        if (useCollider && edgeCollider != null)
            edgeCollider.enabled = true;
        else
            edgeCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.DrawRope();

        if (Input.GetMouseButtonDown(0))
        {
            this.moveToMouse = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.moveToMouse = false;
        }

        Vector3 screenMousePos = Input.mousePosition;
        float xStart = StartPoint.position.x;
        float xEnd = EndPoint.position.x;
        this.mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, 10));
        float currX = this.mousePositionWorld.x; //followObject.transform.position.x

        float ratio = (currX - xStart) / (xEnd - xStart);
        if (ratio > 0)
        {
            this.indexMousePos = (int)(this.segmentCount * ratio);
        }
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < this.segmentCount; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = this.StartPoint.position;
        this.ropeSegments[0] = firstSegment;

        //Constrant to Second Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = this.EndPoint.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentCount - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
            //*
            Vector2[] colliderPoints = new Vector2[this.segmentCount];
            colliderPoints[i] = this.ropeSegments[i].posNow;

            if (this.moveToMouse && indexMousePos > 0 && indexMousePos < this.segmentCount - 1 && i == indexMousePos)
            {
                RopeSegment thisSegment = this.ropeSegments[i];
                RopeSegment nextSegment = this.ropeSegments[i + 1];
                thisSegment.posNow = new Vector2(this.mousePositionWorld.x, this.mousePositionWorld.y);
                nextSegment.posNow = new Vector2(this.mousePositionWorld.x, this.mousePositionWorld.y);
                // thisSegment.posNow = new Vector2 (this.followObject.transform.position.x, this.followObject.transform.position.y);
                // nextSegment.posNow = new Vector2 (this.followObject.transform.position.x, this.followObject.transform.position.y);
                this.ropeSegments[i] = thisSegment;
                this.ropeSegments[i + 1] = nextSegment;
            }
            edgeCollider.points[i] = colliderPoints[i];
        }
    }

    private void DrawRope()
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

        if (useCollider)
            CreateColliders();
    }

    private void CreateColliders()
    {
        Vector2[] colliderPoints = new Vector2[this.segmentCount];

        for (int i = 0; i < this.segmentCount; i++)
        {
            colliderPoints[i] = this.ropeSegments[i].posNow;
        }

        edgeCollider.points = colliderPoints;
    }

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